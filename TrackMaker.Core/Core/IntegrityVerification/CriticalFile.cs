using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrackMaker.Util.StringUtilities;

namespace TrackMaker.Core
{
    /// <summary>
    /// Track Maker 2.1
    /// 
    /// Software Integrity Verification (Iris v719)
    /// 
    /// Verifies the integrity of critical files required for the functioning of the Track Maker before they are loaded. 
    /// 
    /// CriticalFile.cs: Defines a critical file; path, size, and SHA256 hash
    /// </summary>
    public class CriticalFile
    {
        /// <summary>
        /// The path to this critical file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The intended size of this critical file. (only used for CriticalFiles with Strictness set to VerificationStrictness.Moderate or above)
        /// </summary>
        public int FSize { get; set; }

        /// <summary>
        /// The SHA256 hash of this critical file (only used for CriticalFiles with Strictness set to VerificationStrictness.Strict)
        /// </summary>
        public byte[] FSHA256 { get; set; } // array b ecause of the MS SHA256 compute functions

        /// <summary>
        /// The verification strictness of this file
        /// </summary>
        public VerificationStrictness Strictness { get; set; }

        public VerificationResult Verify()
        {
            // mild handling

            VerificationResult VR = new VerificationResult();
            VR.File = this;

            // might need to change this a bit
            if (Path != null)
            {
                VR.WasFileValid = true;

                if (Strictness == VerificationStrictness.Moderate)
                {
                    if (FSize < 0)
                    {
                        VR.WasFileValid = false;
                        return VR;
                    }
                }
                else if (Strictness == VerificationStrictness.Strict)
                {
                    if (FSize > 0 && FSHA256 != null)
                    {
                        if (FSHA256.Length != 32)
                        {
                            VR.FailureReason = $"Invalid SHA256 hash - must be 32 bytes long, was {FSHA256.Length}!";
                            return VR;
                        }
                    }
                    else
                    {
                        VR.FailureReason = "CriticalFile entry invalid - invalid file size or null SHA256 hash!";
                        VR.WasFileValid = false;
                        return VR;
                    }
                }

                
            }
            else
            {
                // false by default
                return VR;
            }

            switch (Strictness)
            {
                case VerificationStrictness.Low:
                    // false by default
                    VR.Successful = VerifyFileExistence();

                    return VR;
                case VerificationStrictness.Moderate:
                    VR.Successful = VerifyFileExistence();

                    if (!VR.Successful)
                    {
                        VR.FailureReason = "File does not exist!";
                        return VR;
                    }

                    VR.Successful = VerifyFileSize();

                    if (!VR.Successful) VR.FailureReason = $"File size is invalid - should be {FSize.ToString()} bytes!";
                    
                    return VR;
                case VerificationStrictness.Strict:

                    VR.Successful = VerifyFileExistence();

                    if (!VR.Successful)
                    {
                        VR.FailureReason = "File does not exist!";
                        return VR;
                    }

                    VR.Successful = VerifyFileSize();

                    if (!VR.Successful)
                    {
                        VR.FailureReason = $"File size is invalid - should be {FSize.ToString()} bytes!";
                        return VR;
                    }

                    VR.Successful = VerifyFileSHA256();

                    if (!VR.Successful) VR.FailureReason = $"File is corrupted or modified - please turn on DeveloperMode if you are working with source code!\n The SHA256 hash should be {FSHA256.ByteArrayToHexString()}.";

                    return VR;
            }

            // someting went wrong 
            return VR; 

        }

        // BAD?
        private bool VerifyFileExistence() => File.Exists(Path);
        
        private bool VerifyFileSize()
        {
            FileInfo FA = new FileInfo(Path);

            if (FA.Length != FSize)
            {
                return false;
            }
            else
            {
                return true; 
            }
        }

        private bool VerifyFileSHA256()
        {
            SHA256 FileSHA = SHA256.Create();

            using (BinaryReader BR = new BinaryReader(new FileStream(Path, FileMode.Open)))
            {
                byte[] HashValue = FileSHA.ComputeHash(BR.BaseStream);

                bool Result = (HashValue.SequenceEqual(FSHA256));

                Logging.Log($"SHA256 hash-based file verification in progress for file {Path}");
                // Add an additional space to make comparison easier
                Logging.Log($"Expected hash: {FSHA256.ByteArrayToHexString()}");
                Logging.Log($"Actual hash:   {HashValue.ByteArrayToHexString()}");

                Logging.Log($"Result: {Result}");

                return Result;
            }
            
        }
    }
}
