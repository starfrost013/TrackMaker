using TrackMaker.Util.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace TrackMaker.Core
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
    public class GlobalState // move to Starfrost UL5 Version 5.3?. This may also be made a non-static class
    {
        /// <summary>
        /// Category Manager (not static for now)
        /// </summary>
        public CategoryManager CategoryManager { get; set; }
        /// <summary>
        /// Name of the current export format
        /// </summary>
        public static string CurrentExportFormatName { get; set; }
        public static string CurrentlyOpenFile { get; set; }
        public static List<Basin> OpenBasins { get; set; }
        public static void SetCurrentOpenFile(string FileName) => CurrentlyOpenFile = FileName;
        public static string GetCurrentOpenFile() => CurrentlyOpenFile;

        /// <summary>
        /// Storm Type Manager (Iris)
        /// </summary>
        public StormTypeManager ST2Manager { get; set; }
        /// <summary>
        /// STATIC CLASS ONLY 
        /// </summary>
        private static void InitBasinList() => OpenBasins = new List<Basin>(); 

        /// <summary>
        /// 2020-11-27
        /// 
        /// Iris: replace with deserialisation
        /// </summary>
        public static void LoadBasins()
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
                        Error.Throw("Fatal Error!", "Basins.xml is corrupted or malformed. The Track Maker will now exit.", ErrorSeverity.FatalError, 1);
                    }

                    XmlRootNode = XmlRootNode.NextSibling; //figure out what happens if the basin node doesn't exist.
                }

                XmlNodeList XmlNodes = XmlRootNode.ChildNodes; //abduct the kids of the basins node

                foreach (XmlNode XmlNode in XmlNodes)
                {
                    Basin Basin = new Basin(); // create a new basin. 

                    if (XmlNode.Name.Contains('#')) continue;

                    if (XmlNode.Name != "Basin")
                    { // change this?
                        
                        Error.Throw("Fatal Error!", "Attempted to load non-basin node, discarding basin!", ErrorSeverity.Error, 2);
                        return;
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
                                Basin.CoordsLower = Coordinate.FromString(XmlAttribute.InnerText, CoordinateFormat.TrackMaker);
                                continue;
                            case "coordsbottomright":
                            case "Coordsbottomright":
                            case "CoordsBottomright":
                            case "coordsBottomRight":
                            case "CoordsBottomRight":
                                // Conversion
                                Basin.CoordsHigher = Coordinate.FromString(XmlAttribute.InnerText, CoordinateFormat.TrackMaker);
                                continue;
                            case "name": // basin name
                            case "Name":
                                Basin.Name = XmlAttribute.Value;
                                continue;

                        }
                    }

                    if (Basin.Name == null)
                    {
                        Error.Throw("Fatal Error!", "Fatal Error: Cannot load basin with no name!", ErrorSeverity.FatalError, 240);
                    }

                    if (Basin.Abbreviation == null) Basin.Abbreviation = "NA";

                    Debug.Assert(Basin.Name != null && Basin.Abbreviation != null);
                    
                    Logging.Log($"Successfully loaded basin {Basin.Name} with image {Basin.ImagePath}");

                    OpenBasins.Add(Basin);
                }
            }
            catch (XmlException err)
            {
                //todo create fatalerror method
#if DEBUG
                Error.Throw("Fatal Error!", $"Basins.xml is corrupt or invalid!\n\n{err}", ErrorSeverity.FatalError, 203);
#else
                Error.Throw("Fatal Error!", "Basins.xml is corrupt or invalid!", ErrorSeverity.FatalError, 203); 
#endif

                Environment.Exit(203);
            }
        }
    }
}
