using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace Track_Maker
{
    public class Project
    {
        public List<Basin> Basins { get; set; } /* All selectable basins in this Project. */
        public List<Basin> OpenBasins { get; set; } /* All opened basins */ 
        public List<CategorySystem> CategorySystems { get; set; } /* All selectable category systems in this Project. */
        public CategorySystem SelectedCategorySystem { get; set; } /* Currently selected category system. */
        public List<Basin> History { get; set; } /* For undo/redo and the like. */ 
        public string Name { get; set; } /* Project name */
        public string Path { get; set; } /* Project path */
        public Basin SelectedBasin { get; set; } /* Currently selected basin. */
        public int CurrentHistoryPoint { get; set; } /* Current history point */

        public Project()
        {
            Basins = new List<Basin>();
            History = new List<Basin>();
            CategorySystems = new List<CategorySystem>();
            OpenBasins = new List<Basin>();
            SelectedBasin = new Basin();
            LoadBasins();
        }

        public void AddBasin(string Name, bool SelectNow = false)
        {
            // This is still terrible, but it's just temporary
            Basin Bs = GetBasinWithName(Name);
            Bs.LoadImage(Bs.BasinImagePath); 
#if PRISCILLA
            // Dano exclusive stuff.
            Bs.SeasonHemisphere = Hemisphere.North;
            Bs.SeasonType = BasinType.Track;

#elif DANO
            // ATTN: You can write anything you want if it's not covered by the currently defined ifdefs 
            Bs.SeasonHemisphere = Hemisphere;
            Bs.SeasonType = Type;
#endif
            // load function goes here

            // Create a background layer
            Layer BgLayer = new Layer();
            BgLayer.Name = "Background";

            if (SelectNow)
            {
                Bs.IsOpen = true;
                Bs.IsSelected = true; 
            }

            Bs.Layers.Add(BgLayer);
            OpenBasins.Add(Bs); 
            SelectedBasin = Bs; 

        }

        /// <summary>
        /// Priscilla+ - load the basin 
        /// </summary>
        internal void LoadBasins()
        {
            try
            {
                XmlDocument XmlDocument = new XmlDocument();
                XmlDocument.Load(@"Data\Basins.xml"); // maybe change?

                XmlNode XmlRootNode = XmlDocument.FirstChild;
                while (XmlRootNode.Name != "Basins")
                {
                    if (XmlRootNode.NextSibling == null)
                    {
                        MessageBox.Show("Basins.xml is corrupted or malformed. The Track Maker will now exit.", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    Basins.Add(Basin);
                }
            }
            catch (XmlException err)
            {
                //todo create fatalerror method
                MessageBox.Show($"[Priscilla[ Basins.xml corrupt, exiting...\n\n{err}", "Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(203);
            }
        }

        public void CreateNewProject(string Name, string ImagePath)
        {
            AddBasin(Name); 
        }

        public void CommitToHistory()
        {
            if (History.Count > Setting.UndoDepth)
            {
                History.RemoveAt(History.Count - 1);

                if (CurrentHistoryPoint > Setting.UndoDepth)
                {
                    CurrentHistoryPoint = Setting.UndoDepth;
                }
            }

            History.Add(SelectedBasin);
        }

        public void SelectBasin(string Name)
        {
            foreach (Basin Basin in Basins)
            {
                if (Basin.Name == Name) SelectedBasin = Basin;
                break;
            }
            return; 
        }

        //restorehistory
        public void Redo()
        {
            SelectedBasin = History[CurrentHistoryPoint];
            CurrentHistoryPoint++;
        }

        /// <summary>
        /// Undo (temp)
        /// </summary>
        public void Undo()
        {
            SelectedBasin = History[CurrentHistoryPoint];
            CurrentHistoryPoint--; 
        }


        /// <summary>
        /// (1.5+) Get basin with name. Major refactoring is currently ongoing that will eventually lead to this being moved to its own class (GlobalState?)
        /// </summary>
        /// <returns></returns>
        public Basin GetBasinWithName(string Name)
        {
            foreach (Basin Basin in Basins)
            {
                if (Basin.Name == Name) return Basin;
            }

            return null;
        }


        /// <summary>
        /// Creates a list of strings from the names of the storms in the current basin. Move to Project.GetBasinNames() in M2/Priscilla.
        /// </summary>
        public List<string> GetBasinNames()
        {
            // Create a new string list.
            List<string> _ = new List<string>();

            // Iterate through all of the storms
            foreach (Basin CurBasin in Basins)
            {
                _.Add(CurBasin.Name);
            }

            return _;
        }

    }
}
