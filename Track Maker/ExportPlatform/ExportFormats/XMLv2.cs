// EVERYTHING ABOUT THIS IS TERRIBLE
// IN V2.1 WE WILL MAKE THIS DESERIALISED

using Microsoft.Win32;
using TrackMaker.UI.StringUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Track_Maker
{
    /// <summary>
    /// TProj-v2 format
    /// 
    /// Priscilla v442
    /// 
    /// 2020-09-13
    /// 
    /// Version 2.0      Priscilla v442      Dummy only
    /// Version 2.0b     Priscilla v443      Began functionality
    /// Version 2.0c     Priscilla v445      Based code off of v1 - added metadata and Proj based
    /// Version 2.1      Priscilla v447      Bug fixes, IsSelected & IsOpen
    /// Version 2.2      Priscilla v453      Importing, BasinName => Name
    /// Version 2.2b     Priscilla v455      Add full importing code   
    /// Version 2.2c     Priscilla v540      Changed format name
    /// Version 2.3      Priscilla v550      Changed root node name from "Proj" to "Project" and changed extension from *.tproj to *.tproj2
    /// Version 2.3b     Priscilla v571      Removed AutoStart
    /// Version 2.4      Priscilla v604      PRISCILLA CODEFREEZE: Add local user name for author (2020-12-20)
    /// 
    /// </summary>

    public class ExportXMLv2 : IExportFormat
    {
        public bool DisplayPreview { get; set; }
        public bool DisplayQualityControl { get; set; }
        public string Name { get; set; }

        // Format Version
        public static int FormatVersionMajor = 2;
        public static int FormatVersionMinor = 4;

        public ExportXMLv2()
        {
            Name = "Project";
            GlobalState.CurrentExportFormatName = GetType().ToString();
        }

        /// <summary>
        /// Get the name of the ExportXMLv2 class. 
        /// </summary>
        /// <returns></returns>
        public string GetName() => Name;

        public bool Export(Project Proj)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();

                SFD.Title = "Export to Project...";
                SFD.Filter = "Track Maker Project XML files|*.tproj2";
                SFD.ShowDialog();

                // user hit cancel
                if (SFD.FileName == "") return true;

                // If it exists, delete it
                if (File.Exists(SFD.FileName))
                {
                    File.Delete(SFD.FileName);
                    FileStream FS = File.Create(SFD.FileName);
                    FS.Close();
                }

                // Export.
                ExportCore(Proj, SFD.FileName);

                return true;
            }
            // error checking
            catch (IOException err)
            {
                MessageBox.Show($"An error occurred while writing to XML format. [Error Code: EX1].\n\n{err}");
                Application.Current.Shutdown(-0xE1);
                return false;
            }
            catch (XmlException err)
            {
                MessageBox.Show($"An error occurred while saving the XML file. [Error Code: EX2].\n\n{err}");
                Application.Current.Shutdown(-0xE2);
                return false;
            }
        }

        public bool ExportCore(Project Proj, string FileName)
        {
            XmlDocument XDoc = new XmlDocument();
            XmlNode XRoot = XDoc.CreateElement("Project");
            Proj.FileName = FileName;
            // Version 2.0: Write the metadata
            XmlNode XMetadataNode = XDoc.CreateElement("Metadata");
            // Version of the format. 
            XmlNode XAuthor = XDoc.CreateElement("Author");
            XmlNode XFormatVersionMajor = XDoc.CreateElement("FormatVersionMajor");
            XmlNode XFormatVersionMinor = XDoc.CreateElement("FormatVersionMinor");

            XmlNode XTimestamp = XDoc.CreateElement("Timestamp");

            XAuthor.InnerText = Environment.UserName;
            XFormatVersionMajor.InnerText = FormatVersionMajor.ToString();
            XFormatVersionMinor.InnerText = FormatVersionMinor.ToString();

            // ISO 8601 date format
            XTimestamp.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            XmlNode XBasinsNode = XDoc.CreateElement("Basins"); 

            foreach (Basin Bas in Proj.OpenBasins)
            {
                XmlNode XBasinNode = XDoc.CreateElement("Basin");
                XmlNode XBasinNameNode = XDoc.CreateElement("UserTag");
                XmlNode XBasinNameBasin = XDoc.CreateElement("Name");
                XmlNode XBasinIsOpen = XDoc.CreateElement("IsOpen");
                XmlNode XBasinIsSelected = XDoc.CreateElement("IsSelected");
                XmlNode XBasinLayers = XDoc.CreateElement("Layers");
                
                XBasinNameNode.InnerText = Bas.UserTag;
                XBasinNameBasin.InnerText = Bas.Name;
                XBasinIsOpen.InnerText = Bas.IsOpen.ToString();
                XBasinIsSelected.InnerText = Bas.IsSelected.ToString();
                // build a layer

                foreach (Layer Lay in Bas.Layers)
                {
                    XmlNode XLayerNode = XDoc.CreateElement("Layer");
                    XmlNode XLayerNameNode = XDoc.CreateElement("Name");
                    XmlNode XLayerGUIDNode = XDoc.CreateElement("GUID");
                    XmlNode XStormsNode = XDoc.CreateElement("Storms");

                    XLayerNameNode.InnerText = Lay.Name;
                    XLayerGUIDNode.InnerText = Lay.LayerId.ToString(); 

                    // dump the storm info to file
                    foreach (Storm XStorm in Lay.AssociatedStorms)
                    {
                        // create the xml nodes.
                        XmlNode XStormNode = XDoc.CreateElement("Storm");
                        XmlNode XStormFormationDate = XDoc.CreateElement("FormationDate");
                        XmlNode XStormID = XDoc.CreateElement("ID");
                        XmlNode XStormName = XDoc.CreateElement("Name");
                        XmlNode XStormNodeList = XDoc.CreateElement("Nodes");
                        XmlNode XStormNodeListDel = XDoc.CreateElement("DeletedNodes"); // the undone nodes

                        // set the basic info - name etc
                        XStormFormationDate.InnerText = XStorm.FormationDate.ToString();
                        XStormID.InnerText = XStorm.Id.ToString();
                        XStormName.InnerText = XStorm.Name;

                        // populate the node list
                        foreach (Node XNode in XStorm.NodeList)
                        {
                            // add new nodes
                            XmlNode XNodeNode = XDoc.CreateElement("Node");
                            XmlNode XNodeIntensity = XDoc.CreateElement("Intensity");
                            XmlNode XNodePosition = XDoc.CreateElement("Position");
                            XmlNode XNodeType = XDoc.CreateElement("Type");

                            // set the info
                            XNodeIntensity.InnerText = XNode.Intensity.ToString();
                            XNodePosition.InnerText = XNode.Position.ToStringEmerald();
                            XNodeType.InnerText = XNode.NodeType.Name;

                            // build the node list xml structure
                            XNodeNode.AppendChild(XNodeIntensity);
                            XNodeNode.AppendChild(XNodePosition);
                            XNodeNode.AppendChild(XNodeType);
                            XStormNodeList.AppendChild(XNodeNode);
                        }

                        // Populate the deleted node list xmlinfo for the basin save information.

                        foreach (Node XNode in XStorm.NodeList_Deleted)
                        {
                            // add new nodes
                            XmlNode XNodeNode = XDoc.CreateElement("Node");
                            XmlNode XNodeIntensity = XDoc.CreateElement("Intensity");
                            XmlNode XNodePosition = XDoc.CreateElement("Position");
                            XmlNode XNodeType = XDoc.CreateElement("Type");

                            // set the info
                            XNodeIntensity.InnerText = XNode.Intensity.ToString();
                            XNodePosition.InnerText = XNode.Position.ToStringEmerald();
                            XNodeType.InnerText = XNode.NodeType.Name;

                            // build the node list xml structure
                            XNodeNode.AppendChild(XNodeIntensity);
                            XNodeNode.AppendChild(XNodePosition);
                            XNodeNode.AppendChild(XNodeType);
                            XStormNodeListDel.AppendChild(XNodeNode);

                        }
                        // build perstorm xml

                        XStormNode.AppendChild(XStormFormationDate);
                        XStormNode.AppendChild(XStormID);
                        XStormNode.AppendChild(XStormName);
                        XStormNode.AppendChild(XStormNodeList);
                        XStormNode.AppendChild(XStormNodeListDel);

                        XStormsNode.AppendChild(XStormNode);

                    }

                    XLayerNode.AppendChild(XLayerNameNode);
                    XLayerNode.AppendChild(XLayerGUIDNode);
                    XLayerNode.AppendChild(XStormsNode);

                    XBasinLayers.AppendChild(XLayerNode); 
                }

                // Build the Basins node

                XBasinNode.AppendChild(XBasinNameNode);
                XBasinNode.AppendChild(XBasinNameBasin);
                XBasinNode.AppendChild(XBasinLayers);

                XBasinsNode.AppendChild(XBasinNode);
            }

            // build metadata

            XMetadataNode.AppendChild(XAuthor);
            XMetadataNode.AppendChild(XFormatVersionMajor);
            XMetadataNode.AppendChild(XFormatVersionMinor); 

            XRoot.AppendChild(XMetadataNode);
           
            // build storms
            XRoot.AppendChild(XBasinsNode);

            XDoc.AppendChild(XRoot);

            XDoc.Save(FileName);

            // on success
            GlobalState.SetCurrentOpenFile(FileName);
            return true;
        }

        /// <summary>
        /// I have to compromise against my old shit code
        /// </summary>
        /// <returns>The imported basin.</returns>
        public ImportResult Import()
        {
            // Implement later

            try
            {
                ImportResult IR = new ImportResult();

                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Title = $"Import from {GetName()}";
                OFD.Filter = "Track Maker Project files|*.tproj2";

                OFD.ShowDialog();

                if (OFD.FileName == "")
                {
                    IR.Status = ExportResults.Cancelled;
                    return IR;
                }
                else
                {
                    XMLImportResult__DEPRECATED XER = ImportCore(OFD.FileName);

                    if (XER.Successful && !XER.Cancelled)
                    {
                        IR.Status = ExportResults.OK;
                        IR.Project = XER.Project;
                        return IR;
                    }
                    else
                    {
                        IR.Status = ExportResults.Error;
                        return IR;
                    }

                }

                
            }
            catch (XmlException)
            {
                Error.Throw("Invalid basin!", "One of the basins in this project file is corrupted!", ErrorSeverity.Error, 182);
                return null;
            }
        }
