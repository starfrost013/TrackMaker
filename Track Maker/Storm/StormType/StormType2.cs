using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Track_Maker
{
    /// <summary>
    /// StormType V2 (the ATCF format forced me to implement this against my will)
    /// 
    /// Priscilla   v475
    /// 
    /// 2020-10-05
    /// </summary>
    public class StormType2
    {
        /// <summary>
        /// The name of this storm type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The abbreviation of this storm type.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Abbreviations used for best-track import purposes
        /// </summary>
        public List<string> BTAbbreviations { get; set; }

        /// <summary>
        /// A preset shape used by this storm type.
        /// </summary>
        public StormShape PresetShape { get; set; }

        /// <summary>
        /// Does this use a preset shape?
        /// </summary>
        public bool UsePresetShape { get; set; }

        /// <summary>
        /// If UsesPresetShape is false, the shape used by this object.
        /// </summary>
        public Shape Shape { get; set; }

        /// <summary>
        /// Do we force a particular colour?
        /// </summary>
        public bool ForceColour { get; set; }

        /// <summary>
        /// If ForceColour is true, the colour that is forced by this StormType.
        /// </summary>
        public Color ForcedColour { get; set; }

        public StormType2() // for now init the shape here
        {
            Shape = new Shape(); 
        }
    }
}
