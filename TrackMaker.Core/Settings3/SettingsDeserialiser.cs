
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; 
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Windows;
using TrackMaker.Core.StaticSerialiser;

namespace TrackMaker.Core
{

    /// <summary>
    /// Iris
    /// 
    /// ApplicationSettings API 3.0
    /// 
    /// ApplicationSettings XML Serialiser
    /// 
    /// 2021-01-21 00:31
    /// </summary>
    public class SettingsSerialiser
    {
        public void LoadApplicationSettings()
        {
            Logging.Log("Loading settings...");

            Logging.Log("Validating settings using XML schema"); 
            SettingsSerialiser_Validate();
            // throws an error if invalid.
            Logging.Log("Settings.xml valid");
            Logging.Log("Serialising settings");
            SettingsSerialiser_Serialise();
            Logging.Log("Serialisation successful!");
            SettingsSerialiser_SetDefaults(); // set the defaults if some settings failed to load
        }
        
        private void SettingsSerialiser_Validate()
        {

            string ApplicationSettingsFilePath = @"Data/Settings.xml";
            string SchemaFilePath = @"Data/Core/Schema/Settings.xsd";

            XmlReaderSettings XRS = new XmlReaderSettings();
            XRS.ValidationType = ValidationType.Schema;
            XRS.ValidationEventHandler += SettingsSerialiser_ValidateFailed;
            XRS.Schemas.Add(null, SchemaFilePath);

            XmlReader SchemaReader = XmlReader.Create(ApplicationSettingsFilePath, XRS);

            XmlDocument XD = new XmlDocument();

            try
            {
                XD.Load(SchemaReader);
            }
            catch (XmlException err)
            {
#if DEBUG
                Error.Throw("Fatal Error!", $"Error validating settings XML using schema: XML invalid.\n\n{err}", ErrorSeverity.FatalError, 412);
#else
                Error.Throw("Fatal Error!", "Error validating settings XML using schema: XML invalid.", ErrorSeverity.FatalError, 412);
#endif
            }

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
                    Logging.Log($"Fatal ApplicationSettings Validation Error:\n\n {Args.Message}");
                    Error.Throw("Fatal Error!", "Failed to validate the settings against the ApplicationSettings XML schema. ApplicationSettings.xml is invalid or corrupt. Full information is located in the most recent log file.", ErrorSeverity.Error, 406); 
                    return; 
            }
        }

        private StaticSerialisationResult SettingsSerialiser_Serialise()
        {
            string Temp_ApplicationSettingsFileName = @"Data/Settings.xml";

            // rename this class...
            StaticSerialisationResult SR = StaticSerialiser.StaticSerialiser.Deserialize(typeof(ApplicationSettings), Temp_ApplicationSettingsFileName);
            
            if (!SR.Successful)
            {
                Error.Throw("Fatal Error!", $"Error loading settings; failed to serialise the settings class\n\n{SR.Message}", ErrorSeverity.FatalError, 407);
                return SR;
            }
            else
            {
                return SR;
            }
            
        }

        /// <summary>
        /// Set default settings
        /// 
        /// TODO: SETTINGSDEFAULTS CLASS
        /// </summary>
        private void SettingsSerialiser_SetDefaults()
        {
         
            // need settingsdefault class
            if (ApplicationSettings.DotSize.X <= 0 && ApplicationSettings.DotSize.Y <= 0) ApplicationSettings.DotSize = new Point(8, 8);
            if (ApplicationSettings.LineSize == 0) ApplicationSettings.LineSize = 2;
            if (ApplicationSettings.DefaultCategorySystem == null) ApplicationSettings.DefaultCategorySystem = "SSHWS";
            if (ApplicationSettings.PeriodLengthHours == 0) ApplicationSettings.PeriodLengthHours = 6;
            if (ApplicationSettings.UndoDepth == 0) ApplicationSettings.UndoDepth = 0;
            // don't set accentcolour the user may want transparency


        }
    }
}
