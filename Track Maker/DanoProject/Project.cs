using Starfrost.UL5.Logging;
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
        // REMOVED in priscilla v542 as we now use a static global data structure for this
        //public List<Basin> Basins { get; set; } /* All selectable basins in this Project. */
        public string FileName { get; set; } /* The file name of this project. */ 
        public List<Basin> OpenBasins { get; set; } /* All opened basins */ 
        public List<CategorySystem> CategorySystems { get; set; } /* All selectable category systems in this Project. */
        public CategorySystem SelectedCategorySystem { get; set; } /* Currently selected category system. */
        public List<Basin> History { get; set; } /* For undo/redo and the like. */ 
        public string Name { get; set; } /* Project name */
        public string Path { get; set; } /* Project path */
        public Basin SelectedBasin { get; set; } /* Currently selected basin. */
        public int CurrentHistoryPoint { get; set; } /* Current history point */

        /// <summary>
        /// Project constructor.
        /// </summary>
        /// <param name="LoadBasins">Reload basins. Deprecated but whatever</param>
        public Project()
        {
            History = new List<Basin>();
            CategorySystems = new List<CategorySystem>();
            OpenBasins = new List<Basin>();
            SelectedBasin = new Basin();
        }

        /// <summary>
        /// Add the basin with Name. Also select it if SelectNow = true. 
        /// </summary>
        /// <param name="Name">The name of the basin we wish to load.</param>
        /// <param name="SelectNow">Select the basin upon loading.</param>
        public void AddBasin(string Name, bool SelectNow = false)
        {
            // This is still terrible, but it's just temporary
            Basin Bs = GetBasinWithName(Name);

            if (Bs == null) Error.Throw("Fatal Error", $"Attempted to add an invalid basin with name {Name}", ErrorSeverity.FatalError, 181);


            Bs.LoadImage(Bs.ImagePath);

            InitBasin(Bs);

        }

        /// <summary>
        /// Add the basin with Name and usertag UserTag. Also select it if SelectNOw = true. 
        /// </summary>
        /// <param name="Name">The name of the basin to add.</param>
        /// <param name="UserTag">The name of the season to add (V3.0 Only)</param>
        /// <param name="SelectNow">Select this basin now</param>
        public void AddBasin(string Name, string UserTag, bool SelectNow = false)
        {
            // This is still terrible, but it's just temporary
            Basin Bs = GetBasinWithName(Name);
            Bs.LoadImage(Bs.ImagePath);
            Bs.UserTag = UserTag;
            InitBasin(Bs);

        }

        public void AddBasin(Basin Bs, bool SelectNow = false)
        {
            InitBasin(Bs); 
        }

        private void InitBasin(Basin Bs, bool SelectNow = false)
        {
#if DANO
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
            Bs.SelectLayerWithName(BgLayer.Name);
#if PRISCILLA
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            MnWindow.Layers.AddLayer(BgLayer.Name); 
#endif
            OpenBasins.Add(Bs);
            SelectedBasin = Bs;
        }

        /// <summary>
        /// This loads the basins.
        /// 
        /// This should be a global thing, but in the interests of finishing Priscilla it is simply easier if we do it this way/
        /// </summary>
        
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
            foreach (Basin Basin in GlobalStateP.OpenBasins)
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
        /// (2.0+) Get basin with name. Major refactoring is currently ongoing that will eventually lead to this being moved to its own class (GlobalState?)
        /// </summary>
        /// <returns></returns>
        public Basin GetBasinWithName(string Name)
        {
            foreach (Basin Basin in GlobalStateP.OpenBasins)
            {
                if (Basin.Name == Name) return Basin;
            }

            return null;
        }


        /// <summary>
        /// Creates a list of strings from the globally loaded basin list.
        /// </summary>
        public List<string> GetBasinNames()
        {
            // Create a new string list.
            List<string> _ = new List<string>();

            // Iterate through all of the storms
            foreach (Basin CurBasin in GlobalStateP.OpenBasins)
            {
                _.Add(CurBasin.Name);
            }

            return _;
        }

        public Basin GetBasinWithAbbreviation(string Abbreviation)
        {
            foreach (Basin Basin in GlobalStateP.OpenBasins)
            {
                if (Basin.Abbreviation == Abbreviation)
                {
                    return Basin;
                }
            }

            return null; 
        }

    }
}
