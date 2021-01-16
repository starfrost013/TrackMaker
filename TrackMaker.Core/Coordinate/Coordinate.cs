using TrackMaker.Util.StringUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackMaker.Core
{
    /// <summary>
    /// Latitude / longitude coordinate. 
    /// </summary>
    public class Coordinate
    {
        public Point Coordinates { get; set; }
        public List<CardinalDirection> Directions { get; set; }

        public Coordinate()
        {
            Directions = new List<CardinalDirection>();
        }

        /// <summary>
        /// Convert a string to a coordinate.
        /// </summary>
        /// <param name="Str">The string you wish to convert to a coordinate.</param>
        /// <param name="CoordinateFormat"></param>
        /// <returns></returns>
        public static Coordinate FromString(string Str, CoordinateFormat CoordinateFormat)
        {
            try
            {
                switch (CoordinateFormat)
                {
                    case CoordinateFormat.TrackMaker:
                        return FromString_TrackMaker(Str);
                    case CoordinateFormat.ATCF:
                        return FromString_ATCF(Str);
                    case CoordinateFormat.HURDAT2:
                        return FromString_ATCF(Str, CoordinateFormat.HURDAT2);
                }
            }

            catch (ArgumentException err)
            {
#if DEBUG              
                Error.Throw("Error", $"An error has occurred.\nError converting from string to coordinate.\n\n{err}", ErrorSeverity.Error, 71);
#else
                Error.Throw("Error", "An error has occurred.\nError converting from string to coordinate.", ErrorSeverity.Error, 71);
#endif
                return null;
            }
            catch (OverflowException err)
            {
#if DEBUG
                Error.Throw("Error", $"An error has occurred.\nError converting from string to coordinate.\nA value was too small or large.\n\n{err}", ErrorSeverity.Error, 72);
#else
                Error.Throw("Error", "An error has occurred.\nError converting from string to coordinate. A value was too small or large.", ErrorSeverity.Error, 72);
#endif

                return null;
            }
            // should not occur
            return null; 
        }

        // Dano-style API, probably rewritten in dano to be better
        private static Coordinate FromString_TrackMaker(string Str)
        {
            Coordinate Coord = new Coordinate();

            // Split by the semicolon
            string[] _ = Str.Split(' ');

            if (_.Length != 2)
            {
                Error.Throw("Error", "An error has occurred.\nError converting from string to coordinate.", ErrorSeverity.Error, 74);
                return null; // will never run
            }

            string[] _2 = _[0].Split(';');
            string[] _3 = _[1].Split(';');

            // concanetate into a format we can understand
            // placing $ before a string in C# signifies to the compiler that this string is an interpolated string = the content between {} are variables or properties or similar
            string _4 = $"{_2[0]},{_3[0]}";


            Coord.Coordinates = _4.SplitXY(true);

            // aaaa
            Coord.Directions = new List<CardinalDirection>();

            // Add stuff to the direction list
            Coord.Directions.Add((CardinalDirection)Enum.Parse(typeof(CardinalDirection), _2[1]));
            Coord.Directions.Add((CardinalDirection)Enum.Parse(typeof(CardinalDirection), _3[1]));

            return Coord;

        }
        
        /// <summary>
        /// Merge
        /// </summary>
        /// <param name="AtcfString"></param>
        /// <returns></returns>
        public static Coordinate FromString_ATCF(string AtcfString, CoordinateFormat AF = CoordinateFormat.ATCF)
        {

            Coordinate Coord = new Coordinate();

            // convert to "real" format.
            string[] CoordinateComponents = AtcfString.Split(',');

            if (CoordinateComponents.Length > 2 || CoordinateComponents.Length == 0)
            {
                Error.Throw("Error!", "Cannot convert an invalid coordinate!", ErrorSeverity.Error, 243);
                return null; // not successful - PRE 3.0
            }

            string CoordinateComponents1 = CoordinateComponents[0];
            string CoordinateComponents2 = CoordinateComponents[1];

            string PreNumericalComponent1 = CoordinateComponents1.Substring(0, CoordinateComponents1.Length - 1);
            string PreNumericalComponent2 = CoordinateComponents2.Substring(0, CoordinateComponents2.Length - 1);

            string CardinalDirection1 = CoordinateComponents1.Substring(CoordinateComponents1.Length - 1);
            string CardinalDirection2 = CoordinateComponents2.Substring(CoordinateComponents2.Length - 1);

            double Coord1 = Convert.ToDouble(PreNumericalComponent1);
            double Coord2 = Convert.ToDouble(PreNumericalComponent2);

            // ATCF format does not include decimal points and uses 1 d.p. of precision...

            if (AF == CoordinateFormat.ATCF)
            {
                Coord1 /= 10.0;
                Coord2 /= 10.0;
            }


            CardinalDirection CD1 = (CardinalDirection)Enum.Parse(typeof(CardinalDirection), CardinalDirection1);
            CardinalDirection CD2 = (CardinalDirection)Enum.Parse(typeof(CardinalDirection), CardinalDirection2);

            // we can't even use the standard methods because ATCF has to be so fucking special 
            Coord.Coordinates = new Point(Coord2, Coord1);

            Coord.Directions.Add(CD2);
            Coord.Directions.Add(CD1);

            return Coord;
        }
        /// <summary>
        /// Helper function to concanetate two split coordinates before throwing them into the parser. (Wish this was more compact)
        /// </summary>
        /// <param name="Str1">The X position of the coordinate.</param>
        /// <param name="Str2">The Y position of the coordinate.</param>
        /// <returns></returns>
        public static Coordinate FromSplitCoordinate(string Str1, string Str2, CoordinateFormat CF = CoordinateFormat.ATCF)
        {
            return FromString($"{Str1},{Str2}", CF);
        }

    }
}
