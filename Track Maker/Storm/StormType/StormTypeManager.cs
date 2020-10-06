using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
                                            case "Name":
                                                ST2.Name = HStormInfo.InnerText;
                                                continue;
                                            case "Shape":

                                                if (!HStormInfo.HasChildNodes)
                                                {
                                                    Error.Throw("Fatal Error!", "StormTypes.xml is malformed - your Track Maker installation is corrupted and you must reinstall.", ErrorSeverity.FatalError, 163);
                                                    return null;
                                                }
                                                continue; 
                                        }
                                    }
                                }

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
    }
}
