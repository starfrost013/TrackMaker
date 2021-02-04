using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TrackMaker.Core
{
    public class RelativePositionConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (!(Value is Point) || TargetType != typeof(RelativePosition))
            {
                Error.Throw("Error!", "Fatal Error: Attempted to convert a non-Point class to RelativePosition!", ErrorSeverity.FatalError, 417);
            }
            else
            {
                Point Vz = (Point)Value;

                double RpX = Vz.X / VolatileApplicationSettings.WindowSize.X;
                double RpY = Vz.Y / VolatileApplicationSettings.WindowSize.Y;

                RelativePosition Rp = new RelativePosition(RpX, RpY);
                return Rp;
            }

            return null; // pre-v3
            
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (!(Value is RelativePosition) || TargetType != typeof(Point))
            {
                Error.Throw("Error!", "Fatal error: Attempted to call RelativePositionConverter.ConvertBack() with parameter 0 (Value) not of type RelativePosition!", ErrorSeverity.Error, 418);
            }
            else
            {
                RelativePosition Rp = (RelativePosition)Value;

                double Px = VolatileApplicationSettings.WindowSize.X * Rp.X;
                double Py = VolatileApplicationSettings.WindowSize.Y * Rp.Y;

                Point P = new Point(Px, Py);
                return P;

            }

            return null; //pre-v3
        }
    }
}
