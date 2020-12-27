// Type Redirection
using DiagResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

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
        public bool DisplayPreview { get; set; }
        public bool DisplayQualityControl { get; set; }
        public string Name { get; set; }

        public ExportHURDAT2()
        {
            Name = "Best-track (HURDAT2)";
            GlobalStateP.CurrentExportFormatName = GetType().ToString();
        }

        public string GetName() => Name;

        public ImportResult Import()
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();

            FBD.Description = $"Open {GetName()} format folder";

            ImportResult IR = new ImportResult();

            DiagResult DR = FBD.ShowDialog();

            switch (DR)
            {
                case DiagResult.OK:
                    string SelectedPath = FBD.SelectedPath;
                    return ImportCore(SelectedPath);
                case DiagResult.Cancel:
                    IR.Status = ExportResults.Cancelled;
                    return IR;
                default:
                    Error.Throw("Fatal Error!!", $"Unhandled ExportResult {DR}!", ErrorSeverity.FatalError, 320);
                    return null; // will not run
            }

            
        }

        public ImportResult ImportCore(string SelectedPath)
        {
            ImportResult IR = new ImportResult();

            if (!ATCFHelperMethods.Export_CheckDirectoryValidForImport(SelectedPath)) 
            {
                Error.Throw("Fatal HURDAT2 Import Error", "Attempted to import nonexistent directory.", ErrorSeverity.Error, 321);
                IR.Status = ExportResults.Error;
                return IR;
            }
            else
            {
                List<string> FileList = Directory.EnumerateFiles(SelectedPath).ToList();

                for (int i = 0; i < FileList.Count; i++)
                {
                    string FileName = FileList[i];

                    List<string> Hurdat2Strings = File.ReadAllLines(FileName).ToList();

                    for (int j = 0; j < Hurdat2Strings.Count(); j++)
                    {
                        string HD2String = Hurdat2Strings[j];

                        // non-header
                        if (j != 0)
                        {
                            List<string> Components = HD2String.Split(',').ToList();

                            // HURDAT2 Format components
                            // 20151020, 0600,  , TD, 13.4N,  94.0W,  25, 1007,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
                            // 0 = date (YYYYMMDD)
                            // 1 = time (YYYYMMDD) ignore this until iris when we will explictly store node dates
                            // 2 = reserved, unused
                            // 3 = category
                            // 4 = latitude
                            // 5 = longitude
                            // 6 = wind speed
                            // 7 = pressure
                            // rest are wind radii 34/50/64kt
                            // Currently we only care about 0, 1, 3, 4, 5, and 6

                            string _Date = Components[0];
                            string _Time = Components[1];
                            string _Category = Components[3];
                            string _Latitude = Components[4];
                            string _Longitude = Components[5];
                            string _WindSpeed = Components[6];

                            // Trim everything.
                            _Date = _Date.Trim();
                            _Time = _Time.Trim();
                            _Category = _Category.Trim();
                            _Latitude = _Latitude.Trim();
                            _Longitude = _Longitude.Trim();
                            _WindSpeed = _WindSpeed.Trim();
                        }
                        // this can and will be refactored
                        else
                        {
                            // HURDAT2 Format Header
                        }
                        
                        // set up the basin if this is the first node of the first file

                        // set up the storm if this is the first node of any file (TERRIBLE SHIT NO GOOD)

                    }
                }
            }
        }

        public bool Export(Project Proj)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "Folder|*.";
                SFD.Title = $"Save to {GetName()} - name folder";
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
                        // write HURDAT2 Format Header

                        // should probably make this a method at some point
                        string BasAbbreviation = Bas.Abbreviation;
                        string StormID = Utilities.PadZero(Sto.Id); // this should return an int
                        string StormName = Sto.Name;

                        string Year = Sto.FormationDate.ToString("yyyy");

                        int NoOfSpacesBeforeName = 19 - Sto.Name.Length;

                        string StormIdString = Sto.Id.ToString();

                        int NoOfSpacesBeforeStormId = 7 - StormIdString.Length;

                        string StDesignation = $"{BasAbbreviation}{StormID}{Year}";

                        SW.Write(StDesignation);
                        SW.Write(',');

                        for (int i = 0; i < NoOfSpacesBeforeName; i++) SW.Write(' ');

                        SW.Write(StormName);
                        SW.Write(',');

                        for (int i = 0; i < NoOfSpacesBeforeStormId; i++) SW.Write(' ');

                        SW.Write(',');
                        SW.WriteLine();

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

            // on success
            GlobalStateP.SetCurrentOpenFile(FileName);
            return true; 
        }
    }
}
