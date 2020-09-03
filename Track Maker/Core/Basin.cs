using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Track_Maker
{
    // Custom hemispheres? (2.0+)
    public enum BasinType { Track, Animation };
    public enum Hemisphere { North, South };

    public class Basin
    {
        public BitmapImage BasinImage { get; set; } // the basin image name
        public string BasinImagePath { get; set; } // the basin image path. we only load what we need.
        public Storm CurrentStorm { get; set; } // the currently selected Storm
        public string Name { get; set; } // the name of the basin
        public Coordinate CoordsLower { get; set; } // The lower point of the coords of this basin
        public Coordinate CoordsHigher { get; set; } // The highest point of the coords of this basin
        public List<Storm> Storms { get; set; } // list of Storms
        
        // New for Dano M1 and Priscilla
        public Point FocusPoint { get; set; } // The focus point of the camera inside this basin.
        public int ZoomLevel { get; set; } // The zoom level of the camera inside this basin.
        public int Year { get; set; } // THE Year
        public Hemisphere SeasonHemisphere { get; set; } // The hemisphere
        public int SeasonID { get; set; }
        public BasinType SeasonType { get; set; }

        // New for Priscilla.
        public List<Layer> Layers { get; set; } // new: list of layers

        public Basin()
        {
            Storms = new List<Storm>();
            Layers = new List<Layer>(); 
        }

        // Dano-style API (move to Project)
        
        public Coordinate FromNodePositionToCoordinate(Point NodePosition)
        {
            Coordinate Coord = new Coordinate();

            //Temp
            double X1 = 0;
            double X2 = 0;
            double Y1 = 0;
            double Y2 = 0;

            //Convert (coords lower/higher are alwa
            X1 = CoordsLower.Coordinates.X;
            Y1 = CoordsLower.Coordinates.Y;
            X2 = CoordsHigher.Coordinates.X;
            Y2 = CoordsHigher.Coordinates.Y;

            // Convert to negative

            foreach (CardinalDirection Cardir in CoordsLower.Directions)
            {
                switch (Cardir)
                {
                    case CardinalDirection.W:
                        X1 = -X1;
                        continue; 
                    case CardinalDirection.S:
                        Y1 = -Y1;
                        continue;
                }
            }

            foreach (CardinalDirection Cardir in CoordsHigher.Directions)
            {
                switch (Cardir)
                {
                    case CardinalDirection.W:
                        X2 = -X2;
                        continue;
                    case CardinalDirection.S:
                        Y2 = -Y2;
                        continue;
                }
            }

            // convert to 1. Get rid of this mainwindow shit asap in dano.
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow; 
            double _ = NodePosition.X / MnWindow.Width;
            double _2 = NodePosition.Y / MnWindow.Height;

            double FinalX = (X1 + (X2 - X1)) * _;
            double FinalY = (Y1 + (Y2 - Y1)) * _2;

            string _s = FinalX.ToString();
            string _s2 = FinalY.ToString();

            string[] _s3 = _s.Split('.');
            string[] _s5 = _s2.Split('.');

            if (_s3.Length != 0)
            {
                // Truncate to the first decimal point if there are decimal points
                string _s4 = _s3[1].Substring(0, 3 - _s3[0].Length);

                // Concanectate
                _s4 = $"{_s3[0]}{_s4}";

                // Convert back
                FinalX = Convert.ToDouble(_s4);
            }

            if (_s5.Length != 0)
            {
                // Truncate to the first decimal point if there are decimal points
                string _s6 = _s5[1].Substring(0, 1);

                // Concanectate
                _s6 = $"{_s5[0]}{_s6}";

                // Convert back
                FinalY = Convert.ToDouble(_s6);
            }

            Coord.Directions = new List<CardinalDirection>();

            if (FinalX < 0)
            {
                FinalX = -FinalX;
                Coord.Directions.Add(CardinalDirection.W);
            }
            else
            {
                Coord.Directions.Add(CardinalDirection.E);
            }

            if (FinalY < 0)
            {
                FinalY = -FinalY;
                Coord.Directions.Add(CardinalDirection.S);
            }
            else
            {
                Coord.Directions.Add(CardinalDirection.N);
            }

            Coord.Coordinates = new Point(FinalX, FinalY);

            return Coord; 
        }

        /// <summary>
        /// Adds a Storm.
        /// </summary>
        /// <returns></returns>
        public bool AddStorm(string Name, string Type, DateTime DateTime)
        {
            try
            {
                Storm Storm = new Storm();
                Storm.Id = Storms.Count; // this actually makes sense.
                Storm.Name = Name;

                Logging.Log($"Adding Storm with id {Storm.Id} and name {Storm.Name}");

                if (Storm.Name == "")
                {
                    Error.Throw("You must add a name!", "Track Maker", Error.ErrorSeverity.Warning, 2);
                    Storm = null;
                    return false;
                }

                // Check the date. [2020-05-13] - convert from nullable datetime to datetime?

                if (DateTime == null)
                {
                    Error.Throw("You must add a date and time!", "Error", Error.ErrorSeverity.Warning, 1);
                    return false;
                }

                Storm.FormationDate = DateTime; // WHY IS THIS NULLABLE I MEAN ITS HELPFUL BUT YES

                // Just starting from what we already had here.

                if (Storms.Count != 0)
                {
                    if (Storm.FormationDate < Storms[0].FormationDate)
                    {
                        MessageBox.Show("You can't have a Storm start earlier than the season!", "Error I1", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }

                if (Type == "Tropical cyclone")
                {
                    Storm.StormType = StormType.Tropical;
                }
                else if (Type == "Subtropical cyclone")
                {
                    Storm.StormType = StormType.Subtropical;
                }
                else if (Type == "Extratropical cyclone")
                {
                    Storm.StormType = StormType.Extratropical;
                }
                else if (Type == "Invest / PTC")
                {
                    Storm.StormType = StormType.InvestPTC;
                }
                else if (Type == "Polar low")
                {
                    Storm.StormType = StormType.PolarLow;
                }
                Logging.Log($"Set storm type to {Storm.StormType}");

                Logging.Log("Initializing node list...");
                Storm.NodeList = new List<Node>(); // initalize the mode list
                Logging.Log("Setting current Storm...");
                CurrentStorm = Storm;
                Logging.Log("Adding Storm to basin Storm list...");
                Storms.Add(Storm);
                Logging.Log("Done! Closing...");

                return true;
            }
            catch (FormatException)
            {
                Error.Throw("You must enter a valid date and time!", "Error", Error.ErrorSeverity.Warning, 3);
                return false;
            }
        }
    }
}
