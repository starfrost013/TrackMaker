using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Starfrost.UL5.WpfUtil
{
    public static class DataTemplateUtil
    {
        public static T FindVisualChild<T>(DependencyObject DO) where T : DependencyObject
        {
            int ChildCount = VisualTreeHelper.GetChildrenCount(DO);

            if (ChildCount == 0) return null; // there is no child

            for (int i = 0; i < ChildCount; i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DO, i);

                if (Child == null)
                {
                    return null;
                }
                else
                {
                    if (Child is T)
                    {
                        return (T)Child;
                    }
                    else
                    {
                        // find child of child
                        // just call it again; we don't need duplicate code 
                        FindVisualChild<T>(Child);
                    }
                }
            }

            return null; 
        }
    }
}
