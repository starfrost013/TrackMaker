using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    /// <summary>
    /// The cardinal directions.
    /// </summary>
    public enum CardinalDirection { N, S, W, E }

    /// <summary>
    /// Latitude / longitude coordinate. 
    /// </summary>
    public class Coordinate
    {
        public Point Coordinates { get; set; }
        public List<CardinalDirection> Directions { get; set; }

        // Dano-style API, probably rewritten in dano to be better
        public static Coordinate FromString(string Str)
        {
            try
            {
                Coordinate Coord = new Coordinate();

                // Split by the semicolon
                string[] _ = Str.Split(' ');

                if (_.Length != 2)
                {
                    MessageBox.Show($"An error has occurred.\nError converting from string to coordinate.\nError CSC4.", "Fatal Error - Now Exiting", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown(74);
                }

                string[] _2 = _[0].Split(';');
                string[] _3 = _[1].Split(';');

                // concanetate into a format we can understand
                string _4 = $"{_2[0]},{_3[0]}";

                Coord.Coordinates = _4.SplitXY();

                // aaaa
                Coord.Directions = new List<CardinalDirection>();

                // Add stuff to the direction list
                Coord.Directions.Add((CardinalDirection)Enum.Parse(typeof(CardinalDirection), _2[1]));
                Coord.Directions.Add((CardinalDirection)Enum.Parse(typeof(CardinalDirection), _3[1]));

                return Coord;
            }
            catch (ArgumentException err)
            {
                MessageBox.Show($"An error has occurred.\nError converting from string to coordinate.\nError CSC1.\n\n{err}", "Fatal Error - Now Exiting", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(71);
                return null;
            }
            catch (OverflowException err)
            {
                MessageBox.Show($"An error has occurred.\nError converting from string to coordinate. A value was too small or large.\nError CSC2.\n\n{err}", "Fatal Error - Now Exiting", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(72);
                return null;
            }

        }

        /// <summary>
        /// Helper function to concanetate two split coordinates before throwing them into the parser. (Wish this was more compact)
        /// </summary>
        /// <param name="Str1">The X position of the coordinate.</param>
        /// <param name="Str2">The Y position of the coordinate.</param>
        /// <returns></returns>
        public static Coordinate FromSplitCoordinate(string Str1, string Str2)
        {
            return FromString($"{Str1},{Str2}");
        }

    }
}
