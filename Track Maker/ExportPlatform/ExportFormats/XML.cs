using Microsoft.Win32; 
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
    /// XML export format (version 1.x). 
    /// 
    /// Version history:
    /// 1.0         v0.3.211.0      2020-04-29     Initial format. Simply save the ID, name, and nodes of each storm (intensity, position, and type).
    /// 1.1         v1.0.305.0      2020-05-15     Add formation date saving.
    /// 1.2         v2.0.390.0      2020-05-24     Use AddNode. Add version saving. (did we do this?)
    /// 1.3         v2.0.445.20257  2020-09-13     Remove MainWindow dependency for exporting, importing soon (Priscilla/Dano)
    /// 1.4         v2.0.472.20274  2020-09-30     Dummy out export for reduced user confusion, put warning around import. 
    /// </summary>
    public class ExportXML : IExportFormat
    {
        public bool AutoStart { get; set; }
        public bool DisplayQualityControl { get; set; }
        internal MainWindow MnWindow { get; set; }
        public string Name { get; set; }

        public ExportXML()
        {
            Name = "Project";
            MnWindow = (MainWindow)Application.Current.MainWindow;
            AutoStart = true;
        }

        public void GeneratePreview(Canvas XCanvas)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return Name;
        }

        // 2020-05-21 [v1.0.364.0] - change to modify instead of overwrite the current basin

        /// <summary>
        /// DEPRECATED 2020-09-19 [v2.0.455] 
        /// </summary>
        /// <returns></returns>
        public Project Import()
        {
            try
            {

                OpenFileDialog SFD = new OpenFileDialog();
                SFD.Title = "Import from track maker project file...";
                SFD.Filter = "Track Maker project files|*.tproj";
                SFD.ShowDialog();

                if (SFD.FileName == "") return null;

                XmlDocument XDoc = new XmlDocument();
                MnWindow.CurrentProject.FileName = SFD.FileName;
                Basin XBasin = MnWindow.CurrentProject.SelectedBasin;

                // Fix schizophrenic code in preparation for end of MainWindow dependency
                Canvas Ct = MnWindow.HurricaneBasin;

#if DANO
                StormTypeManager ST2M = GlobalState.GetST2Manager();
#else
                StormTypeManager ST2M = MnWindow.ST2Manager;
#endif
                Ct.Children.Clear();

                XBasin.ClearBasin();
                // Layer Init
                XBasin.CurrentLayer = new Layer(); 

                XDoc.Load(SFD.FileName);

                XmlNode XRoot = XDoc.FirstChild;

                while (XRoot.Name != "Project")
                {
                    if (XRoot.NextSibling == null)
                    {
                        MessageBox.Show($"An error has occurred. Invalid project root node. The project you are attempting to load may be corrupted. [Error Code: IX3]");
                        Application.Current.Shutdown(-0xF3);
                    }

                    XRoot = XRoot.NextSibling;
                }

                XmlNodeList XChildren = XRoot.ChildNodes;

                foreach (XmlNode XChild in XChildren)
                {
                    Storm Storm = new Storm();

                    if (!XChild.HasChildNodes)
                    {
                        // the user may have never used undo 
                        if (XChild.Name == "DeletedNodes") continue;

                        MessageBox.Show($"An error occurred while loading the project. Can't load an empty storm! [Error Code: IX4].");
                        Application.Current.Shutdown(-0xF4);
                        return null;
                    }

                    XmlNodeList XChild2 = XChild.ChildNodes;

                    foreach (XmlNode XChild2C in XChild2)
                    {
                        List<string> XMLValues = XChild.InnerXml.InnerXml_Parse();

                        switch (XChild2C.Name)
                        {
                            case "FormationDate":
                                Storm.FormationDate = DateTime.Parse(XChild2C.InnerText);
                                continue;
                            case "ID":
                                Storm.Id = Convert.ToInt32(XMLValues[3]);
                                continue;
                            case "Name":
                                Storm.Name = XMLValues[5];
                                continue;
                            case "DeletedNodes":
                            case "Nodes":
                                // get the node information

                                XmlNodeList XGrandchildren = XChild2C.ChildNodes;

                                foreach (XmlNode XGrandChild in XGrandchildren)
                                {
                                    if (!XGrandChild.HasChildNodes)
                                    {
                                        MessageBox.Show($"An error occurred while loading the project. Can't load an empty node list! [Error Code: IX5].");
                                        Application.Current.Shutdown(-0xF5);
                                        return null;
                                    }

                                    // loop
                                    switch (XGrandChild.Name)
                                    {
                                        // a node definition
                                        case "Node":
                                            // get the node information
                                            Node Node = new Node();
                                            XmlNodeList XGreatGrandChildren = XGrandChild.ChildNodes;

                                            // Dano: universal load function
                                            Node.Id = Storm.NodeList.Count;

                                            foreach (XmlNode XGreatGrandchild in XGreatGrandChildren)
                                            {
                                                if (!XGreatGrandchild.HasChildNodes)
                                                {
                                                    MessageBox.Show($"An error occurred while loading the project. Can't load an empty node! [Error Code: IX6].");
                                                    Application.Current.Shutdown(-0xF6);
                                                    return null;
                                                }

                                                switch (XGreatGrandchild.Name)
                                                {
                                                    case "Intensity":
                                                        // the intensity of the node
                                                        Node.Intensity = Convert.ToInt32(XGreatGrandchild.InnerText);
                                                        continue;
                                                    case "Position":
                                                        // the position of the node
                                                        Node.Position = XGreatGrandchild.InnerText.SplitXY();
                                                        continue;
                                                    case "Type":
                                                        // the type of the node
                                                        Node.NodeType = ST2M.GetStormTypeWithName(XGreatGrandchild.InnerText);
                                                        continue;
                                                    default:
                                                        continue;
                                                }
                                            }

                                            if (XChild.Name == "DeletedNodes")
                                            {
                                                // wtf is this code....? dumb shit workaround...?
                                                //if (Node.Intensity == 0 && Node.NodeType == StormType.Tropical && Node.Position.X == 0 && Node.Position.Y == 0) continue;
                                                Storm.NodeList_Deleted.Add(Node);
                                                continue;
                                            }
                                            else
                                            {
                                                Storm.NodeList.Add(Node);
                                            }

                                            continue;
                                    }

                                }
                                continue;
                            default:
                                continue;

                        }
                    }

                    // temp
                    XBasin.CurrentLayer.AssociatedStorms.Add(Storm);
                }

                //ADDED MORE
                Ct.UpdateLayout();

                Project Proj = new Project();
                // THIS IS A VERY DUMB AND SHIT HACK
                // BASIN LOADING SYSTEM IS FUCKED RN WILL FIX AROUND BETA (V518)
                Proj.Basins = MnWindow.CurrentProject.Basins;
                // THIS IS THE END OF THE VERY DUMB AND SHIT HACK
                Proj.FileName = SFD.FileName; 
                Proj.AddBasin(XBasin, true);

                return Proj;
            }
            catch (FormatException err)
            {
                MessageBox.Show($"An error occurred while loading the project - an error occurred converting between two types of data. [Error Code: IX3].\n\n{err}");
                Application.Current.Shutdown(-0xF3);
                return null;
            }
            catch (IOException err)
            {
                MessageBox.Show($"An error occurred while loading the project. [Error Code: IX1].\n\n{err}");
                Application.Current.Shutdown(-0xF1);
                return null;
            }
            catch (XmlException err)
            {
                MessageBox.Show($"An error occurred while loading an XML file. [Error Code: IX2].\n\n{err}");
                Application.Current.Shutdown(-0xF2);
                return null;
            }
        }

        /// <summary>
        /// removed - priscilla version 472
        /// </summary>
        /// <param name="Project"></param>
        /// <returns></returns>
        public bool Export(Project Project)
        {
            throw new NotImplementedException("Track Maker 1.x export support is removed in Priscilla - only import allowed! This should not be called!");
        }

        public bool ExportCore(Project Project, string FileName)
        {
            throw new NotImplementedException("WHO CALLED THIS METHOD? IMA BEAT YO ASS LIKE BIDEN BEAT TRUMP");
        }

        /*
        public bool ExportCore(Project Project, string FileName)
        {
            // V1 Export Code
            // Unused since build 472, removed in 514a

            
            XmlDocument XDoc = new XmlDocument();
            XmlNode XRoot = XDoc.CreateElement("Project");

            // dump the storm info to file
            foreach (Storm XStorm in Project.SelectedBasin.GetFlatListOfStorms())
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
                    XNodeType.InnerText = XNode.NodeType.ToString();

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
                    XNodeType.InnerText = XNode.NodeType.ToString();

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

                XRoot.AppendChild(XStormNode);
            }

            XDoc.AppendChild(XRoot);

            XDoc.Save(FileName);

            return true;
            
        }*/

    }
}

