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
    /// Category system class for deserialisation
    /// </summary>
    /// 
    [XmlRoot("CategorySystems")]
    public class CategorySystemCollection
    {
        [XmlElement("CategorySystem")]
        private List<CategorySystem> CategorySystems { get; set; }

        public CategorySystemCollection()
        {
            CategorySystems = new List<CategorySystem>();
        }


        public void Add(CategorySystem CategorySystemObject) => CategorySystems.Add(CategorySystemObject);
        public void Remove(CategorySystem CategorySystemObject) => CategorySystems.Remove(CategorySystemObject);
        public void Clear() => CategorySystems.Clear(); 
    }
}
