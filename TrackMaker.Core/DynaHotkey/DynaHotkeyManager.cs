using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrackMaker.Core
{
    public class DynaHotkeyManager
    {
        public static int DHAPI_Version_Major = 1;
        public static int DHAPI_Version_Minor = 5;
        public static int DHAPI_Version_Revision = 0;

        public List<DynaHotkey> Hotkeys { get; set; }

        public DynaHotkeyManager()
        {
            Hotkeys = new List<DynaHotkey>();
        }


        public void AddNewHotkey(string KeyName, List<Key> Keys = null)
        {
            DynaHotkey DK = new DynaHotkey(KeyName, Keys);
            Hotkeys.Add(DK);

        }

        /// <summary>
        /// Adds a new hotkey and optionally adds it to the global list.
        /// </summary>
        /// <param name="KeyName"></param>
        /// <param name="Keys">The keyboard keys that need to be pressed to load this key.</param>
        /// <param name="AddToGlobalList">Add to the global list. Set this to false if your hotkeys only have a specific use.</param>
        /// <returns></returns>
        public DynaHotkey AddNewHotkey(string KeyName, List<Key> Keys = null, bool AddToGlobalList = true)
        {
            DynaHotkey DK = new DynaHotkey(KeyName, Keys);
            if (AddToGlobalList) Hotkeys.Add(DK);
            return DK; 

        }

        /// <summary>
        /// For single-key DynaHotkeys, check for duplication.  
        /// </summary>
        /// <param name="Key"></param>
        public bool CheckIfKeyIsDuplicated(Key Key)
        {
            foreach (DynaHotkey Hotkey in Hotkeys)
            {
                if (Hotkey.Keys.Count == 1)
                {
                    if (Hotkey.Keys.Contains(Key)) return true; 
                } 
            }

            return false;
        }

        public void ClearAllHotkeys() => Hotkeys.Clear();
        public void DeleteHotkey(DynaHotkey DH) => Hotkeys.Remove(DH);

        public bool DeleteHotkeyWithName(string KeyName)
        {
            foreach (DynaHotkey Hotkey in Hotkeys)
            {
                if (Hotkey.Name == KeyName)
                {
                    Hotkeys.Remove(Hotkey);
                    return true; 
                }

            }

            return false;
        }

        public DynaHotkey GetHotkeyWithName(string KeyName)
        {
            foreach (DynaHotkey Hotkey in Hotkeys)
            {
                if (Hotkey.Name == KeyName) return Hotkey;
            }

            return null;
        }

        
    }
}
