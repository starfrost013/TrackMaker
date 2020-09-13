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
    /// TProj-v2 format
    /// 
    /// Priscilla v442
    /// 
    /// 2020-09-12
    /// 
    /// Version 1.0     Priscilla v443      Began functionality
    /// </summary>
    public class XMLv2 : IExportFormat
    {
        public bool AutoStart { get; set; }
        public string Name { get; set; }
        public static int FormatVersionMajor = 2;
        public static int FormatVersionMinor = 0;
        public XMLv2()
        {
            AutoStart = false;
            Name = "XML";
        }

        /// <summary>
        /// Get the name of the XMLv2 class. 
        /// </summary>
        /// <returns></returns>
        public string GetName() => Name;

        public bool Export(Basin Basin)
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

                ExportCore(Basin, SFD.FileName);

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

        public bool ExportCore(Basin Basin, string FileName)
        {
            XmlDocument XDoc = new XmlDocument();
            XmlNode XRoot = XDoc.CreateElement("Project");

            // Version 2.0: Write the metadata

            // Version of the format. 
            XmlNode XFormatVersionMajor = XDoc.CreateElement("FormatVersionMajor");
            XmlNode XFormatVersionMinor = XDoc.CreateElement("FormatVersionMinor");

            XmlNode XTimestamp = XDoc.CreateElement("Timestamp");

            XFormatVersionMajor.InnerText = FormatVersionMajor.ToString();
            XFormatVersionMinor.InnerText = FormatVersionMinor.ToString();

            // ISO 8601 date format
            XTimestamp.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // dump the storm info to file
            foreach (Storm XStorm in Basin.Storms)
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

        /// <summary>
        /// Import this
        /// </summary>
        /// <returns>The imported basin.</returns>
        public Basin Import()
        {
            // Implement later
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// This will be removed - export formats will not generate previews in v2
        /// </summary>
        /// <param name="Canvas"></param>
        public void GeneratePreview(Canvas Canvas)
        {
            throw new NotImplementedException();
        }
    }
}
