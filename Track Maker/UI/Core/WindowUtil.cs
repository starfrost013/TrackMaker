using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; 

namespace Track_Maker
{
    public enum Direction { Smaller, Larger }
    public partial class MainWindow : Window
    {
        // Todo: Handle the Priscilla Sidebar
        internal void SetFullscreen()
        {
            switch (Fullscreen)
            {
                // MOVE THIS CODE
                case false: // if it's false, turn it on
                    CurrentProject.SelectedBasin.RecalculateNodePositions(Fullscreen, new Point(Width, Height));
                    Fullscreen = true;
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    MainMenu.Width = SystemParameters.PrimaryScreenWidth;
                    HurricaneBasin.Width = SystemParameters.PrimaryScreenWidth;
                    HurricaneBasin.Height = SystemParameters.PrimaryScreenHeight - MainMenu.Height; // MOVE THIS CODE 
                    return;
                case true: // if it's true, turn it off
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    MainMenu.Width = Width;
                    HurricaneBasin.Width = Width;
                    HurricaneBasin.Height = Height - MainMenu.Height; // MOVE THIS CODE
                    CurrentProject.SelectedBasin.RecalculateNodePositions(Fullscreen, new Point(Width, Height));
                    Fullscreen = false;
                    return;
            }
        }
    }
}
