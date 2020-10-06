using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    /// <summary>
    /// A vertex (area where lines meet, points in a shape if you're dumb) position that is converted to a WPF Point - we may need to store additional stuff in here later so I am putting this as part of a wrapper class.
    /// 
    /// Part of a Shape
    /// </summary>
    public class VertexPoint
    {
        public Point Position { get; set; }
    }
}
