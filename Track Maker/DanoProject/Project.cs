using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    public class Project
    {
        public List<Basin> Basins { get; set; } /* All selectable basins in this Project. */
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
            SelectedBasin = new Basin();
        }

        public void AddBasin(string Name, string ImagePath)
        {
            Basin Bs = new Basin();
            Bs.Name = Name;
            Bs.BasinImagePath = ImagePath;

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

            Bs.Layers.Add(BgLayer);
            Basins.Add(Bs); 
            SelectedBasin = Bs; 

        }

        public void CreateNewProject(string Name, string ImagePath)
        {
            AddBasin(Name, ImagePath); 
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

        public void Undo()
        {
            SelectedBasin = History[CurrentHistoryPoint];
            CurrentHistoryPoint--; 
        }

    }
}
