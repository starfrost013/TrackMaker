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
        public string Abbreviation { get; set; } // the abbreviation of this category
        public string ATCFAbbreviation { get; set; } // ATCF Abbreviation
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
        public string GetAbbreviatedCategoryName(string SourceString, int NoOfWords, int StartWordIndex = 0, int LettersPerWord = 1, bool BTFormat = false)
        {

            // if we have an abbreviation return it (v602c)
            if (Abbreviation != null) return Abbreviation;

            string[] Words = SourceString.Split(' ');

            if (NoOfWords > Words.Length || NoOfWords < 0 || StartWordIndex > Words.Length)
            {
                Error.Throw("Fatal Error", $"Invalid call to Category.GetAbbreviatedCategoryName() - length was {Words.Length}, must be between 0 and {NoOfWords}!", ErrorSeverity.FatalError, 126);
                return null;
            }

            int ShortestWordLength = Words[0].Length;

            for (int i = StartWordIndex; i < (StartWordIndex + NoOfWords); i++) 
            {
                // we already ran a bounds check so we will iterate through all teh words we are checking 
                string Word = Words[i];

                if (Word.Length < ShortestWordLength) ShortestWordLength = Word.Length;

            }
            
            if (LettersPerWord < 0 || LettersPerWord > ShortestWordLength)
            {
                Error.Throw("Fatal Error", $"Invalid call to Category.GetAbbreviatedCategoryName() - letters per word were {LettersPerWord}, must be between 0 and {ShortestWordLength}!", ErrorSeverity.FatalError, 127); 
            }


            StringBuilder SB = new StringBuilder();
            
            for (int i = StartWordIndex; i < NoOfWords; i++ )
            {
                string Wrd = Words[i];
                
                for (int j = 0; j < LettersPerWord; j++)
                {
                    // ok
                    SB.Append(Wrd[j]); 
                }
            }

            string Final = SB.ToString(); 

            Debug.Assert(Final != null);

            if (BTFormat) Final = Final.ToUpper();

            return Final;
        }
    }

}
