using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace Track_Maker
{
    /// <summary>
    /// Priscilla   v514 (pre-beta release 3 stage)
    /// 
    /// GlobalState.Priscilla
    /// 
    /// Version 2.0 Global State
    /// 
    /// 10/31/20
    /// </summary>
    public class GlobalStateP // move to Starfrost UL5 Version 5.3?. This may also be made a non-static class
    {
        public static List<Basin> OpenBasins { get; set; }
        public static string CurrentlyOpenFile { get; set; }

        public static void SetCurrentOpenFile(string FileName) => CurrentlyOpenFile = FileName;
        public static string GetCurrentOpenFile() => CurrentlyOpenFile;

        /// <summary>
        /// STATIC CLASS ONLY 
        /// </summary>
        private static void InitBasinList() => OpenBasins = new List<Basin>(); 

        /// <summary>
        /// 2020-11-27
        /// </summary>
        internal static void LoadBasins()
        {
            try
            {
                InitBasinList(); 
                XmlDocument XmlDocument = new XmlDocument();
                XmlDocument.Load(@"Data\Basins.xml"); // maybe change?

                XmlNode XmlRootNode = XmlDocument.FirstChild;
                while (XmlRootNode.Name != "Basins")
                {
                    if (XmlRootNode.NextSibling == null)
                    {
                        MessageBox.Show("Basins.xml is corrupted or malformed. The Track Maker will now exit.", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(1);
                    }

                    XmlRootNode = XmlRootNode.NextSibling; //figure out what happens if the basin node doesn't exist.
                }

                XmlNodeList XmlNodes = XmlRootNode.ChildNodes; //abduct the kids of the basins node

                foreach (XmlNode XmlNode in XmlNodes)
                {
                    Basin Basin = new Basin(); // create a new basin. 

                    if (XmlNode.Name != "Basin")
                    { // change this?
                        MessageBox.Show("Basins.xml corrupt, exiting...", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(2);
                    }

                    XmlAttributeCollection XmlAttributes = XmlNode.Attributes;

                    foreach (XmlAttribute XmlAttribute in XmlAttributes)
                    {
                        switch (XmlAttribute.Name) // go through all the attributes
                        {
                            case "Abbreviation":
                            case "Acronym":
                            case "BasinCode":
                                Basin.Abbreviation = XmlAttribute.Value;
                                continue;
                            case "bgimage": // basin image path
                            case "Bgimage":
                            case "bgImage":
                            case "BgImage":
                                Basin.ImagePath = XmlAttribute.Value; //set the basin image path to the value
                                continue; // yeah
                            case "coordstopleft":
                            case "Coordstopleft":
                            case "coordsTopleft":
                            case "coordsTopLeft":
                            case "CoordsTopLeft":
                                // Conversion
                                Basin.CoordsLower = Coordinate.FromString(XmlAttribute.InnerText);
                                continue;
                            case "coordsbottomright":
                            case "Coordsbottomright":
                            case "CoordsBottomright":
                            case "coordsBottomRight":
                            case "CoordsBottomRight":
                                // Conversion
                                Basin.CoordsHigher = Coordinate.FromString(XmlAttribute.InnerText);
                                continue;
                            case "name": // basin name
                            case "Name":
                                Basin.Name = XmlAttribute.Value;
                                continue;

                        }
                    }
                    //todo: additional error detection
                    Logging.Log($"Successfully loaded basin {Basin.Name} with image {Basin.ImagePath}");

                    OpenBasins.Add(Basin);
                }
            }
            catch (XmlException err)
            {
                //todo create fatalerror method
                MessageBox.Show($"[Priscilla] Basins.xml corrupt, exiting...\n\n{err}", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(203);
            }
        }
    }
}
