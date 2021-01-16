using TrackMaker.Util.ScaleUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; 

/// <summary>
/// starfrost's Hurricane Track Maker Export Platform Interface
/// 
/// This allows us to write classes easily and quickly for exporting to any format we want. 
/// 
/// Version 1.10.0
/// December 27, 2020
/// 
/// (Priscilla - v627)
/// 
/// v2.0.627.0      V1.10.0     Removed obsolete DisplayPreview
/// v2.0.604.0      V1.9        Import: ImportResult class implemented
/// v2.0.571.0      V1.8        Removed AutoStart
/// v2.0.559.0      V1.7        Add DisplayPreview, still haven't removed AutoStart yet
/// v2.0.540.0      V1.6        Preparation for removal of autostart - split out IImageExportControl
/// v2.0.485.0      V1.5        Added boolean property for displaying QualityControl
/// v2.0.464.0      V1.4        Project now mandatory
/// </summary>
/// 
namespace Track_Maker
{
    public interface IExportFormat
    {
        string Name { get; set; } // The name of the file format to export to.
        string GetName(); // Returns the name of this ExportFormat. 
        ImportResult Import(); // Import from this file format.
        bool Export(Project Project); // Export from this file format. 
        bool ExportCore(Project Project, string FileName); // Does the actual exporting. 
        //bool DisplayPreview { get; set; } // post-beta - display preview
        //void GeneratePreview(Canvas ImportCanvas); // Generate a preview - this will be hanled by the track maker...for imports, might reintro this 
    }
}
