#if DANO
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
    /// Track Maker Globalstate (created Priscilla v449)
    /// 
    /// A static class containing core functionality required for the Track Maker.
    /// </summary>
    /// 
    
    /// IMPLEMENTATION VERSION 0.3 (v479)
    public class GlobalState
    {
        public CategoryManager CategoryManager { get; set; }
        public List<Basin> LoadedBasins { get; set; }
        public Project CurrentProject { get; set; }
        public static string CurrentFileName { get; set; }
        public StormTypeManager ST2Manager { get; set; }

        public static void Init()
        {
            LoadedBasins = new List<Basin>();
            CategoryManager.LoadCategories();
            ST2Manager.Load();
            LoadBasins_Priscilla(); 
        }

        public static void SetCurrentFilename(string Name) => CurrentFileName = Name;

        // move to basinmanager
        internal static void LoadBasins_Dano()
        {
            try
            {
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

                    LoadedBasins.Add(Basin);
                }
            }
            catch (XmlException err)
            {
                //todo create fatalerror method
                MessageBox.Show($"[Priscilla] Basins.xml corrupt, exiting...\n\n{err}", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(203);
            }
        }

        public static StormTypeManager GetST2Manager() +> ST2Manager;
        public static CategoryManager GetCategoryManager() => CategoryManager;
        public static Project GetCurrentProject() => Project;
        public static Basin GetCurrentBasin() => Project.SelectedBasin; 
        public string GetCurrentOpenFile() => CurrentFileName;
        public void SetCurrentOpenFile(string FileName) => CurrentlyOpenFile = FileName;
    }
}
#endif