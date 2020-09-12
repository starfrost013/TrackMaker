using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Track_Maker
{
    /// <summary>
    /// TProj-v2 format
    /// 
    /// Priscilla v442
    /// 
    /// 2020-09-12
    ///
    /// </summary>
    public class XMLv2 : IExportFormat
    {
        public bool AutoStart { get; set; }
        public string Name { get; set; }
        public XMLv2()
        {
            AutoStart = false;
            Name = "XML";
        }

        /// <summary>
        /// Get the name of the XMLv2 class. 
        /// </summary>
        /// <returns></returns>
        public string GetName() => Name;

        /// <summary>
        /// Export a basin to this format.
        /// </summary>
        /// <param name="Basin">The basin to export</param>
        /// <returns></returns>
        public bool Export(Basin Basin)
        {
            // Implement later
            throw new NotImplementedException();
        }

        /// <summary>
        /// Why do we mandate this?
        /// </summary>
        /// <param name="Basin">The basin to export</param>
        /// <param name="FileName">The file name to use to export</param>
        /// <returns></returns>
        public bool ExportCore(Basin Basin, string FileName)
        {
            // Implement later
            throw new NotImplementedException();
        }

        /// <summary>
        /// Import this
        /// </summary>
        /// <returns>The imported basin.</returns>
        public Basin Import()
        {
            // Implement later
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// This will be removed - export formats will not generate previews in v2
        /// </summary>
        /// <param name="Canvas"></param>
        public void GeneratePreview(Canvas Canvas)
        {
            throw new NotImplementedException();
        }
    }
}
