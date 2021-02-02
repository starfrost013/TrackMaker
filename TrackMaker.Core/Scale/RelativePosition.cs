using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace TrackMaker.Core
{

    /// <summary>
    /// 2021-02-02      v2.1.706
    /// 
    /// RelativePosition: RelativePosition for the purposes of dynamic scaling of UI elements and nodes.
    /// </summary>
    public class RelativePosition
    {
        private double _x { get; set; }
        public double X { get 
            {
                return _x; 
            }
            set
            {
                if (value < 0 || value > 1)
                {
                    Error.Throw("Error!", "Attempted to set invalid RelativePosition!", ErrorSeverity.Error, 415);
                }
                else
                {
                    _x = value; 
                }

                
            }

        }

        private double _y { get; set; }
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value < 0 || value > 1)
                {
                    Error.Throw("Error!", "Attempted to set invalid RelativePosition!", ErrorSeverity.Error, 415);
                }
                else
                {
                    _y = value;
                }


            }


        }
    }
}
