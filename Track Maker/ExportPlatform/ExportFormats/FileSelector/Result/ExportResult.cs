using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMaker.Core;

namespace TrackMaker
{
    /// <summary>
    /// Priscilla v604 2020-12-20 22:59
    /// 
    /// ImportResult
    /// 
    /// Result class for import and export
    /// </summary>
    public class ImportResult
    {
        public Project Project { get; set; }
        public ExportResults Status { get; set; }
    }
}
