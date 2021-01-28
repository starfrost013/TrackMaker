using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input; 

namespace TrackMaker.Core
{ 

    /// <summary>
    /// 2021-01-27     Iris v701
    /// 
    /// DynaHotkey 
    /// 
    /// Iris dynamic hotkey class; allows dynamic triggering of hotkeys for quick category switching
    /// </summary>
    public class DynaHotkey
    {
        /// <summary>
        /// The name of this DynaHotkey.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The key that is required to be pressed for this DynaHotkey to be triggered
        /// </summary>
        public List<Key> Keys { get; set; }

        public DynaHotkey(string KeyName, List<Key> KeyList = null)
        {
            Name = KeyName; 

            if (Keys == null)
            {
                Keys = new List<Key>();
                return;
            }
            else
            {
                Keys = KeyList;
                return; 

            }
        }

        public void AddKey(Key Key) => Keys.Add(Key);
        public void RemoveKey(Key Key) => Keys.Remove(Key);
    }
}
