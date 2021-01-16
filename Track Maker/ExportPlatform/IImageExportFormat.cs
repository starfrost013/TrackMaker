using TrackMaker.UI.ScaleUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Export format for images and other formats where a quality control may be required (ex: 3d model in v4)
    /// </summary>
    public interface IImageExportFormat : IExportFormat
    {
        /// <summary>
        /// Does this format display a QualityControl in ExportUI?
        /// </summary>
        bool DisplayQualityControl { get; set; }

        /// <summary>
        /// Current image quality to use for export.
        /// </summary>
        ImageQuality Quality { get; set; }
    }
}
