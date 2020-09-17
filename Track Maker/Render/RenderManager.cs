using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Track_Maker
{
    public partial class MainWindow : Window
    {
        public void RenderContent(Canvas HurricaneBasin, Point DotSize, List<Storm> StormList = null)
        {
            // optimise by clearing it every time
            HurricaneBasin.Children.Clear();

            foreach (Layer XLayer in CurrentProject.SelectedBasin.Layers)
            {
                // render loop
                foreach (Storm XStorm in XLayer.AssociatedStorms)
                {
                    if (StormList != null)
                    {
                        if (!StormList.Contains(XStorm))
                        {
                            continue;
                        }
                    }

                    DrawLines(XStorm, DotSize, HurricaneBasin);

                    foreach (Node XNode in XStorm.NodeList)
                    {
                        switch (XNode.NodeType)
                        {
                            // tropical systems
                            case StormType.Tropical:
                                Ellipse Ellipse = new Ellipse();
                                Ellipse.Width = DotSize.X;
                                Ellipse.Height = DotSize.Y;

                                // get the colour (Dano: refactor)
                                Ellipse.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));

                                // set the position
                                Canvas.SetLeft(Ellipse, XNode.Position.X);
                                Canvas.SetTop(Ellipse, XNode.Position.Y);

                                HurricaneBasin.Children.Add(Ellipse);
                                continue;
                            case StormType.Subtropical:
                                Rectangle Rect = new Rectangle();
                                Rect.Width = DotSize.X - DotSize.X / 8; // some people think the rects are too big (8/8 = 1) - this also means that all subtropical dots are 7/8ths the size of the other dots
                                Rect.Height = DotSize.Y - DotSize.Y / 8;

                                // get the colour
                                Rect.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));

                                // set the position
                                Canvas.SetLeft(Rect, XNode.Position.X);
                                Canvas.SetTop(Rect, XNode.Position.Y);

                                HurricaneBasin.Children.Add(Rect);
                                continue;
                            case StormType.Extratropical:
                            case StormType.InvestPTC:
                            case StormType.PolarLow:
                                // draw the triangle
                                Polygon Poly = new Polygon();
                                Poly.Points.Add(new Point(0, 8));
                                Poly.Points.Add(new Point(4, 0));
                                Poly.Points.Add(new Point(8, 8));

                                // Since there were many requests for an Invest/PTC storm type, here it is. 
                                if (XNode.NodeType != StormType.InvestPTC)
                                {
                                    Poly.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));
                                }
                                else
                                {
                                    Poly.Fill = new SolidColorBrush(new Color { A = 255, R = 128, G = 204, B = 255 });
                                }

                                Canvas.SetLeft(Poly, XNode.Position.X);
                                Canvas.SetTop(Poly, XNode.Position.Y);

                                HurricaneBasin.Children.Add(Poly);
                                continue;
                        }


                    }
                    if (Setting.DefaultVisibleTextNames) DrawText(XStorm, HurricaneBasin);
                }
            }
            

            // get WPF to render it
            UpdateLayout(); 
        }

        public Color RenderBasedOnNodeIntensity(Storm XStorm, Node XNode)
        {
            // check the node intensity
            // category system

            // Moved to a function here
            Category _ = XStorm.GetNodeCategory(XNode, Catman.CurrentCategorySystem);

            if (_ == null)
            {
                return new Color { A = 128, R = 128, G = 128, B = 128 };
            }
            else
            {
                return _.Color;
            }

        }

        /// <summary>
        /// This draws the lines. INCREASING Y IS DOWN!
        /// </summary>
        internal void DrawLines(Storm XStorm, Point DotSize, Canvas HurricaneBasin)
        {
            for (int i = 0; i < XStorm.NodeList.Count - 1; i++)
            {
                Node a = XStorm.NodeList[i];
                Node b = XStorm.NodeList[i + 1];
                // draw a line for each storm

                Line XLine = new Line();
                XLine.StrokeThickness = Setting.LineSize;

                XLine.X1 = a.Position.X + DotSize.X / 2;
                XLine.X2 = b.Position.X + DotSize.X / 2; 
                XLine.Y1 = a.Position.Y + DotSize.Y / 2; 
                XLine.Y2 = b.Position.Y + DotSize.Y / 2; 

                XLine.Stroke = new SolidColorBrush(new Color { A = 255, R = 255, G = 255, B = 255 }); // white colour for lines

                // add the line
                HurricaneBasin.Children.Add(XLine);
            }
        }

        internal void DrawText(Storm XStorm, Canvas HurricaneBasin)
        {
            // we draw by the first node so there's nowhere to draw it if there are no nodes
            if (XStorm.NodeList.Count == 0) return;

            TextBlock txtblock = new TextBlock();
            txtblock.FontSize = 11;
            // text is the storm name
            txtblock.Text = XStorm.Name;
            // white text
            txtblock.Foreground = new SolidColorBrush(new Color { A = 255, R = 255, G = 255, B = 255 });

            Canvas.SetLeft(txtblock, XStorm.NodeList[0].Position.X - (5.5 * txtblock.Text.Length));
            Canvas.SetTop(txtblock, XStorm.NodeList[0].Position.Y + 15);

            HurricaneBasin.Children.Add(txtblock); 

        }
    }
}
