using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfrost.UL5.MathUtil
{
    public static class MathUtil
    {
        public static double RoundNearest(double x, double amount) => Math.Round((x * amount) / amount);
    }
}
