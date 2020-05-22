using System;
using System.Collections.Generic;
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
