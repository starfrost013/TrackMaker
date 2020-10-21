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
                return new Color {A = 0, R = 0, B = 0, G = 0};
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

        public static bool ContainsCaseInsensitive(this string Text, string Value, StringComparison SC = StringComparison.CurrentCultureIgnoreCase)
        {
            return Text.IndexOf(Value, SC) >= 0; 
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
    }
}
