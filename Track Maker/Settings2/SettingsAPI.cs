using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Reflection; 
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using System.Windows.Media; 

namespace Track_Maker
{
    /// <summary>
    /// Emerald Settings
    /// 
    /// Ported from Emerald Lite/NetEmerald/Emerald Mini game engine
    /// </summary>
    /// 

    public static class EmeraldSettings
    {
        internal static XmlNode LoadSettingsXmlGetNode()
        {
            try
            {
                // Priscilla 442 - simplify
                XmlDocument XDoc = LoadSettingsXml(); 

                XmlNode XRoot = GetFirstNode(XDoc); 

                return XRoot;
            }
            // can't load serversettings.xml because it doesn't exist
            catch (FileNotFoundException err)
            {
                MessageBox.Show($"Uh oh, something bad happened!! GenerateGameSettings() failed. Error 10!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            // some error in parsing the xml
            catch (IOException err)
            {
                //NetEmeraldCore.ThrowError("IOException while loading xml!", 6, err);
                MessageBox.Show($"Temp error. IOException occurred while loading settings xml. Error 9!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null; // handle nicely.
            }
            catch (XmlException err)
            {
                MessageBox.Show($"Temp error. ServerSettings.xml corrupted or maflormed! Error 18!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static XmlDocument LoadSettingsXml()
        {
            try
            {
                XmlDocument XDoc = new XmlDocument();

                string SettingFile = @"Data\Settings.xml";

                if (!File.Exists(SettingFile))
                {
                    Error.Throw("Fatal error", "Settings.xml not found!", ErrorSeverity.FatalError, 110);
                }

                XDoc.Load(SettingFile);

                return XDoc;
            }
            // can't load serversettings.xml because it doesn't exist
            catch (FileNotFoundException err)
            {
                MessageBox.Show($"Uh oh, something bad happened!! GenerateGameSettings() failed. Error 10!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            // some error in parsing the xml
            catch (IOException err)
            {
                //NetEmeraldCore.ThrowError("IOException while loading xml!", 6, err);
                MessageBox.Show($"Temp error. IOException occurred while loading settings xml. Error 9!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null; // handle nicely.
            }
            catch (XmlException err)
            {
                MessageBox.Show($"Temp error. ServerSettings.xml corrupted or maflormed! Error 18!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// This is used for write access to Settings.xml.  
        /// </summary>
        /// <returns></returns>
        internal static XmlNode GetFirstNode(XmlDocument XDoc, string VerifyString = "Settings")
        {
            XmlNode XRoot = XDoc.FirstChild;

            // Feature: if we pass null as our VerifyString, we don't verify and just return the first node
            if (VerifyString == null) return XRoot; 

            while (XRoot.Name != VerifyString)
            {
                //get the next sibling until it has the name we want
                XRoot = XRoot.NextSibling;
            }

            return XRoot;
        }

        /// <summary>
        /// Internal Api for getting nodes from the root node obtained by using LoadSettingsXml()/
        /// </summary>
        /// <param name="XRoot">The root node of the ServerSettings Xml.</param>
        /// <param name="NodeName">The name of the setting to acquire.</param>
        /// <returns></returns>
        private static XmlNode GetNode(XmlNode XRoot, string NodeName)
        {
            //iterate through the metadata 
            XmlNodeList XMetadata = XRoot.ChildNodes;

            foreach (XmlNode XMetadataElement in XMetadata)
            {
                // get the name. Switch statements don't support non-constant values
                if (XMetadataElement.Name == NodeName)
                {
                    return XMetadataElement;
                }

            }
            return null; // node not found or incorrect node
        }

        /// <summary>
        /// Obtains a double setting.
        /// </summary>
        /// <param name="SettingsElement">The name of the setting to acquire.</param>
        /// <returns></returns>
        public static bool GetBool(string SettingsElement)
        {
            try
            {
                XmlNode XRoot = LoadSettingsXmlGetNode();
                XmlNode XElement = GetNode(XRoot, SettingsElement);

                //throw an error if xelement is null
                if (XElement == null || XRoot == null)
                {
                    Error.Throw("An error has occurred.", "Attempted to load invalid setting boolean. A default value will be used.", ErrorSeverity.Warning, 12);
                    return false; // 2.0.2 
                }


                bool Val = Convert.ToBoolean(XElement.InnerText);

                XRoot = null;
                XElement = null;

                return Val;
            }
            catch (FormatException err)
            {
                Error.Throw("An error has occurred.", $"Error converting string to boolean while loading xml! A default message will be used.\n\n{err}", ErrorSeverity.Warning, 11);
                return false;
            }
        }

        /// <summary>
        /// Obtains a double setting.
        /// </summary>
        /// <param name="SettingsElement">The name of the setting to acquire.</param>
        /// <returns></returns>
        public static double GetDouble(string SettingsElement)
        {
            try
            {
                XmlNode XRoot = LoadSettingsXmlGetNode();
                XmlNode XElement = GetNode(XRoot, SettingsElement);

                //throw an error if xelement is null
                if (XElement == null || XRoot == null) 
                {
                    Error.Throw("An error has occurred.", "Attempted to load invalid setting boolean. A default value will be used.", ErrorSeverity.Warning, 13);
                    return 0; 
                }

                double Val = Convert.ToDouble(XElement.InnerText);

                return Val;
            }
            catch (FormatException err)
            {
                Error.Throw("An error has occurred.", $"Error converting string to double while loading Settings XML! A default value will be used.\n\n{err}", ErrorSeverity.Error, 14);
                return 0; 
            }
        }

        /// <summary>
        /// Obtains an int setting.
        /// </summary>
        /// <param name="SettingsElement">The name of the setting to acquire.</param>
        /// <returns></returns>
        public static int GetInt(string SettingsElement)
        {
            try
            {
                XmlNode XRoot = LoadSettingsXmlGetNode();
                XmlNode XElement = GetNode(XRoot, SettingsElement);

                // throw an error if xelement is null
                if (XElement == null || XRoot == null)
                {
                    Error.Throw("An error has occurred.", "Attempted to load invalid Int32 setting. A default value will be used.", ErrorSeverity.Warning, 15);
                }

                int Val = Convert.ToInt32(XElement.InnerText);

                return Val;
            }
            catch (FormatException err)
            {
                Error.Throw("An error has occurred.", $"Error converting string to int while loading xml! A default value will be used.\n\n{err}", ErrorSeverity.Error, 16);

                return 0;
            }
        }

        /// <summary>
        /// Obtains a string setting.
        /// </summary>
        /// <param name="SettingsElement">The name of the setting to acquire.</param>
        public static string GetString(string SettingsElement)
        {
            XmlNode XRoot = LoadSettingsXmlGetNode();
            XmlNode XElement = GetNode(XRoot, SettingsElement);

            // throw an error if xelement is null
            if (XElement == null || XRoot == null)
            {
                Error.Throw("An error has occurred.", "Attempted to load invalid setting string! A default value will be used.", ErrorSeverity.Warning, 17);
                return $"Setting {SettingsElement} has missing value!"; 
            }

            string Val = XElement.InnerText;

            return Val;

        }

        /// <summary>
        /// Obtains a point setting.
        /// </summary>
        /// <param name="SettingsElement">The element name to grab.</param>
        /// <returns></returns>
        public static Point GetPoint(string SettingsElement)
        {
            XmlNode XRoot = LoadSettingsXmlGetNode();
            XmlNode XElement = GetNode(XRoot, SettingsElement);

            // throw an error if xelement is null
            if (XElement == null || XRoot == null) 
            {
                Error.Throw("An error has occurred.", "Attempted to load invalid setting point! A default value will be used.", ErrorSeverity.Warning, 18);
                return new Point(0, 0);
            }


            Point XY = XElement.InnerText.SplitXY(); 

            return XY;
        }

        public static Color GetColour(string SettingsElement)
        {
            XmlNode XRoot = LoadSettingsXmlGetNode();
            XmlNode XElement = GetNode(XRoot, SettingsElement);

            if (XElement == null || XRoot == null)
            {
                Error.Throw("An error has occurred.", "Attempted to load invalid setting colour! A default value will be used.", ErrorSeverity.Warning, 19);
                return new Color { A = 255, R = 255, G = 255, B = 255};
            }

            Color RGB = XElement.InnerText.SplitRGB();
  
            return RGB;
        }

        /// <summary>
        /// Did we consent to telemetry?
        /// </summary>
        /// <returns></returns>
        public static TelemetryConsent GetTelemetryConsent(string SettingsElement)
        {
            try
            {
                // Get the TelemetryConsent node
                XmlNode XRoot = LoadSettingsXmlGetNode();
                XmlNode XElement = GetNode(XRoot, SettingsElement);

                // If it doesn't exist crash (TEMP - add an IsOptional bool param for settings) 
                if (XElement == null || XRoot == null)
                {
                    Error.Throw("An error has occurred.", "Attempted to load invalid TelemetryConsent setting! A default value will be used - you will be prompted every time until this issue is fixed.", ErrorSeverity.Warning, 100);
                    return TelemetryConsent.No; // err on the side of caution
                }

                // Parse as TelemetryConsent 
                return (TelemetryConsent)Enum.Parse(typeof(TelemetryConsent), XElement.InnerText);
            }
            catch (ArgumentException err)
            {
                Error.Throw("An error has occurred.", $"Attempted to load invalid TelemetryConsent setting!\n\n{err}", ErrorSeverity.FatalError, 101);
                return TelemetryConsent.No; 
            }
        }

        /// <summary>
        /// Easier to have an enum for this here.
        /// </summary>
        /// <param name="SettingsElement"></param>
        /// <returns></returns>
        public static WndStyle GetWindowStyle(string SettingsElement)
        {
            try
            {
                // Get the TelemetryConsent node
                XmlNode XRoot = LoadSettingsXmlGetNode();
                XmlNode XElement = GetNode(XRoot, SettingsElement);

                // If it doesn't exist crash (TEMP - add an IsOptional bool param) 
                if (XElement == null || XRoot == null)
                {
                    Error.Throw("Fatal Error", "Attempted to load invalid WindowStyle setting! A default value will be used.", ErrorSeverity.Warning, 130);
                }

                // Parse as TelemetryConsent 
                return (WndStyle)Enum.Parse(typeof(WndStyle), XElement.InnerText);
            }
            catch (ArgumentException err)
            {
                Error.Throw("Fatal Error", $"Attempted to load invalid WindowStyle setting!\n\n{err}", ErrorSeverity.Warning, 131);
                return WndStyle.Windowed;
            }

        }

        /// <summary>
        /// Saves a setting to Settings.xml.
        /// </summary>
        /// <param name="SettingsElement">The name of the setting to change.</param>
        /// <param name="SettingsValue">The value to change it to.</param>
        /// <returns></returns>
        public static bool SetSetting(string SettingsElement, string SettingsValue)
        {
            try
            {
                if (SettingsElement == null || SettingsElement.Length == 0)
                {
                    Error.Throw("Error!", "Attempted to set an invalid (null or zero-length) setting!", ErrorSeverity.Error, 222);
                    return false; 
                }

                Logging.Log($"[Saving settings] Saving the setting {SettingsElement} to Settings.xml (value {SettingsValue}...");
                // Load settings and get the first node.
                XmlDocument XDoc = LoadSettingsXml();
                XmlNode XRoot = GetFirstNode(XDoc);

                // Find the setting we need.
                XmlNode XElement = GetNode(XRoot, SettingsElement);
                
                if (XRoot == null)
                {
                    Error.Throw("Error!", "An error occurred while saving settings. Either settings.xml is empty or we attempted to modify an invalid setting.", ErrorSeverity.Error, 220);
                    return false; 
                }
                else
                {
                    if (XElement == null)
                    {
                        XmlNode XNewElement = XDoc.CreateElement(SettingsElement);
                        XRoot.AppendChild(XNewElement);
                        XRoot.Value = SettingsValue;
                    }
                    else
                    {
                        // Update its inner text.
                        XElement.InnerText = SettingsValue;
                    }

                    // Save it.
                    XDoc.Save($@"Data\Settings.xml");
                }


                return true;
            }
            catch (XmlException err)
            {
                // An error occurred while saving - return false. 
                Error.Throw("Error!", $"An error occurred while saving settings.\n\n{err}", ErrorSeverity.Error, 221);
                return false;
            }
        }

    }
}
