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
    /// A category system. Stores categories within it.
    /// </summary>

    public class CategorySystem
    {
        [XmlElement("Category")]
        public List<Category> Categories { get; set; }

        public string Name { get; set; }

        public CategorySystem()
        {
            Categories = new List<Category>();
        }

        public Category GetHighestCategory() => Categories[Categories.Count - 1];
    }
}
