using Microsoft.Win32; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;

namespace Track_Maker
{
    /// <summary>
    /// BestTrack export services
    /// 
    /// This is crap because I don't understand what half of this shitty format does. 
    /// 
    /// Created: 2020-05-17
    /// 
    /// Modified: 2020-12-24
    /// </summary>
    /// 
    public class ExportBestTrack : IExportFormat
    {
        public bool DisplayPreview { get; set; }
        public bool DisplayQualityControl { get; set; }
        public MainWindow MnWindow { get; set; }
        public string Name { get; set; }

        public ExportBestTrack()
        {
            Name = "Best-track (ATCF)";
            MnWindow = (MainWindow)Application.Current.MainWindow;
            GlobalStateP.CurrentExportFormatName = GetType().ToString();
        }

        public string GetName()
        {
            return Name; 
        }

        public ImportResult Import()
        {
            try
            {

                ImportResult IR = new ImportResult();

                System.Windows.Forms.FolderBrowserDialog OFD = new System.Windows.Forms.FolderBrowserDialog()
                {
                    Description = $"Open {GetName()} format folder",
                };

#if DANO
                StormTypeManager ST2Manager = GlobalState.GetST2Manager(); 
#else
                MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
                StormTypeManager ST2Manager = MnWindow.ST2Manager;
#endif

                if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    if (OFD.SelectedPath == "") return null;

                    Project Proj = ImportCore(ST2Manager, OFD.SelectedPath);

                    if (Proj == null)
                    {
                        IR.Status = ExportResults.Error;
                        return IR;
                    }
                    else
                    {
                        IR.Project = Proj;
                        IR.Status = ExportResults.OK;
                        return IR;
                    }
                }
                else
                {
                    //temp

                    IR.Status = ExportResults.Cancelled;
                    return IR; 
                }
            }
            catch (DirectoryNotFoundException)
            {
                Error.Throw("Fatal Error", "Attempted to import a nonexistent ATCF DatFolder", ErrorSeverity.Error, 241);
                return null;
            }
            catch (PathTooLongException)
            {
                Error.Throw("Error", "The path to the file is longer than 260 characters. Please shorten it.", ErrorSeverity.Error, 150);
                return null; 
            }
            
        }

        // pre-globalstate...refactor this in Dano to not have that passed to it
        public Project ImportCore(StormTypeManager ST2M, string FolderName)
        {
            // this is one of the worst fucking file formats I have ever laid my fucking eyes on, NOAA are a bunch of fucking wanker twats, nobody should use this pile of crap

            string[] Storms = Directory.GetFiles(FolderName);

            // this is terrible design 
            Project Proj = new Project();
            Proj.FileName = $"{FolderName}/*.*";
            Basin Bas = new Basin();

            // this is still a mess for now
            // not foreach as we can use it for setting up the basin
            for (int i = 0; i < Storms.Count(); i++)
            {
                string StormFileName = Storms[i];

                string[] ATCFLines = File.ReadAllLines(StormFileName);

                // holy fucking shit i hate the ATCF format so fucking muc
                Layer Lyr = new Layer();
               
                Storm Sto = new Storm(); 

                StormType2 StormType = new StormType2();
                DateTime StormFormationDT = new DateTime(1959, 3, 10);

                // XML OR JSON OR FUCKING ANYTHING PLS
                // not foreach because it makes it slightly easier to set the date
                for (int j = 0; j < ATCFLines.Length; j++)
                {
                    string ATCFLine = ATCFLines[i];

                    string[] Components = ATCFLine.Split(',');

                    string _StrAbbreviation = Components[0];
                    // get the stuff we actually need
                    string _StrId = Components[1];
                    string _StrTime = Components[2];
                    string _StrTimeSinceFormation = Components[3];
                    string _StrCoordX = Components[6];
                    string _StrCoordY = Components[7];
                    string _StrIntensity = Components[9];
                    string _StrCategory = Components[10];
                    string _StrName = Components[28]; // bleh 

                    // trim it
                    _StrAbbreviation = _StrAbbreviation.Trim();
                    _StrId = _StrId.Trim(); 
                    _StrTime = _StrTime.Trim();
                    _StrTimeSinceFormation = _StrTimeSinceFormation.Trim();
                    _StrCoordX = _StrCoordX.Trim();
                    _StrCoordY = _StrCoordY.Trim();
                    _StrIntensity = _StrIntensity.Trim();
                    _StrCategory = _StrCategory.Trim();
                    _StrName = _StrName.Trim();

                    // initialise the basin with the abbreviation loaded from XML
                    // we just use the name if there is no abbreviation specified in XML
                    int Intensity = 0; 
                    // first iteration...
                    if (j == 0)
                    {

                        if (i == 0)
                        {
                            Bas = Proj.GetBasinWithAbbreviation(_StrAbbreviation);

                            if (Bas.CoordsHigher == null || Bas.CoordsLower == null)
                            {
                                Error.Throw("Error!", "This basin is not supported by the ATCF format as it does not have defined borders.", ErrorSeverity.Error, 249);
                                return null;
                            }
                        }

                        Intensity = Convert.ToInt32(_StrIntensity);

                        Sto.FormationDate = ParsingUtil.ParseATCFDateTime(_StrTime);

                        Lyr.Name = _StrName; 

                        if (_StrName == null)
                        {
                            Error.Throw("Error!", "Attempted to load storm with an invalid name!", ErrorSeverity.Error, 245);
                            return null; 
                        }
                        else
                        {
                            Sto.Name = _StrName;
                        }

                    }

                    int Id = Convert.ToInt32(_StrId);
                    Coordinate Coord = Coordinate.FromSplitCoordinate(_StrCoordX, _StrCoordY);

                    // create a node and add it

                    Node Nod = new Node();
                    Nod.Id = Id;
                    Nod.Intensity = Intensity;

                    Nod.Position = Bas.FromCoordinateToNodePosition(Coord);
                    Nod.NodeType = GetStormType(_StrCategory);

                    Sto.AddNode(Nod);                    
                }

                Lyr.AddStorm(Sto);
                Bas.AddLayer(Lyr);
            }

            Proj.AddBasin(Bas);

            return Proj;
            
        }

