// Type Redirection
using DiagResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

using Microsoft.Win32;
using TrackMaker.UI.Logging;
using TrackMaker.UI.ScaleUtilities;
using TrackMaker.UI.StringUtilities;
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
            GlobalState.CurrentExportFormatName = GetType().ToString();
        }

        public string GetName() => Name;

        public ImportResult Import()
        {
            ImportResult IR = new ImportResult();

            try
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();

                FBD.Description = $"Open {GetName()} format folder";

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
            catch (FormatException err)
            {
                Error.Throw("Fatal HURDAT2 Import Error", $"Malformed HURDAT2 file - invalid format when converitng between types\n\n{err}", ErrorSeverity.Error, 325);
                IR.Status = ExportResults.Error;
                return IR;
            }
            catch (OverflowException err)
            {
                Error.Throw("Fatal HURDAT2 Import Error", $"Malformed HURDAT2 file - intensity overflow:\n\n{err}", ErrorSeverity.Error, 324);
                IR.Status = ExportResults.Error;
                return IR;
            }

            
        }

        public ImportResult ImportCore(string SelectedPath)
        {
            ImportResult IR = new ImportResult();

            Project Proj = new Project();
            Proj.FileName = $@"{SelectedPath}\*.*";

            if (!ATCFHelperMethods.Export_CheckDirectoryValidForImport(SelectedPath, CoordinateFormat.HURDAT2)) 
            {
                IR.Status = ExportResults.Error;
                return IR;
            }
            else
            {
                List<string> FileList = Directory.EnumerateFiles(SelectedPath).ToList();

                Basin Bas = new Basin();

                for (int i = 0; i < FileList.Count; i++)
                {
                    string FileName = FileList[i];

                    List<string> Hurdat2Strings = File.ReadAllLines(FileName).ToList();

                    Storm Sto = new Storm();

                    for (int j = 0; j < Hurdat2Strings.Count(); j++)
                    {
                        string HD2String = Hurdat2Strings[j];

                        Node CN = new Node();

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
                            string _Category = Components[4];
                            string _Latitude = Components[5];
                            string _Longitude = Components[6];
                            string _WindSpeed = Components[7];

                            // Trim everything.
                            _Date = _Date.Trim();
                            _Time = _Time.Trim();
                            _Category = _Category.Trim();
                            _Latitude = _Latitude.Trim();
                            _Longitude = _Longitude.Trim();
                            _WindSpeed = _WindSpeed.Trim();

                            // first real information
                            if (j == 1)
                            {
                                string DateString = $"{_Date}, {_Time}";
                                Sto.FormationDate = ParsingUtil.ParseATCFDateTime(DateString, CoordinateFormat.HURDAT2);
                            }

                            CN.Id = j;
                            RealStormType RST = ATCFHelperMethods.Export_IdentifyRealType(_Category);
                            
                            CN.Intensity = Convert.ToInt32(_WindSpeed);
                            CN.NodeType = ATCFHelperMethods.Export_GetStormType(_Category);

                            if (CN.NodeType == null)
                            {
                                Error.Throw("Error!", "Invalid or unknown stormtype detected!", ErrorSeverity.Error, 322);
                                IR.Status = ExportResults.Error;
                                return IR;
                            }

                            Coordinate Coordinate = Coordinate.FromSplitCoordinate(_Longitude, _Latitude, CoordinateFormat.HURDAT2);

                            CN.Position = Bas.FromCoordinateToNodePosition(Coordinate);

                            Sto.AddNode(CN);

                        }
                        // this can and will be refactored
                        else
                        {
                            // HURDAT2 Format Header
                            string HD2Header = HD2String;

                            List<string> HD2HeaderComponents = HD2Header.Split(',').ToList();

                            string HD2ID = HD2HeaderComponents[0];
                            string HD2Name = HD2HeaderComponents[1];
                            string HD2AdvisoryCount = HD2HeaderComponents[2]; // we don't use this

                            if (HD2ID.Length != 8)
                            {
                                Error.Throw("Error!", "Invalid ID field in HURDAT2 storm header.", ErrorSeverity.Error, 323);
                                IR.Status = ExportResults.Error;
                                return IR;
                            }

                            string BasinAbbreviation = HD2ID.Substring(0, 2);

                            // set up the basin if this is the first node of the first file

                            if (i == 0)
                            {
                                Bas = Proj.GetBasinWithAbbreviation(BasinAbbreviation);

                                if (Bas.CoordsLower == null || Bas.CoordsHigher == null)
                                {
                                    Error.Throw("Error!", "This basin is not supported by the HURDAT2 format as it does not have defined boundaries in Basins.xml.", ErrorSeverity.Error, 326);
                                    IR.Status = ExportResults.Error;
                                    return IR;
                                }

                                // sets up a background layer for us so we do not need to create one manually
                                Proj.InitBasin(Bas); 
                            }

                            Sto.Name = HD2Name;
                            continue; 
                        }

                        Sto.AddNode(CN);

                        // set up the storm if this is the first node of any file (TERRIBLE SHIT NO GOOD)

                    }

                    Bas.AddStorm(Sto); 
                }

                Proj.AddBasin(Bas);
            }

            IR.Project = Proj;
            IR.Status = ExportResults.OK;
            return IR;
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

                //this feature is totally moot
                //this will be reimplemented in 3.0 better
                //Directory.CreateDirectory(Bas.Name);
                //Directory.SetCurrentDirectory(Bas.Name);

                List<Storm> FlatStorms = Bas.GetFlatListOfStorms(); 

                foreach (Storm Sto in FlatStorms)
                {
                    using (StreamWriter SW = new StreamWriter(new FileStream($"{Bas.Name}_{Sto.Name}.dat", FileMode.Create)))
                    {
                        // write HURDAT2 Format Header

                        // should probably make this a method at some point
                        string BasAbbreviation = Bas.Abbreviation;
                        string StormAdvisoryCount = Utilities.PadZero(Sto.NodeList.Count); // this should return an int
                        string StormName = Sto.Name;

                        string Year = Sto.FormationDate.ToString("yyyy");

                        if (StormName.Length > 19) Sto.Name = Sto.Name.Substring(0, 19);

                        int NoOfSpacesBeforeName = 19 - StormName.Length;

                        if (StormAdvisoryCount.Length > 7) Sto.Name = Sto.Name.Substring(0, 7);

                        int NoOfSpacesBeforeStormAdvisoryCount = 7 - StormAdvisoryCount.Length;

                        string StDesignation = $"{BasAbbreviation}{StormAdvisoryCount}{Year}";

                        SW.Write(StDesignation);
                        SW.Write(',');

                        for (int i = 0; i < NoOfSpacesBeforeName; i++) SW.Write(' ');

                        SW.Write(StormName);
                        SW.Write(',');

                        for (int i = 0; i < NoOfSpacesBeforeStormAdvisoryCount; i++) SW.Write(' ');

                        SW.Write(StormAdvisoryCount);
                        SW.Write(',');
                        SW.WriteLine();

                        foreach (Node No in Sto.NodeList)
                        {
                            // this is going to go here, too tired for this

                            //Temporary Code
                            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;

                            Category Cat = Sto.GetNodeCategory(No, MnWindow.Catman.CurrentCategorySystem);

                            if (Cat == null)
                            {
                                Logging.Log("Export Warning: Skipping node with nonexistent or illegal category");
                                continue;
                            }

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

                            string[] CatWords = Cat.Name.Split(' ');

                            // select the last word
                            string Abbv = Cat.GetAbbreviatedCategoryName(Cat.Name, CatWords.Length - 1, 0, 1, true);

                            SW.Write($"{Abbv}, ");

                            Coordinate CD = Proj.SelectedBasin.FromNodePositionToCoordinate(No.Position);
                            
                            // dumb fucking piece of shit hack because what the fuck is compatibility you fucking NOAA dumb fucks
                            CD.Coordinates = new Point(CD.Coordinates.X / 10, CD.Coordinates.Y / 10); 
                            SW.Write($"{CD.Coordinates.X}{CD.Directions[0].ToString()}, {CD.Coordinates.Y}{CD.Directions[1]}, ");


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
            GlobalState.SetCurrentOpenFile(FileName);
            return true; 
        }
    }
}
