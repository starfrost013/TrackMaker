using Starfrost.UL5.LineEndingUtilities;
using Starfrost.UL5.VersionUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Starfrost.UL5.StringUtilities
{
    /// <summary>
    /// String extension methods. 
    /// </summary>
    public static class StringUtil
    {
        public static string ToStringEmerald(this Point XPoint)
        {
            return $"{XPoint.X},{XPoint.Y}";
        }

        /// <summary>
        /// Split a string into X/Y positions. Static class that can be used from any class.
        /// </summary>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static Point SplitXY(this String SplitString)
        {
            try
            {
                string[] Split = SplitString.Split(',');

                if (Split.Length != 2)
                {
                    Console.WriteLine("Error converting string to position - must be 2 positions supplied");
                    return new Point(-1, -1);
                }

                // Convert the string components to two double-precision floats
                double X = Convert.ToDouble(Split[0]);
                double Y = Convert.ToDouble(Split[1]);

                // Create a new coordinatepoint
                Point XY = new Point(X, Y);
                return XY;
            }
            catch (FormatException err) // essentially this means that an overflow number was
            {
                Console.WriteLine($"Error converting string to position - invalid position\n\n{err}");

                //Todo: Generic type parameter based (for proving skills) user input validation
                return new Point(-1, -1);
            }
        }

        // From Emerald Game Engine
        // © 2020 Connor Hyde.
        public static Color SplitRGB(this String SplitString)
        {
            try
            {
                // Split the string by comma
                string[] Split = SplitString.Split(',');

                // RGB has three components - error out if we have less than three
                if (Split.Length != 3) MessageBox.Show("Error converting string to RGB colour - must be 2 positions supplied", "Emerald Game Engine Error 40", MessageBoxButton.OK, MessageBoxImage.Error);

                Color RGB = new Color();

                // For the track maker we don't need to set alpha. For free! we may need to
                RGB.A = 0xFF;

                // Convert to RGB
                RGB.R = Convert.ToByte(Split[0]);
                RGB.G = Convert.ToByte(Split[1]);
                RGB.B = Convert.ToByte(Split[2]);

                // Return our generated colour.
                return RGB;


            }
            catch (FormatException err)
            {
                MessageBox.Show($"Error converting string to position - invalid position\n\n{err}", "Emerald Game Engine Error 41", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Color { A = 0, R = 0, B = 0, G = 0 };
            }

        }


        // From Emerald Game Engine
        // © 2020 Connor Hyde.
        public static Color SplitARGB(this String SplitString)
        {
            try
            {
                // Split the string by comma
                string[] Split = SplitString.Split(',');

                // RGB has three components - error out if we have less than three
                if (Split.Length != 4) MessageBox.Show("Error converting string to RGB colour - must be 2 positions supplied", "Emerald Game Engine Error 40", MessageBoxButton.OK, MessageBoxImage.Error);

                Color ARGB = new Color();

                ARGB.A = Convert.ToByte(Split[0]);

                // Convert to RGB
                ARGB.R = Convert.ToByte(Split[1]);
                ARGB.G = Convert.ToByte(Split[2]);
                ARGB.B = Convert.ToByte(Split[3]);

                // Return our generated colour.
                return ARGB;


            }
            catch (FormatException err)
            {
                MessageBox.Show($"Error converting string to position - invalid position\n\n{err}", "Emerald Game Engine Error 41", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Color { A = 0, R = 0, G = 0, B = 0 };
            }


        }

        /// <summary>
        /// Splits a string into an Emerald GameDLL version. Modified 2020-04-30 for scriptdomains.
        /// </summary>
        /// <param name="SplitString">The string to split.</param>
        /// <returns></returns>
        public static List<int> SplitVersion(this String SplitString)
        {
            try
            {
                string[] _1 = SplitString.Split('.');

                // If we don't have 3 versions then error out
                if (_1.Length != 4)
                {
                    MessageBox.Show($"Error converting string to version - must be 4 version components supplied", "Emerald Game Engine Error 42", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                List<int> Version = new List<int>();

                // Build the version

                foreach (string _2 in _1)
                {
                    Version.Add(Convert.ToInt32(_2));
                }

                return Version;
            }
            // Error Condition: Attempted to convert an invalid portion of a string. 
            catch (FormatException err)
            {
                MessageBox.Show($"Error converting string to version - invalid version information\n\n{err}", "Emerald Game Engine Error 56", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Convert from string to version. 
        /// </summary>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static EVersion ToVersion(this String SplitString)
        {
            try
            {
                string[] _1 = SplitString.Split('.');

                // If we don't have 3 versions then error out
                if (_1.Length != 4)
                {
                    MessageBox.Show($"Error converting string to version - must be 4 version components supplied", "Emerald Game Engine Error 60", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                EVersion Version = new EVersion();

                // set the version information
                Version.Major = Convert.ToInt32(_1[0]);
                Version.Minor = Convert.ToInt32(_1[1]);
                Version.Build = Convert.ToInt32(_1[2]);
                Version.Revision = Convert.ToInt32(_1[3]);

                return Version;

            }
            catch (FormatException err)
            {
                MessageBox.Show($"Error converting string to version - invalid version component supplied!\n\n{err}", "Emerald Game Engine Error 61", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }


        public static List<string> InnerXml_Parse(this String InnerXml)
        {
            // xml preprocessing
            string[] PreSplit = InnerXml.Split('<');

            List<string> FinalList = new List<string>();

            // Strip it to the name

            foreach (string SplitV1 in PreSplit)
            {
                // Skip entry strings
                if (SplitV1 == "") continue;

                string[] SplitGreaterThan = SplitV1.Split('>');

                // Split each node into its respective value
                foreach (string XMLNodeOrValue in SplitGreaterThan)
                {
                    // remove remaining XML-related characters (Version 515)
                    string XMLNodeOrValuePost = XMLNodeOrValue.Replace("/", "");
                    if (XMLNodeOrValuePost == "") continue; // skip the strings that are not like the other 

                    // only add once
                    if (!FinalList.Contains(XMLNodeOrValuePost)) FinalList.Add(XMLNodeOrValuePost);

                }
            }

            return FinalList;
        }

        public static double RoundNearest(double x, double amount)
        {
            return Math.Round((x * amount) / amount);
        }

        public static Color ConvertWinformsToWpfColour(System.Drawing.Color XColour)
        {
            return Color.FromArgb(XColour.A, XColour.R, XColour.G, XColour.B);
        }

        public static System.Drawing.Color ConvertWpfToWinformsColour(Color XColour)
        {
            return System.Drawing.Color.FromArgb(XColour.A, XColour.R, XColour.G, XColour.B);
        }

        public static string ConvertArrayToString(this string[] String, bool UseNewline, LineEnding LE) // UseNewline and LineEnding added on conversion to UL5
        {
            StringBuilder SB = new StringBuilder();

            foreach (string StringComponent in String)
            {

                if (UseNewline)
                {
                    SB.Append(StringComponent); 
                    continue;
                }
                else
                {
                    switch (LE)
                    {
                        case LineEnding.Windows:
                            SB.Append($"{StringComponent}\r\n");
                            continue;
                        case LineEnding.Unix:
                            SB.Append($"{StringComponent}\n");
                            continue;
                    }
                }


            }

            return SB.ToString();
        }

        public static int GetMonthsBetweenTwoDates(DateTime Initial, DateTime EndDate) // DO NOT DO THE ABSOLUTE!
        {
            return 12 * (Initial.Year - EndDate.Year) - Initial.Month + EndDate.Month;
        }


        public static string PadZero(int X, int Zeros = 1)
        {
            // we don't do this for >10
            if (X > 9) return X.ToString();

            StringBuilder SB = new StringBuilder();

            for (int i = 0; i < Zeros; i++) //AAA
            {
                SB.Append("0");
            }

            SB.Append(X.ToString());

            return SB.ToString();
        }

        public static bool ContainsCaseInsensitive(this string Text, string Value, StringComparison SC = StringComparison.CurrentCultureIgnoreCase)
        {
            return Text.IndexOf(Value, SC) >= 0;
        }

        /// <summary>
        /// Convert HT- / XA- / XML formatted string to plaintext - replaces the most common mnemonics
        /// </summary>
        /// <param name="HTXAMLFormattedString"></param>
        /// <returns></returns>
        public static string Xaml2Cs(this string HTXAMLFormattedString)
        {
            string Fnl = HTXAMLFormattedString;

            if (Fnl.Contains("&amp;"))
            {
                Fnl = Fnl.Replace("&amp;", "&"); 
            }

            if (Fnl.Contains("&lt;")) Fnl = Fnl.Replace("&lt;", ">");
            if (Fnl.Contains("&gt;")) Fnl = Fnl.Replace("&gt;", ">");
            if (Fnl.Contains("&nbsp;")) Fnl = Fnl.Replace("&nbsp;", 0xA0.ToString());

            return Fnl; 

        }

    }


    /// <summary>
    /// I know LINQ has this, but I'm experimenting with generic type parameters.
    /// 
    /// Imported from ediff0-0.7
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListUtil<T>
    {
        public static List<T> ToList(Array Arr)
        {
            List<T> List0 = new List<T>();

            foreach (object Obj in Arr)
            {
                if (Obj is T)
                {
                    List0.Add((T)Obj);
                }
            }

            return List0;
        }

        // where t, y : object
        public static List<T> FromGenericList(List<object> OldList) 
        {
            List<T> NewList = new List<T>();

            foreach (object Obj in OldList)
            {
                T ObjNT = (T)Obj;

                Debug.Assert(ObjNT is T); 

                NewList.Add(ObjNT);
            }

            return NewList; 
        }
    }
}

