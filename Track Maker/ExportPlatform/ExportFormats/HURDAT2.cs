using Microsoft.Win32;
using Starfrost.UL5.ScaleUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection; 
using System.Text;
using System.Threading.Tasks;
using System.Windows; 

namespace Track_Maker
{
    public class ExportHURDAT2 : IExportFormat
    {
        public bool AutoStart { get; set; }
        public bool DisplayQualityControl { get; set; }
        public string Name { get; set; }

        public ExportHURDAT2()
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
                        foreach (Node No in Sto.NodeList)
                        {
                            // this is going to go here, too tired for this

                            DateTime NoDate = Sto.GetNodeDate(No.Id);

                            // Write the node date
                            SW.Write(NoDate.ToString("yyyyMMdd"));

                            SW.Write(", ");

                            // Write the node time...
                            SW.Write(NoDate.ToString("HHmm"));

                            SW.Write(", ");

                            int Peak = Sto.GetPeakIntensity();

                            // Write the tag. We will support more of these in future.
                            if (Peak == No.Intensity)
                            {
                                SW.Write(", I, ");
                            }
                            else
                            {
                                SW.Write(",  , ");
                            }

                            //Temporary Code
                            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;

                            Category Cat = Sto.GetNodeCategory(No, MnWindow.Catman.CurrentCategorySystem);

                            string[] CatWords = Cat.Name.Split(' ');

                            // select the last word
                            string Abbv = Cat.GetAbbreviatedCategoryName(Cat.Name, CatWords.Length - 1, 0, 1, true);

                            SW.Write($"{Abbv}, ");

                            Coordinate CD = Proj.SelectedBasin.FromNodePositionToCoordinate(No.Position);
                            
                            // dumb fucking piece of shit hack because what the fuck is compatibility you fucking NOAA dumb fucks
                            CD.Coordinates = new Point(CD.Coordinates.X / 10, CD.Coordinates.Y / 10); 
                            SW.Write($"{CD.Coordinates.X}{CD.Directions[0].ToString()}, {CD.Coordinates.Y}{CD.Directions[0]}, ");


                            // we don't save this data lol. 
                            // yes i could just write the comma 
                            SW.Write($"{No.Intensity},   914,  150,  110,   90,  150,   80,   60,   50,   70,   45,   40,   30,   45");
                             
                            if (No.Id != Sto.NodeList.Count - 1)
                            {
                                SW.Write(","); 
                            }
                           
                           
                            SW.WriteLine();
                        }
                    }
                }

                Directory.SetCurrentDirectory(FileName); 
            }

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            return true; 
        }
    }
}
