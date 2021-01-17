using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackMaker.UI
{
    // Hmm
    public enum TrackMaker.UIComponent { StartPage, NewProject, NewSeason, ProjectMain, Export2, Settings, AddNewStorm, AddTrackPoint, BasinSwitcher, CategoryManager, SeasonManager, PackInstaller, AnimationEditor }
    public class DanoEventArgs : EventArgs
    {
        public List<object> DanoParameters { get; set; }
        public bool LaunchUIComponentAfterDone { get; set; }
        public TrackMaker.UIComponent LaunchAfterDone { get; set; }

        public DanoEventArgs()
        {
            DanoParameters = new List<object>(); 
        }
    }
}
