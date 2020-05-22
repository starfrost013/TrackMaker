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
        public string Name { get; set; }
        public Basin SelectedBasin { get; set; } /* Currently selected basin. */

    }
}
