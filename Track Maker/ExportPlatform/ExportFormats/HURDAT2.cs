using Microsoft.Win32; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection; 
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker.ExportPlatform
{
    public class HURDAT2 : IExportFormat
    {
        public bool AutoStart { get; set; }
        public string Name { get; set; }

        public HURDAT2()
        {
            AutoStart = false;
            Name = "Best-track (HURDAT2)";
        }
        public string GetName() => Name;

        public Project Import()
        {
            throw new NotImplementedException(); 
        }

        public bool Export(Project Proj)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "Folder|*.";
                SFD.Title = "Save to HURDAT2 ATCF Format - name folder";
                SFD.ShowDialog();

                if (SFD.FileName != "")
                {
                    ExportCore(Proj, SFD.FileName); 
                }
                else
                {
                    return true; // exportresult?
                }
                return true; 

            }
            catch (PathTooLongException)
            {
                Error.Throw("The path is too long.", "Please shorten the path to your file to be less than 260 characters.", ErrorSeverity.Error, 124);
                return false; 
            }
        }

        public bool ExportCore(Project Proj, string FileName)
        {
            Directory.CreateDirectory(FileName);
            Directory.SetCurrentDirectory(FileName); 

            foreach (Basin Bas in Proj.OpenBasins)
            {
                Directory.CreateDirectory(Bas.Name);
                Directory.SetCurrentDirectory(Bas.Name);

                List<Storm> FlatStorms = Bas.GetFlatListOfStorms(); 

                foreach (Storm Sto in FlatStorms)
                {
                    using (StreamWriter SW = new StreamWriter(new FileStream($"{Bas.Name}_{Sto.Name}.dat", FileMode.Create)))
                    {

                    }
                }

                Directory.SetCurrentDirectory(FileName); 
            }

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            return true; 
        }
    }
}
