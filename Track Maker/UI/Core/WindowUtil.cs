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
        internal void SetFullscreen()
        {
            switch (Fullscreen)
            {
                // MOVE THIS CODE
                case false: // if it's false, turn it on
                    RecalculateNodePositions();
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
                    RecalculateNodePositions();
                    Fullscreen = false;
                    return;
            }
        }

        /// <summary>
        /// When leaving or entering fullscreen mode, recalculate the position of each node so it doesn't end up in the wrong place.
        /// </summary>
        private void RecalculateNodePositions()
        {
            foreach (Storm StormtoRecalc in CurrentBasin.Storms)
            {
                foreach (Node NodetoRecalc in StormtoRecalc.NodeList)
                {
                    switch (Fullscreen)
                    {
                        case true:
                            NodetoRecalc.Position = new Point(NodetoRecalc.Position.X / (SystemParameters.PrimaryScreenWidth / Width), NodetoRecalc.Position.Y / (SystemParameters.PrimaryScreenHeight / Height));
                            continue;
                        case false:
                            NodetoRecalc.Position = new Point(NodetoRecalc.Position.X * (SystemParameters.PrimaryScreenWidth / Width), NodetoRecalc.Position.Y * (SystemParameters.PrimaryScreenHeight / Height));
                            continue;

                    }
                }
            }
        }

        public void RecalculateNodePositions(Direction RecalcDir, Point RecalcRes, Basin XBasin)
        {
            foreach (Storm StormtoRecalc in XBasin.Storms)
            {
                foreach (Node NodetoRecalc in StormtoRecalc.NodeList)
                {
                    switch (RecalcDir)
                    {
                        case Direction.Smaller:
                            // get it smaller
                            NodetoRecalc.Position = new Point(NodetoRecalc.Position.X / (RecalcRes.X / Width), NodetoRecalc.Position.Y / (RecalcRes.Y / Height));
                            continue;
                        case Direction.Larger:
                            // get it larger
                            NodetoRecalc.Position = new Point(NodetoRecalc.Position.X * (RecalcRes.X / Width), NodetoRecalc.Position.Y * (RecalcRes.Y / Height));
                            continue;

                    }
                }
            }

            return; 
        }
    }
}
