using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Track_Maker
{
    partial class MainWindow
    {
        public void LoadBasins()
        {
            try
            {
                XmlDocument XmlDocument = new XmlDocument();
                XmlDocument.Load("Basins.xml"); // maybe change?

                XmlNode XmlRootNode = XmlDocument.FirstChild;
                while (XmlRootNode.Name != "Basins")
                {
                    if (XmlRootNode.NextSibling == null)
                    {
                        MessageBox.Show("Basins.xml is corrupted. The Track Maker will now exit.", "Track Maker", MessageBoxButton.OK,MessageBoxImage.Error);
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
                                Basin.BasinImagePath = XmlAttribute.Value; //set the basin image path to the value
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
                    Logging.Log($"Successfully loaded basin {Basin.Name} with image {Basin.BasinImagePath}");

                    BasinList.Add(Basin);
                }
            }
            catch (XmlException err)
            {
                //todo create fatalerror method
                MessageBox.Show($"Basins.xml corrupt, exiting...\n\n{err}", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(3);
            }
        }
    }
}
