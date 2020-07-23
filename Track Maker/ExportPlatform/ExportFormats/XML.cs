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
    /// XML export format. 
    /// 
    /// Version history:
    /// 1.0         v0.3.211.0  2020-04-29     Initial format. Simply save the ID, name, and nodes of each storm (intensity, position, and type).
    /// 1.1         v1.0.305.0  2020-05-15     Add formation date saving.
    /// 1.2         v2.0.390.0  2020-05-24     Use AddNode. Add version saving.
    /// </summary>
    public class ExportXML : IExportFormat
    {
        public bool AutoStart { get; set; }
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

        }

        public string GetName()
        {
            return Name;
        }
        // 2020-05-21 [v1.0.364.0] - change to modify instead of overwrite the current basin
        public Basin Import()
        {
            try
            {
                OpenFileDialog SFD = new OpenFileDialog();
                SFD.Title = "Import from TProj file";
                SFD.Filter = "Track Maker project files|*.tproj";
                SFD.ShowDialog();

                if (SFD.FileName == "") return null;

                XmlDocument XDoc = new XmlDocument();
                

                MainWindow Mx = (MainWindow)Application.Current.MainWindow;

                Basin XBasin = MnWindow.CurrentBasin;

                Canvas Ct = Mx.HurricaneBasin;
                Ct.Children.Clear();

                XBasin.Storms = new List<Storm>();
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

                    List<string> _2 = XChild.InnerXml.InnerXml_Parse();

                    foreach (XmlNode XChild2C in XChild2)
                    {
                        switch (XChild2C.Name)
                        {
                            case "FormationDate":
                                Storm.FormationDate = DateTime.Parse(XChild2C.InnerText);
                                continue; 
                            case "ID":
                                Storm.Id = Convert.ToInt32(_2[2]);
                                continue;
                            case "Name":
                                Storm.Name = _2[4];
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
                                                        Node.NodeType = (StormType)Enum.Parse(typeof(StormType), XGreatGrandchild.InnerText);
                                                        continue;
                                                    default:
                                                        continue;
                                                }
                                            }

                                            if (XChild.Name == "DeletedNodes")
                                            {
                                                if (Node.Intensity == 0 && Node.NodeType == StormType.Tropical && Node.Position.X == 0 && Node.Position.Y == 0) continue;
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
                    XBasin.Storms.Add(Storm);
                }

                //ADDED MORE
                Ct.UpdateLayout();
                return XBasin;
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

        public bool Export(Basin basin, List<Storm> XStormList)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();

                SFD.Title = "Export to TProj file";
                SFD.Filter = "Track Maker Project XML files|*.tproj";
                SFD.ShowDialog();

                // user hit cancel
                if (SFD.FileName == "") return true;

                //utilfunc v2
                if (File.Exists(SFD.FileName))
                {
                    File.Delete(SFD.FileName);
                    FileStream FS = File.Create(SFD.FileName);
                    FS.Close();
                }

                ExportCore(basin, XStormList, SFD.FileName);

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

        public bool ExportCore(Basin basin, List<Storm> XStormList, string FileName)
        {
            XmlDocument XDoc = new XmlDocument();
            XmlNode XRoot = XDoc.CreateElement("Project");

            // dump the storm info to file
            foreach (Storm XStorm in MnWindow.CurrentBasin.Storms)
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
        }
    }
}
