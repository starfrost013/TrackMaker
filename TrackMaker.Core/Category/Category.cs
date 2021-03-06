﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization; 
using System.Threading.Tasks;
using System.Windows; 
using System.Windows.Media;

namespace TrackMaker.Core
{
    
    /// <summary>
    /// An individual category, as a part of a CategorySystem.
    /// </summary>
    public class Category
    {

        /// <summary>
        /// Abbreviation of this category's name
        /// </summary>
        [XmlElement("Abbreviation")]
        public string Abbreviation { get; set; } // the abbreviation of this category

        /// <summary>
        /// The colour used by track nodes/points in this category.
        /// </summary>
        [XmlElement("Color")]
        public Color Color { get; set; }

        /// <summary>
        /// Name of this category
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Lower bound of this category
        /// </summary>
        [XmlElement("LowerBound")]
        public int LowerBound { get; set; }

        /// <summary>
        /// Higher bound of this category
        /// </summary>
        [XmlElement("HigherBound")]
        public int HigherBound { get; set; }

        /// <summary>
        /// Hotkey for this category
        /// </summary>
        [XmlElement("ForceHotkey")]
        public DynaHotkey Hotkey { get; set; }

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
                Error.Throw("Fatal Error", $"Invalid call to Category.GetAbbreviatedCategoryName() - length was {Words.Length}, must be between 0 and {NoOfWords}!", ErrorSeverity.FatalError, 282);
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
            
            // Build each word
            for (int i = StartWordIndex; i <= NoOfWords; i++ )
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

        /// <summary>
        /// Get an abbreviated category name. (v2.1.701.3+)
        /// Added in Iris build 701.3. A "raw" variant of the previous method.
        /// </summary>
        /// <param name="CatLength">The length of the abbreviation you wish to obtain.</param>
        /// <param name="BTFormat">Best track format - convert to uppercase before returning.</param>
        /// <returns></returns>
        public string GetAbbreviatedCategoryName(string SourceString, int NoOfWords, int StartWordIndex = 0, int LettersPerWord = 1)
        {
            string[] Words = SourceString.Split(' ');

            if (NoOfWords > Words.Length || NoOfWords < 0 || StartWordIndex > Words.Length)
            {
                Error.Throw("Fatal Error", $"Invalid call to Category.GetAbbreviatedCategoryName() - length was {Words.Length}, must be between 0 and {NoOfWords}!", ErrorSeverity.FatalError, 282);
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

            // Build each word
            for (int i = StartWordIndex; i <= NoOfWords; i++)
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

            return Final;
        }

    }

}