        /// <summary>
        /// Needs to be majorly updated and refactored for v2
        /// </summary>
        /// <param name="basin"></param>
        /// <returns></returns>
        public bool Export(Project Project)
        {
            // Change to FolderBrowserDialog (v616)
            try
            {

                System.Windows.Forms.FolderBrowserDialog FFD = new System.Windows.Forms.FolderBrowserDialog();
                FFD.Description = $"Save to {GetName()} - enter folder name";

                FFD.ShowDialog();

                if (FFD.SelectedPath == "") return true;

                //utilfunc v2
                if (Directory.Exists(FFD.SelectedPath))
                {
                    Directory.Delete(FFD.SelectedPath);
                    Directory.CreateDirectory(FFD.SelectedPath);
                }

                // Run the export and if we failed clean up 
                if (!ExportCore(Project, FFD.SelectedPath))
                {
                    string _ = FFD.SelectedPath.Replace(".", "");
                    foreach (string FileName in Directory.EnumerateFiles(_))
                    {
                        File.Delete(FileName);
                    }

                    Directory.Delete(_); 
                }
            }
            catch (PathTooLongException)
            {
                Error.Throw($"Error", "Error: The path to the file is too long - please use a shorter path.", ErrorSeverity.Error, 129); 
                return false;
            }
            catch (IOException err)
            {
                Error.Throw("Error", "An error occurred during export to ATCF BestTrack format.", ErrorSeverity.Error, 130);
#if DEBUG
                Error.Throw("Error", $"An error occurred during export to ATCF BestTrack format.\n\n{err}", ErrorSeverity.Error, 130);
#endif
                return false;
            }

            return true;
        }

