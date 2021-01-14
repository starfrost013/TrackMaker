using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    /// <summary>
    /// XML deserialisation class for category systems
    /// 
    /// 2020-01-14 00:40   v2.1.659
    /// </summary>
    [XmlRoot("CategorySystems")]
    public partial class CategoryManager
    {
        [XmlElement("CategorySystem")]
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

}
