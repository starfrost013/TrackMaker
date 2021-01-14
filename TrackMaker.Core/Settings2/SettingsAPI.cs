using Starfrost.UL5.Logging;
using Starfrost.UL5.StringUtilities;
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

namespace TrackMaker.Core
{
    /// <summary>
    /// Emerald Settings
    /// 
    /// Ported from Emerald Lite/NetEmerald/Emerald Mini game engine
    /// </summary>
    /// 

    public static class SettingsAPI
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
        /// Temp (Priscilla 442)
        /// </summary>
        private static void GenerateSettings()
        {
            FileStream FStreamSettings = File.Create("Settings.xml");

            FStreamSettings.Dispose(); 
            // END TEMP
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
                if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid setting boolean!", ErrorSeverity.FatalError, 12);


                bool Val = Convert.ToBoolean(XElement.InnerText);

                XRoot = null;
                XElement = null;

                return Val;
            }
            catch (FormatException err)
            {
                Error.Throw("An error has occurred.", $"Error converting string to boolean while loading xml!\n\n{err}", ErrorSeverity.FatalError, 11);
                Application.Current.Shutdown(11);
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
                if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid setting double!", ErrorSeverity.FatalError, 13);

                double Val = Convert.ToDouble(XElement.InnerText);

                return Val;
            }
            catch (FormatException err)
            {
                MessageBox.Show($"Error converting string to double while loading xml! Error 14!\n\n{err}", "An error has occurred.", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(14);
                return -1; 
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
                if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid setting int!", ErrorSeverity.FatalError, 15);

                int Val = Convert.ToInt32(XElement.InnerText);

                return Val;
            }
            catch (FormatException err)
            {
                Error.Throw("An error has occurred.", $"Error converting string to int while loading xml!\n\n{err}", ErrorSeverity.FatalError, 16);

                Application.Current.Shutdown(16); // Settings may be corrupted

                return -1; 
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
            if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid setting string!", ErrorSeverity.FatalError, 17);

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
            if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid setting point!", ErrorSeverity.FatalError, 18);


            Point XY = XElement.InnerText.SplitXY(); 

            return XY;
        }

        public static Color GetColour(string SettingsElement)
        {
            XmlNode XRoot = LoadSettingsXmlGetNode();
            XmlNode XElement = GetNode(XRoot, SettingsElement);

            if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid setting colour!", ErrorSeverity.FatalError, 19);

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
                if (XElement == null || XRoot == null) Error.Throw("An error has occurred.", "Attempted to load invalid TelemetryConsent setting!", ErrorSeverity.FatalError, 100); 

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
                if (XElement == null || XRoot == null) Error.Throw("Fatal Error", "Attempted to load invalid WindowStyle setting!", ErrorSeverity.FatalError, 130);

                // Parse as TelemetryConsent 
                return (WndStyle)Enum.Parse(typeof(WndStyle), XElement.InnerText);
            }
            catch (ArgumentException err)
            {
                Error.Throw("Fatal Error", $"Attempted to load invalid WindowStyle setting!\n\n{err}", ErrorSeverity.FatalError, 131);
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
                Logging.Log($"[Saving settings] Setting {SettingsElement} to {SettingsValue}...");
                // Load settings and get the first node.
                XmlDocument XDoc = LoadSettingsXml();
                XmlNode XRoot = GetFirstNode(XDoc);

                // Find the setting we need.
                XmlNode XElement = GetNode(XRoot, SettingsElement);
                
                if (XElement == null || XRoot == null)
                {
                    Error.Throw("Error!", "An error occurred while saving settings. Either settings.xml is empty or we attempted to modify an invalid setting.", ErrorSeverity.Error, 220);
                    return false; 
                }

                // Update its inner text.
                XElement.InnerText = SettingsValue;

                // Save it.
                XDoc.Save($@"{Directory.GetCurrentDirectory()}\Settings.xml");

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