        /// <summary>
        /// Needs to be majorly updated and refactored for v2
        /// </summary>
        /// <param name="basin"></param>
        /// <returns></returns>
        public bool ExportCore(Project Project, string FileName)
        {
            Directory.CreateDirectory(FileName);

            Basin SelBasin = Project.SelectedBasin;

            // create a file for each storm
            foreach (Storm Storm in SelBasin.GetFlatListOfStorms())
            {
                if (Storm.Name.Length > 12)
                {   
                    // Woah, calm it (Priscilla 442)
                    Error.Throw("Error", "This format, the ATCF BestTrack format, has several limitations (and is obsolete to begin with). Exporting to this format is not recommended. \n\nLimitations:\n\n- It does not support storms with names with a length above twelve characters.\n- It does not support abbreviated character names of lengths more than three characters.\n- It uses exceedingly bizarre spelling.\nIt is from the 1980s.\n\n- There is no documentation on the Internet, despite extensive efforts to acquire some.", ErrorSeverity.Error, 131); 
                    return false;
                }

                string StormFileName = Path.Combine(FileName, $@"{Storm.Name}.dat");
                // Create a new file for each storm.
                using (StreamWriter SW = new StreamWriter(new FileStream(StormFileName, FileMode.Create)))
                {
                    // For each node. 
                    foreach (Node Node in Storm.NodeList)
                    {

                        if (!Node.IsRealType())
                        {
                            Error.Throw("Error", "Custom storm types are not supported when exporting to ATCF/HURDAT2 format.", ErrorSeverity.Error, 248);
                            return false;
                        }

                        if (SelBasin.Abbreviation != null)
                        {
                            SW.Write($"{SelBasin.Abbreviation}, ");
                        }
                        else
                        {
                            SW.Write($"{SelBasin.Name}, ");
                        }

                        // Pad with a zero if <10 and write storm id.
                        SW.Write($"{Utilities.PadZero(Storm.Id + 1)}, ");

                        // Get the node date
                        DateTime NodeDate = Storm.GetNodeDate(Node.Id); 

                        SW.Write($"{NodeDate.ToString("yyyyMMdd")}{Utilities.PadZero(NodeDate.Hour)}");

                        // Some weird padding shit (It's actually time since system formation. Who knew???)
                        SW.Write(",   , ");

                        // Best marker, more useless padding
                        SW.Write("BEST,   ");

                        // God knows what this does
                        SW.Write("0, ");

                        // Node position.

                        Category Cat = Storm.GetNodeCategory(Node, MnWindow.Catman.CurrentCategorySystem);
                        
                        Coordinate X = Project.SelectedBasin.FromNodePositionToCoordinate(Node.Position); 

                        SW.Write($"{X.Coordinates.X}{X.Directions[0]},  {X.Coordinates.Y}{X.Directions[1]},  ");

                        // Intensity.

                        for (int i = 0; i < 4 - Node.Intensity.ToString().Length; i++)
                        {
                            SW.Write(" ");
                        }

                        // Convert to KT (DANO: MOVE TO DEDICATED HurricaneConversions CLASS)

                        double Intensity = Utilities.RoundNearest(Node.Intensity / 1.151, 5);

                        SW.Write($"{Intensity}, ");

                        // Pressure. We don't do this until dano and even then it will be optional. Just put 1000mbars
                        SW.Write("1000, ");

                        // Write the category and a WHOLE bunch of information that we don't need or use yet - environmental pressure etc - I don't know what most of these are tbh

                        CategorySystem CurrentCategorySystem = MnWindow.Catman.CurrentCategorySystem;

                        Category Ct = Storm.GetNodeCategory(Node, CurrentCategorySystem);

                        // v610: fix crash when exporting projects with invalid categories (i.e. storm too intense) iris: add palceholder
                        if (Ct == null) Ct = CurrentCategorySystem.GetHighestCategory();

                        string CatString = null;

                        if (Ct.Abbreviation == null)
                        {
                            int SplitLength = CatString.Split(' ').Length;
                            CatString = Cat.GetAbbreviatedCategoryName(CatString, SplitLength, 0, 1, true);
                        }
                        else
                        {
                            CatString = Ct.Abbreviation; 
                        }

                        Debug.Assert(CatString != null); 

                        if (CatString.Length > 3)
                        {
                            MessageBox.Show("Due to the limitations of the ATCF format, category names cannot be longer than 3 characters abbreviated.");
                            return false; 
                        }

                        // Placeholder information
                        for (int i = 0; i < 3 - CatString.Length; i++)
                        {
                            SW.Write(" ");
                        }
                        
                        SW.Write($"{CatString},  "); // todo: abbreviate (dano)

                        // Aforementioned information
                        SW.Write($"x,   0,    ,    0,    0,    0,    0, 1014,   90,  60,   0,   0,   L,   0,    ,   0,   0, ");

                        // Shit Format
                        for (int i = 0; i < (12 - Storm.Name.Length); i++)
                        {
                            SW.Write(" ");
                        }

                        SW.Write(Storm.Name);

                        // More pointless and irrelevant data.
                        SW.Write(", M,  0,    ,    0,    0,    0,    0, ");

                        // Why does this exist
                        SW.Write("genesis-num, ");
                        SW.Write(Utilities.PadZero(Storm.Id + 1, 2));
                        SW.Write(", \n"); // Write newline

                        // on success
                        GlobalStateP.SetCurrentOpenFile(FileName);
                    }
                }
            }

            
            return true;
        }

        private StormType2 GetStormType(string NodeType)
        {
            RealStormType RST = Export_IdentifyRealType(NodeType);
#if PRISCILLA
            StormTypeManager STM = MnWindow.ST2Manager;
#else
            StormTypeManager STM = MnWindow.GetST2Manager();
#endif
            StormType2 ST2 = STM.GetStormTypeWithRealStormTypeName(RST);

            return ST2; 
        }

        public void GeneratePreview(Canvas canvas)
        {
            throw new NotImplementedException(); 
        }

        private RealStormType Export_IdentifyRealType(string NodeType)
        {
            // This only needs to support SSHWS categories, as this is ATCF...
            if (NodeType.ContainsCaseInsensitive("SS")
                || NodeType.ContainsCaseInsensitive("SD"))
            {
                return RealStormType.Subtropical;
            }
            else
            {
                if (NodeType.ContainsCaseInsensitive("EX"))
                {
                    return RealStormType.Extratropical;
                }
                else
                {
                    return RealStormType.Tropical;
                }
            }
        }

    }
}
