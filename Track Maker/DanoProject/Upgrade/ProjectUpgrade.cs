using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Track_Maker
{
    /// <summary>
    /// Make non-static?
    /// </summary>
    public static class ProjectUpgrader
    {

        /// <summary>
        /// Upgrade a Project from V1 to V2.x format.
        /// </summary>
        /// <param name="V1FileName"></param>
        /// <returns></returns>
        public static bool V1ToV2Upgrade(string V1FileName)
        {
            // To complete  2020-11-08

            XmlDocument XMLD = new XmlDocument();
            XMLD.Load(V1FileName); 

            // probably a terrible idea code
            foreach (XmlNode XNode in XMLD)
            {
                if (XNode.Name == "FormatVersionMajor" || XNode.Name == "FormatVersionMinor")
                {
                    Error.Throw("Error", "Attempted to upgrade Version 2.x file; this is not a version 1.x format file!", ErrorSeverity.Error, 193);
                    return true; 
                }


            }
            // end probably a terrible idea code
        }
    }
}
