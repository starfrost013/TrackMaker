using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using System.Windows.Shapes;

namespace Track_Maker
{
    public class StormTypeManager
    {
        public List<StormType2> Types { get; set; }

        public StormType2 GetTypeWithName(string Name)
        {
            foreach (StormType2 ST2 in Types)
            {
                if (ST2.Name == Name)
                {
                    return ST2;
                }
            }

            // awaiting general purpose validation support in 3.0
            return null;
        }

        /// <summary>
        /// Optional function for init
        /// </summary>
        public void Init()
        {
            Types = Load(); 
        }

        /// <summary>
        /// In Dano (3.0) we will redesign every single XML loader to use a schema
        /// 
        /// As it stands this is the last non-schema-based XML loader...
        /// </summary>
        /// <returns></returns>
        public List<StormType2> Load()
        {
#if DANO
            return Load_Deserialised();
#else
            return Load_Manual(); 
#endif
        }

#if DANO

        private List<StormType2> Load_Deserialised()
        {
            throw new NotImplementedException(); 
        }
#else
        private List<StormType2> Load_Manual()
        {
            try
            {
                List<StormType2> ST2List = new List<StormType2>();

                XmlDocument XD = new XmlDocument();

                // V2.1: settings
                XD.Load(@"Data\StormTypes.xml");

                XmlNode XRoot = XD.FirstChild;

                if (XRoot.Name.Contains("#"))
                {
                    if (XRoot.NextSibling == null)
                    {
                        Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 171);
                        return null;
                    }
                    else
                    {
                        XRoot = XRoot.NextSibling;
                    }
                }

                if (!XRoot.HasChildNodes)
                {
                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 162);
                    return null;
                }
                else
                {
                    XmlNodeList CNodes = XRoot.ChildNodes;

                    foreach (XmlNode HStormType in CNodes)
                    {
                        switch (HStormType.Name)
                        {
                            // The root node for a storm type. 
                            case "StormType":

                                StormType2 ST2 = new StormType2(); 
                                
                                if (!HStormType.HasChildNodes)
                                {
                                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 163);
                                    return null;
                                }
                                else
                                {
                                    XmlNodeList GCNodes = HStormType.ChildNodes;
                                    
                                    foreach (XmlNode HStormInfo in GCNodes)
                                    {
                                        switch (HStormInfo.Name)
                                        {
                                            case "Abbreviation":
                                                ST2.Abbreviation = HStormInfo.InnerText;
                                                continue;
                                            // Best-Track abbreviations
                                            case "BTAbbreviations":
                                                if (!HStormInfo.HasChildNodes)
                                                {
                                                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 166);
                                                    return null;
                                                }
                                                else
                                                {
                                                    XmlNodeList BTAbbreviationXMLNodes = HStormInfo.ChildNodes;

                                                    foreach (XmlNode BTAbbreviationXMLNode in BTAbbreviationXMLNodes)
                                                    {
                                                        switch (BTAbbreviationXMLNode.Name)
                                                        {
                                                            case "Abbreviation":
                                                                // reduces code complexity by eliminating the need for error checking in this instance
                                                                ST2.BTAbbreviations.Add(BTAbbreviationXMLNode.InnerText);
                                                                continue; 

                                                        }
                                                    }
                                                }
                                                continue;
                                            case "ForceColour":
                                                ST2.ForceColour = true;
                                                ST2.ForcedColour = HStormInfo.InnerText.SplitARGB();
                                                continue;
                                            case "Name":
                                                ST2.Name = HStormInfo.InnerText;
                                                continue;
                                            case "Shape":

                                                if (!HStormInfo.HasChildNodes)
                                                {
                                                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 163);
                                                    return null;
                                                }
                                                else
                                                {
                                                    
                                                    XmlNodeList ShapeData = HStormInfo.ChildNodes; 

                                                    foreach (XmlNode ShapeDataNode in ShapeData)
                                                    {
                                                        switch (ShapeDataNode.Name)
                                                        {
                                                            case "PresetShape":
                                                                ST2.UsePresetShape = true;
                                                                ST2.PresetShape = (StormShape)Enum.Parse(typeof(StormShape), ShapeDataNode.InnerText);
                                                                continue;
                                                            case "VertexPoints":
                                                                // todo
                                                                Shape Shp = new Shape();

                                                                if (!ShapeDataNode.HasChildNodes)
                                                                {
                                                                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 164);
                                                                    return null;
                                                                }
                                                                else
                                                                {
                                                                    XmlNodeList PolygonDataNodes = ShapeDataNode.ChildNodes;

                                                                    foreach (XmlNode PolygonDataNode in PolygonDataNodes)
                                                                    {
                                                                        Polygon Poly = new Polygon();
                                                                        switch (PolygonDataNode.Name)
                                                                        {
                                                                            case "VertexPosition":
                                                                                // god
                                                                                
                                                                                if (!PolygonDataNode.HasChildNodes)
                                                                                {
                                                                                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 165);
                                                                                    return null;
                                                                                }
                                                                                else
                                                                                {
                                                                                    XmlNodeList PolygonInformationNodes = PolygonDataNode.ChildNodes;

                                                                                    foreach (XmlNode PolygonInformation in PolygonInformationNodes)
                                                                                    {
                                                                                        // load the polygon information, convert the data into polygon points 
                                                                                        switch (PolygonInformation.Name)
                                                                                        {
                                                                                            case "Position":
                                                                                                Poly.Points.Add(PolygonInformation.InnerText.SplitXY());
                                                                                                continue; 

                                                                                        }
                                                                                    }

                                                                                    
                                                                                }

                                                                                continue;
                                                                        }

                                                                        Shp.VPoints = Poly; 
                                                                    }
                                                                }

                                                                ST2.Shape = Shp;
                                                                continue; 

                                                        }
                                                    }
                                                }

                                                continue; 
                                        }
                                    }
                                }

                                Logging.Log($"Successfully loaded storm type with name {ST2.Name}");
                                ST2List.Add(ST2);

                                continue;
                        }
                    }
                }

                return ST2List;

            }
            catch (FileNotFoundException)
            {
                Error.Throw("Fatal Error!", "Cannot load StormTypes.xml - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 160);
                return null; 
            }
            catch (XmlException)
            {
                Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall!", ErrorSeverity.FatalError, 161);
                return null; 
            }
        }
