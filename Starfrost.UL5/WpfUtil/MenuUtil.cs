using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; 

namespace TrackMaker.UI.WpfUtil
{
    public static class MenuUtil
    {
        /// <summary>
        /// Find a menu item with a specific header.
        /// 
        /// ENCASE ALL CALLS TO THIS FUNCTION IN TRY {} BLOCKS!
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="Header">The header to find</param>
        /// <returns></returns>
        public static MenuItem FindMenuItemWithHeader(this Menu Menu, string Header)
        {
            foreach (MenuItem Mn in Menu.Items)
            {
                if ((string)Mn.Header == Header) return Mn; 
            }

            return null; //pre-validation and result classses
        }

        /// <summary>
        /// Find a menu item with a specific name (code-behind)
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="Name">The name of the menu item to find.</param>
        /// <returns></returns>
        public static MenuItem FindMenuItemWithName(this Menu Menu, string Name)
        {
            foreach (MenuItem Mn in Menu.Items)
            {
                if (Mn.Name == Name) return Mn; 
            }

            return null; //pre-v3
        }
    }
}
