using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; 
using System.Windows.Media;

namespace Track_Maker
{


    public partial class CategoryManager
    {
        public List<CategorySystem> CategorySystems { get; set; }
        public CategorySystem CurrentCategorySystem { get; set; }
        public CategoryManager()
        {
            CategorySystems = new List<CategorySystem>(); 
        }

        public bool SetCategoryWithName(string Name)
        {
            foreach (CategorySystem Catsystem in CategorySystems)
            {
                if (Catsystem.Name == Name)
                {
                    CurrentCategorySystem = Catsystem;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Priscilla [428+]
        /// </summary>
        /// <returns></returns>
        public List<string> GetCategorySystemNames()
        {
            List<string> NameList = new List<string>();

            foreach (CategorySystem CatSystem in CategorySystems)
            {
                NameList.Add(CatSystem.Name);
            }

            return NameList; 
        }

        public List<string> GetCurrentSystemCategoryNames()
        {
            List<string> NameList = new List<string>();

            foreach (Category Category in CurrentCategorySystem.Categories)
            {
                NameList.Add(Category.Name); 
            }

            return NameList;
        }
    }


    /// <summary>
    /// A category system. 
    /// </summary>
    public class CategorySystem
    {
        public List<Category> Categories { get; set; }

        public string Name { get; set; }

        public CategorySystem()
        {
            Categories = new List<Category>();
        }
    }

    /// <summary>
    /// An individual category, as part of a CategorySystem.
    /// </summary>
    public class Category
    {
        public Color Color { get; set; }
        public string Name { get; set; }
        public int LowerBound { get; set; }
        public int HigherBound { get; set; }
    }

}
