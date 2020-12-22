using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Starfrost.UL5.WpfUtil
{
    public static class TransformUtil<T> where T : Transform
    {
        public static T FindTransformWithClass(List<Transform> TransformList)
        {
            foreach (Transform Tx in TransformList)
            {
                if (Tx is T)
                {
                    return (T)Tx;
                }
            }

            return null; // return null if not valid
        }
    }
}
