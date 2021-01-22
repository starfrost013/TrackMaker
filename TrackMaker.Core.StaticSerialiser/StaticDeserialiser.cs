using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TrackMaker.Core.StaticSerialiser
{
    public partial class StaticSerialiser
    {
        public static StaticSerialisationResult Deserialize(Type staticClass, string fileName)
        {
            StaticSerialisationResult SSR = new StaticSerialisationResult();

            XmlReader xmlReader = null;

            try
            {
                xmlReader = new XmlTextReader(fileName);

                Deserialize(staticClass, xmlReader);

                SSR.Successful = true;
                return SSR;
            }
            catch (Exception ex)
            {
                SSR.Successful = false;
                SSR.Message = ex.Message;
                return SSR;
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                    xmlReader = null;
                }
            }
        }

        public static object Deserialize(string name, Type type, XmlReader xmlReader)
        {
            XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
            XmlAttributes xmlAttributes = new XmlAttributes();
            xmlAttributes.XmlRoot = new XmlRootAttribute(name);
            xmlAttributeOverrides.Add(type, xmlAttributes);
            XmlSerializer xmlSerializer = new XmlSerializer(type, xmlAttributeOverrides);

            return xmlSerializer.Deserialize(xmlReader);
        }

        public static bool Deserialize(Type staticClass, XmlReader xmlReader)
        {
            FieldInfo[] fieldArray = staticClass.GetFields(BindingFlags.Static | BindingFlags.Public);
            string currentElement = null;

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.EndElement)
                    continue;

                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    currentElement = xmlReader.Name;
                }

                foreach (FieldInfo fieldInfo in fieldArray)
                {
                    string fieldName = fieldInfo.Name;
                    Type fieldType = fieldInfo.FieldType;
                    object fieldObject = fieldInfo.GetValue(fieldType);

                    if (fieldInfo.IsNotSerialized)
                        continue;

                    if (fieldInfo.Name == currentElement)
                    {
                        if (typeof(IDictionary).IsAssignableFrom(fieldType) || typeof(IList).IsAssignableFrom(fieldType))
                        {
                            fieldObject = Deserialize(fieldName, fieldType, xmlReader);

                            fieldInfo.SetValueDirect(__makeref(fieldObject), fieldObject);
                        }
                        else if (xmlReader.NodeType == XmlNodeType.Text)
                        {
                            TypeConverter typeConverter = TypeDescriptor.GetConverter(fieldType);
                            object value = typeConverter.ConvertFromString(xmlReader.Value);

                            fieldInfo.SetValue(fieldObject, value);
                        }
                    }
                }
            }

            return true;
        }
    }
}
