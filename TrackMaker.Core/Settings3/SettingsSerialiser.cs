using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Threading.Tasks;

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
            XmlSerializer XSI = new XmlSerializer(typeof(ApplicationSettings));
            SettingsSerialiser_Validate();

        }
        
        private void SettingsSerialiser_Validate()
        {
            XmlReader XR = new XmlReader();
            XmlReaderSettings XRS = new XmlReaderSettings();

            XmlSchema SettingsSchema = new XmlSchema();

            TextReader SchemaReader = File.Open(@"Data/Settings.xml"); 
            XRS.Schemas.Add(new XmlSchema())
        }
    }
}
