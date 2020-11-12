using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dano.AdvisoryGenerator
{
    partial class AdvMainWindow
    {
        private static double RoundNearest(double raw, double n)
        {
            return (Math.Round(raw / n)) * n;
        }
    }
}