#if PRISCILLA
        public XMLImportResult__DEPRECATED ImportCore(string FileName)
        {
            XMLImportResult__DEPRECATED XER = new XMLImportResult__DEPRECATED();
            XER.Successful = false;
            
            XmlDocument XD = new XmlDocument();
            XD.Load(FileName);

            XmlNode XDR = XD.FirstChild;

            Project Proj = new Project();
            Proj.FileName = FileName;

#if DANO
            Proj.Basins = GlobalState.LoadedBasins;
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            //Proj.Basins = MnWindow.CurrentProject.Basins;
            
#endif
            if (!XDR.HasChildNodes) return XER;

            List<Layer> Layers = new List<Layer>();  

            foreach (XmlNode XDRA in XDR.ChildNodes)
            {
                switch (XDRA.Name)
                {
                    case "Metadata":
                        continue;
                    case "Basins":

                        // Check that there are not no valid basins.
                        if (!XDRA.HasChildNodes)
                        {
                            Error.Throw("No valid basins!", "There are no valid basins!", ErrorSeverity.Error, 181);
                            return XER;
                        }

                        XmlNodeList XDRBList = XDRA.ChildNodes;

                        foreach (XmlNode XDRACB in XDRBList)
                        {

                            // Create a new basin.
                            Basin Bas = new Basin();

                            if (!XDRACB.HasChildNodes)
                            {
                                Error.Throw("Invalid basin!", "One of the basins in this Proj2 file is corrupted!", ErrorSeverity.Error, 121);
                                return XER;
                            }
                            else
                            {
                                XmlNodeList XDRAList = XDRACB.ChildNodes;
                                // Iterate through the child nodes of the basin.
                                foreach (XmlNode XDRAC in XDRAList)
                                {
                                    switch (XDRAC.Name)
                                    {
                                        case "Name": // The name of this basin. Triggers a GlobalState load.
                                            Bas = Proj.GetBasinWithName(XDRAC.InnerText);
                                            continue;
                                        case "UserTag": // The user-given name of this basin
                                            Bas.UserTag = XDRAC.InnerText;
                                            continue;
                                        case "IsOpen": // Not sure if I'll use this
                                            Bas.IsOpen = Convert.ToBoolean(XDRAC.InnerText );
                                            continue;
                                        case "IsSelected": // Not sure if I'll use this
                                            Bas.IsSelected = Convert.ToBoolean(XDRAC.InnerText);
                                            continue;
                                        case "Layers":

                                            // Detect if somehow an invalid layer was created
                                            if (!XDRAC.HasChildNodes)
                                            {
                                                Error.Throw("Invalid basin!", "There are no layers!", ErrorSeverity.Error, 122);
                                                return XER;
                                            }
                                            else
                                            {
                                                // Iterate through the layers
                                                XmlNodeList XDRACList = XDRAC.ChildNodes;

                                                foreach (XmlNode XDRACL in XDRACList)
                                                {
                                                    switch (XDRACL.Name)
                                                    {
                                                        case "Layer":

                                                            Layer Lyr = new Layer();
                                                            if (!XDRACL.HasChildNodes)
                                                            {
                                                                Error.Throw("Invalid basin!", "Empty layer detected!", ErrorSeverity.Error, 123);
                                                                return XER;
                                                            }
                                                            else
                                                            {
                                                                XmlNodeList XDRACLList = XDRACL.ChildNodes;

                                                                // Yeah
                                                                foreach (XmlNode XDRACLL in XDRACLList)
                                                                {
                                                                    

                                                                    switch (XDRACLL.Name)
                                                                    {
                                                                        case "GUID": // GUID of this basin
                                                                            Lyr.LayerId = Guid.Parse(XDRACLL.ChildNodes[0].InnerText);
                                                                            continue;
                                                                        case "Name": // Name of this basin
                                                                            Lyr.Name = XDRACLL.InnerText;
                                                                            continue;
                                                                        case "Storms": // Storms of this basin

                                                                            if (!XDRACLL.HasChildNodes)
                                                                            {
                                                                                continue; // empty layer
                                                                            }
                                                                            else
                                                                            {
                                                                                // Find each storm node

                                                                                XmlNodeList XDRACLLSList = XDRACLL.ChildNodes;

                                                                                foreach (XmlNode XDRACLLS in XDRACLLSList)
                                                                                {
                                                                                    Storm Sto = new Storm();
                                                                                    switch (XDRACLLS.Name)
                                                                                    {
                                                                                        case "Storm":
                                                                                            
                                                                                            if (!XDRACLLS.HasChildNodes)
                                                                                            {
                                                                                                Error.Throw("Invalid basin!", "Empty layer detected!", ErrorSeverity.Error, 186);
                                                                                                return XER;
                                                                                            }
                                                                                            else
                                                                                            {

                                                                                                XmlNodeList XDRACLLSSList = XDRACLLS.ChildNodes;

                                                                                                foreach (XmlNode XDRACLLSS in XDRACLLSSList)
                                                                                                {
                                                                                                    switch (XDRACLLSS.Name)
                                                                                                    {
                                                                                                        case "FormationDate": // The formation date of this system.
                                                                                                            Sto.FormationDate = DateTime.Parse(XDRACLLSS.ChildNodes[0].InnerText);
                                                                                                            continue;
                                                                                                        case "ID": // The ID of this system.
                                                                                                            Sto.Id = Convert.ToInt32(XDRACLLSS.ChildNodes[0].InnerText);
                                                                                                            continue;
                                                                                                        case "Name": // Name of this system.
                                                                                                            Sto.Name = XDRACLLSS.ChildNodes[0].InnerText;
                                                                                                            continue;
                                                                                                        case "Nodes":
                                                                                                            // this code is bad and will be entirely scrapped in version 2.1, fixing it in v551
                                                                                                            if (!XDRACLLSS.HasChildNodes)
                                                                                                            {
                                                                                                                continue;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                NodeImportResult NIR = ImportNodes(XDRACLLSS);

                                                                                                                if (NIR.Successful && !NIR.Empty)
                                                                                                                {
                                                                                                                    Sto.NodeList = NIR.Nodes;
                                                                                                                }

                                                                                                            }

                                                                                                            continue;
                                                                                                        case "DeletedNodes":
                                                                                                            NodeImportResult DNIR = ImportNodes(XDRACLLSS);

                                                                                                            if (DNIR.Successful && !DNIR.Empty)
                                                                                                            {
                                                                                                                Sto.NodeList_Deleted = DNIR.Nodes;
                                                                                                            }

                                                                                                            continue;

                                                                                                    }


                                                                                                }


                                                                                            
                                                                                            }
                                                                                            // Get the storm nodes
                                                                                            Lyr.AddStorm(Sto);
                                                                                            continue;
                                                                                    }



                                                                                }

                                                                            }

                                                                            continue;
                                                                    }

                                                                }


                                                            }


                                                            if (Lyr.Name == null)
                                                            {
                                                                Error.Throw("Invalid basin!", "Layer with no name!", ErrorSeverity.Error, 125);
                                                                return XER;
                                                            }
                                                            else
                                                            {
                                                                Layers.Add(Lyr); 
                                                            }

                                                            continue;


                                                    }
                                                    continue; 
                                                }

                                            }
                                                continue; 
                                    }


                                    continue;
                                }


                            }

                            List<string> IXmlParse = XDRA.InnerXml.InnerXml_Parse();
                                
                            // this is a complete hack,
                            // in version 2.1 this code will be thrown out and replaced with xml deserialisation as this is genuinely terrible
                            string BasinName = null; 
                            
                            for (int i = 0; i < IXmlParse.Count; i++)
                            {
                                string IXmlParseString = IXmlParse[i];

                                IXmlParseString = IXmlParseString.Xaml2Cs();

                                // this should in all reasonable circumstances hit the storm first.
                                if (IXmlParseString == "Name")
                                {
                                    // if we are one element before the end of the list, name is empty
                                    if (i - IXmlParse.Count == 1)
                                    {
                                        Error.Throw("Fatal Error!", "Invalid Proj file - no name specified for basin.", ErrorSeverity.Error, 180);
                                        return XER;
                                    }
                                    else
                                    {
                                        // also convert the next element

                                        string PlusOne = IXmlParse[i + 1].Xaml2Cs();
                                        BasinName = PlusOne;
                                        break; // avoid multiple hits
                                    }
                                }
                            }

                            if (BasinName != null)
                            {
                                Basin NewBasin = Proj.GetBasinWithName(BasinName);

                                // really dumb hack
                                foreach (Layer Lyrs in Layers)
                                {
                                    NewBasin.AddLayer(Lyrs);
                                }

                                Proj.AddBasin(NewBasin, true);
                                Proj.SelectedBasin = NewBasin;
                            }




                        }



                        continue; 
                }
            }

            XER.Successful = true;
            XER.Project = Proj; 
            return XER;
        }
