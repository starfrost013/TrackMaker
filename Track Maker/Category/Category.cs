using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Get an abbreviated category name. (v2.0.463+)
        /// </summary>
        /// <param name="CatLength">The length of the abbreviation you wish to obtain.</param>
        /// <param name="BTFormat">Best track format - convert to uppercase before returning.</param>
        /// <returns></returns>
        public string GetAbbreviatedCategoryName(int CatLength, bool BTFormat = false)
        {
            if (CatLength > Name.Length || CatLength < 0)
            {
                Error.Throw("Fatal Error", $"Invalid call to Category.GetAbbreviatedCategoryName() - length was {CatLength}, must be between 0 and {Name.Length}!", ErrorSeverity.FatalError, 126);
                return null; 
            }

            string Substring = Name.Substring(0, CatLength);

            Debug.Assert(Substring != null);

            if (BTFormat) Substring = Substring.ToUpper(); 

            return Substring; 
        }

        /// <summary>
        /// Get an abbreviated category name. (v2.0.463+)
        /// </summary>
        /// <param name="CatLength">The length of the abbreviation you wish to obtain.</param>
        /// <param name="BTFormat">Best track format - convert to uppercase before returning.</param>
        /// <returns></returns>
        public string GetAbbreviatedCategoryName(string SourceString, int NoOfWords, int StartWordIndex = 0, bool BTFormat = false)
        {

            string[] Words = SourceString.Split(' '); 

            if (Words.Length > NoOfWords || NoOfWords < 0)
            {
                Error.Throw("Fatal Error", $"Invalid call to Category.GetAbbreviatedCategoryName() - length was {CatLength}, must be between 0 and {Name.Length}!", ErrorSeverity.FatalError, 126);
                return null;
            }

            List<char> Chars = new List<char>();

            for (int i = StartWordIndex; i < NoOfWords; i++ )
            {
                string Wrd = Words[i];
                Chars.Add(Wrd[0]);
            }

            string Final = Chars.ToString(); 

            Debug.Assert(Final != null);

            if (BTFormat) Final = Final.ToUpper();

            return Final;
        }
    }

}
