using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfrost.UL5.Core
{
    public class App
    {
        public string Name { get; set; }
    }

    public static class AppRegistration
    {
        /// <summary>
        /// Register a UL5 app. 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static App RegisterApp(string Name)
        {
            App AP = new App();
            AP.Name = Name;
            return AP;
        }
    }
}