#endif
        private protected NodeImportResult ImportNodes(XmlNode XNN)
        {

            NodeImportResult NIR = new NodeImportResult();

            // get globalstate structures in Dano, use MainWindow for Priscilla

#if DANO
            StormTypeManager ST2Manager = GlobalState.GetST2Manager();
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            StormTypeManager ST2Manager = MnWindow.ST2Manager;
#endif
            if (!XNN.HasChildNodes)
            {
                NIR.Successful = true;
                NIR.Empty = true;
                return NIR; 
            }
            else
            {
                foreach (XmlNode XNNChild in XNN.ChildNodes)
                {
                    
                    Node NewNode = new Node(); 

                    if (!XNNChild.HasChildNodes)
                    {
                        Error.Throw("Error!", "Empty node detected!", ErrorSeverity.Error, 222);
                    }
                    else
                    {
                        foreach (XmlNode NodeInformationNode in XNNChild.ChildNodes)
                        {
                            switch (NodeInformationNode.Name)
                            {
                                case "Intensity":
                                    NewNode.Intensity = Convert.ToInt32(NodeInformationNode.InnerText);
                                    continue;
                                case "Position":
                                    NewNode.Position = NodeInformationNode.InnerText.SplitXY();
                                    continue;
                                case "Type":
                                    NewNode.NodeType = ST2Manager.GetStormTypeWithName(NodeInformationNode.InnerText);
                                    continue;
                            }
                        }
                    }



                    NIR.Nodes.Add(NewNode); 
                }

                NIR.Successful = true;
                NIR.Empty = false;
                return NIR; 
            }



        }
        
        /// <summary>
        /// This will be removed - export formats will not generate previews in v2, the track maker will
        /// </summary>
        /// <param name="Canvas"></param>
        public void GeneratePreview(Canvas Canvas)
        {
            throw new NotImplementedException();
        }
    }
}
