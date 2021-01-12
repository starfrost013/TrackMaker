using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization; 
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
        /// 
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The abbreviation of this storm type.
        /// </summary>
        /// 
        [XmlElement("Abbreviation")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Abbreviations used for best-track import purposes
        /// </summary>
        /// 
        [XmlElement("BTAbbreviation")]
        public List<string> BTAbbreviations { get; set; }

        /// <summary>
        /// A preset shape used by this storm type.
        /// </summary>
        /// 
        [XmlElement("PresetShape")]
        public StormShape PresetShape { get; set; }

        /// <summary>
        /// Does this use a preset shape?
        /// </summary>
        public bool UsePresetShape { get; set; }

        /// <summary>
        /// If UsesPresetShape is false, the shape used by this object.
        /// </summary>
        /// 
        [XmlElement("Shape")]
        public Shape Shape { get; set; }

        /// <summary>
        /// Do we force a particular colour?
        /// </summary>
        public bool ForceColour { get; set; }

        /// <summary>
        /// If ForceColour is true, the colour that is forced by this StormType.
        /// </summary>
        /// 
        [XmlElement("ForceColour")]
        public Color ForcedColour { get; set; }

        /// <summary>
        /// v630: user request [Hypercane] - force a size for this storm type. Overrides the setting.
        /// </summary>
        /// 
        public bool ForceSize { get; set; }

        /// <summary>
        /// v630: user request [Hypercane] - the size to force to.
        /// </summary>
        /// 
        [XmlElement("ForceSize")]
        public Point ForcedSize { get; set; }

        public StormType2() // for now init the shape here
        {
            Shape = new Shape();
            BTAbbreviations = new List<string>();
        }
    }
}
