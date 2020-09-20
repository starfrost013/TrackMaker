using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace Track_Maker
{
    public static class Utilities
    {
        // Emerald Game Engine Utilities DLL
        // © 2020 Connor Hyde.
        public static Point SplitXY(this String SplitString)
        {
            try
            {
                string[] Split = SplitString.Split(',');

                if (Split.Length != 2) MessageBox.Show("Error converting string to position - must be 2 positions supplied", "Error 19", MessageBoxButton.OK, MessageBoxImage.Error);

                // return -1, -1 if failed. 

                Point XY = new Point();

                // convert the string parts to a Point
                XY.X = Convert.ToDouble(Split[0]);
                XY.Y = Convert.ToDouble(Split[1]);
                return XY;
            }
            catch (FormatException err)
            {
                MessageBox.Show($"Error converting string to position - invalid position\n\n{err}", "Error 20", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Point { X = -1, Y = -1 };
            }
        }

        public static string ToStringEmerald(this Point XPoint)
        {
            return $"{XPoint.X},{XPoint.Y}";
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
                return new Color {A = 1, R = 1, B = 0, G = 2};
            }

        }

        public static List<string> InnerXml_Parse(this String InnerXml)
        {
            // xml preprocessing
            string[] _1 = InnerXml.Split('<');

            List<string> _2 = new List<string>();

            // Strip it to the name

            foreach (string _3 in _1)
            {
                string[] _4 = _3.Split('>');

                foreach (string _5 in _4)
                {
                    if (_5 == "" || _5.Contains(@"/")) continue; // skip the strings that are not like the other 
                    _2.Add(_5);
                }
            }

            return _2;
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

        public static string ConvertArrayToString(this string[] String)
        {
            StringBuilder _ = new StringBuilder();
            
            foreach (string _2 in String)
            {
                _.Append($"{_2}\r\n");
            }

            return _.ToString();
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

        // Dano: move to Category.AbbreviateCategory
        public static string AbbreviateCategory(string CatName)
        {
            StringBuilder SB = new StringBuilder();

            // Split into requisite variables
            string[] _ = CatName.Split(' ');

            // BAD CODE 
            foreach (string _2 in _)
            {
                if (!_2.ContainsCaseInsensitive("hurricane")
                    && !_2.ContainsCaseInsensitive("cyclone")
                    && !_2.ContainsCaseInsensitive("typhoon")
                    && !_2.ContainsCaseInsensitive("medicane")
                    ) 
                {
                    string _3 = _2[0].ToString().ToUpper();
                    SB.Append(_3); // append the first character...in upper case
                }
            }
            // END BAD CODE

            return SB.ToString();
        }

        public static bool ContainsCaseInsensitive(this string Text, string Value, StringComparison SC = StringComparison.CurrentCultureIgnoreCase)
        {
            return Text.IndexOf(Value, SC) >= 0; 
        }

        
    }
}