#endif
        /// <summary>
        /// Get a list of storm types.
        /// </summary>
        /// <returns></returns>
        public List<StormType2> GetListOfStormTypes() => Types;
    
        public StormType2 GetStormTypeWithName(string Name)
        {
            foreach (StormType2 ST2 in Types)
            {
                if (ST2.Name == Name) return ST2;
            }

            // pre result validation
            return null;
        }

        public StormType2 GetStormTypeWithAbbreviation(string Abbreviation)
        {
            foreach (StormType2 ST2 in Types)
            {
                if (ST2.Abbreviation == Abbreviation) return ST2;
            }

            return null;
        }

        /// <summary>
        /// Apparently version 1's old code is such a pile of shit that we need this function.
        /// 
        /// Holy fucking shit.
        /// </summary>
        /// <param name="Id">The ID of the stormtype2 that we want. By the way, this isn't assigned, it's literally the way it's ordered - what the fuck was up with version 1.0?</param>
        /// <returns></returns>
        public StormType2 GetStormTypeWithId(int Id)
        {
            if (Id < 0 || Id > Types.Count)
            {
                Error.Throw("E170", $"THE PROGRAMMER IS A DUMB SHIT BUG:  Attempted to create a StormType2 with invalid shitindex {Id}, must be between 0 and {Types.Count}!", ErrorSeverity.FatalError, 170);
                return null; // doesn't matter
            }
            else
            {
                return Types[Id]; 
            }
        }
    }
}
