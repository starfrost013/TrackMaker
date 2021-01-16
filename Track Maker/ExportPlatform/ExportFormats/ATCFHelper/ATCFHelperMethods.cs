using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TrackMaker.Core;
using TrackMaker.Util.StringUtilities;

namespace Track_Maker
{
    public static class ATCFHelperMethods
    {
        public static bool Export_DeleteAllFilesInSpecifiedDirectory(string DirectoryPath)
        {
            List<string> FilesInDirectory = Directory.EnumerateFiles(DirectoryPath).ToList();

            foreach (string FileInDirectory in FilesInDirectory)
            {
                File.Delete(FileInDirectory);
            }

            return true;
        }

        /// <summary>
        /// Checks if an ATCF file is valid.
        /// </summary>
        /// <param name="PathToDirectory"></param>
        /// <returns></returns>
        // Iris: genericised input validation
        public static bool Export_CheckDirectoryValidForImport(string PathToDirectory, CoordinateFormat AgencyFormat = CoordinateFormat.ATCF)
        {
            if (!Directory.Exists(PathToDirectory))
            {
                Error.Throw("Error", "Import error: can't import nonexistent folder!", ErrorSeverity.Error, 282);
                return false;
            }

            List<string> FilesInDirectory = Directory.EnumerateFiles(PathToDirectory).ToList();

            if (FilesInDirectory.Count == 0)
            {
                Error.Throw("Error", "Import error: can't import empty folder!", ErrorSeverity.Error, 277);
                return false;
            }

            foreach (string FileInDirectory in FilesInDirectory)
            {
                // check 1: file extension
                if (!FileInDirectory.Contains(".dat"))
                {
                    Error.Throw("Error", "Import error: This directory has files that are not in the ATCF BestTrack format. Please remove these files from the folder before continuing.", ErrorSeverity.Error, 279);
                    return false;
                }
                else
                {
                    // check 2: file size. A reasonable estimation for the minimum size of one of these files is 200 bytes - a valid line should be 224 bytes, but some could be smaller, so 200 bytes
                    DirectoryInfo DI = new DirectoryInfo(PathToDirectory);

                    List<FileInfo> FI = DI.EnumerateFiles().ToList();

                    foreach (FileInfo FIn in FI)
                    {
                        if (FIn.Length < 200)
                        {
                            Error.Throw("Error", "Import error: This directory has files that are not in the ATCF BestTrack format. Please remove these files from the folder before continuing.", ErrorSeverity.Error, 280);
                            return false;
                        }
                    }

                    // check 3: file data. Should defeat everyone without technical knowledge and everyone who is not intentionally attempting to insert invalid data.

                    using (BinaryReader BR = new BinaryReader(new FileStream(FileInDirectory, FileMode.Open)))
                    {

                        int FirstCommaIndex = 2;
                        int SecondCommaIndex = 6;

                        if (AgencyFormat == CoordinateFormat.HURDAT2)
                        {
                            FirstCommaIndex = 8;
                            SecondCommaIndex = 28;
                        }

                        long BytesRemaining = BR.BaseStream.Length - BR.BaseStream.Position;

                        if (BytesRemaining < SecondCommaIndex)
                        {
                            Error.Throw("Error", "Invalid HURDAT2 format file - error verifying file format (stage 3)", ErrorSeverity.Error, 328);
                            return false;
                        }

                        byte[] Bytes = BR.ReadBytes(SecondCommaIndex + 1);

                        Debug.Assert(Bytes.Length == (SecondCommaIndex + 1));

                        if (Bytes[FirstCommaIndex] == 0x2C && Bytes[SecondCommaIndex] == 0x2C)
                        {
                            BR.Close();
                            return true;
                        }
                        else
                        {
                            BR.Close();
                            Error.Throw("Error", "Import error: This directory has files that are not in the ATCF BestTrack format. Please remove these files from the folder before continuing.", ErrorSeverity.Error, 281);
                            return false;
                        }
                    }
                }
            }

            // for now
            return false;
        }

        public static RealStormType Export_IdentifyRealType(string NodeType)
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
                    if (NodeType.ContainsCaseInsensitive("LO"))
                    {
                        return RealStormType.Invest;
                    }
                    else
                    {
                        return RealStormType.Tropical;
                    }
                    
                }
            }
        }

        public static StormType2 Export_GetStormType(string NodeType)
        {
            RealStormType RST = ATCFHelperMethods.Export_IdentifyRealType(NodeType);
#if PRISCILLA
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            StormTypeManager STM = MnWindow.ST2Manager;
#else
            StormTypeManager STM = MnWindow.GetST2Manager();
#endif
            StormType2 ST2 = STM.GetStormTypeWithRealStormTypeName(RST);

            return ST2;
        }

    }
}
