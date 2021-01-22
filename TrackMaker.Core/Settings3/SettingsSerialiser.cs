
using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Threading.Tasks;
using TrackMaker.Core.StaticSerialiser;

namespace TrackMaker.Core
{

    /// <summary>
    /// Iris
    /// 
    /// Settings API 3.0
    /// 
    /// Settings XML Serialiser
    /// 
    /// 2021-01-21 00:31
    /// </summary>
    public class SettingsSerialiser
    {
        public void LoadSettings3()
        {
            Logging.Log("Loading settings...");

            Logging.Log("Validating settings using XML schema"); 
            SettingsSerialiser_Validate();
            // throws an error if invalid.
            Logging.Log("Settings.xml valid");
            Logging.Log("Serialising settings");
            SettingsSerialiser_Serialise();
            Logging.Log("Serialisation successful!"); 
        }
        
        private void SettingsSerialiser_Validate()
        {

            string SettingsFilePath = @"Data/Settings.xml";
            string SchemaFilePath = @"Data/Core/Settings.xsd";

            XmlReaderSettings XRS = new XmlReaderSettings();
            XRS.ValidationType = ValidationType.Schema;
            XRS.ValidationEventHandler += SettingsSerialiser_ValidateFailed;
            XRS.Schemas.Add(null, SchemaFilePath);

            XmlReader SchemaReader = XmlReader.Create(SettingsFilePath, XRS);

            XmlDocument XD = new XmlDocument();

            XD.Load(SchemaReader);
            
        }

        /// <summary>
        /// Handles schema validation failure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Args"></param>
        private void SettingsSerialiser_ValidateFailed(object sender, ValidationEventArgs Args)
        {
            switch (Args.Severity)
            {
                case XmlSeverityType.Warning:
                    Logging.Log($"Schema-based validation warning: The settings may not be consistent with the schema: {Args.Message}");
                    return; 
                case XmlSeverityType.Error:
                    Logging.Log($"Fatal Settings Validation Error:\n\n {Args.Message}");
                    Error.Throw("Fatal Error!", "Failed to validate the settings against the Settings XML schema. Settings.xml is invalid or corrupt. Full information is located in the most recent log file.", ErrorSeverity.Error, 406); 
                    return; 
            }
        }

        private StaticSerialisationResult SettingsSerialiser_Serialise()
        {
            string Temp_SettingsFileName = @"Data/Settings.xml";

            // rename this class...
            StaticSerialisationResult SR = StaticSerialiser.StaticSerialiser.Deserialize(typeof(ApplicationSettings), Temp_SettingsFileName);
            
            if (!SR.Successful)
            {
                Error.Throw("Fatal Error!", "Error loading settings; failed to serialise the settings class.", ErrorSeverity.FatalError, 407);
                return SR;
            }
            else
            {
                return SR;
            }
            
        }
    }
}
