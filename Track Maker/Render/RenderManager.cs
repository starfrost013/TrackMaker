using Starfrost.UL5.WpfUtil;
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

        /// <summary>
        /// Renders the contents of the project.
        /// 
        /// Need to refactor to reduce redundant code use.
        /// 
        /// </summary>
        /// <param name="HurricaneBasin">The canvas containing the basin</param>
        /// <param name="DotSize">The dot size setting. (NEEDS TO BE CHANGED)</param>
        /// <param name="StormList">The storm list to rend</param>
        public void RenderContent(Canvas HurricaneBasin, Point DotSize, List<Storm> StormList = null)
        {
           
            // optimise by clearing it every time (this is optimisation per v0.x standards LMAO)
            HurricaneBasin.Children.Clear();

            foreach (Layer XLayer in CurrentProject.SelectedBasin.BuildListOfZOrderedLayers())
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

                    Render_DrawLines(XStorm, DotSize, HurricaneBasin);

                    foreach (Node XNode in XStorm.NodeList)
                    {
                        if (XNode.NodeType.PresetShape != StormShape.Custom)
                        {
                            switch (XNode.NodeType.PresetShape)
                            {
                                // tropical systems
                                case StormShape.Circle:
                                    Ellipse Ellipse = new Ellipse();

                                    if (XNode.NodeType.ForceSize) DotSize = XNode.NodeType.ForcedSize;

                                    Ellipse.Width = DotSize.X;
                                    Ellipse.Height = DotSize.Y;

                                    // get the colour (Dano: refactor)
                                    Ellipse.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));

                                    // Since there were many requests for an Invest/PTC storm type, here it is. 
                                    // need to fix redundant code - rc2 or iris

                                    if (XNode.NodeType.ForceColour)
                                    {
                                        // Invest / PTC uses [255,128,204,255] ARGB format
                                        Ellipse.Fill = new SolidColorBrush(XNode.NodeType.ForcedColour);
                                    }
                                    else
                                    {
                                        Ellipse.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));
                                    }

                                    // set the position
                                    Canvas.SetLeft(Ellipse, XNode.Position.X);
                                    Canvas.SetTop(Ellipse, XNode.Position.Y);

                                    HurricaneBasin.Children.Add(Ellipse);
                                    continue;
                                case StormShape.Square:

                                    Rectangle Rect = new Rectangle();

                                    if (XNode.NodeType.ForceSize) DotSize = XNode.NodeType.ForcedSize;

                                    Rect.Width = DotSize.X - DotSize.X / 8; // some people think the rects are too big (8/8 = 1) - this also means that all subtropical dots are 7/8ths the size of the other dots
                                    Rect.Height = DotSize.Y - DotSize.Y / 8;

                                    // get the colour
                                    Rect.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));

                                    // Since there were many requests for an Invest/PTC storm type, here it is. 
                                    // need to fix redundant code - rc2 or iris

                                    if (XNode.NodeType.ForceColour)
                                    {
                                        // Invest / PTC uses [255,128,204,255] ARGB format
                                        Rect.Fill = new SolidColorBrush(XNode.NodeType.ForcedColour);
                                    }
                                    else
                                    {
                                        Rect.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));
                                    }

                                    // set the position
                                    Canvas.SetLeft(Rect, XNode.Position.X);
                                    Canvas.SetTop(Rect, XNode.Position.Y);

                                    HurricaneBasin.Children.Add(Rect);
                                    continue;
                                case StormShape.Triangle:

                                    // draw the triangle
                                    Polygon Poly = new Polygon();

                                    if (XNode.NodeType.ForceSize) DotSize = XNode.NodeType.ForcedSize;

                                    Poly.Points.Add(new Point(0, DotSize.Y));
                                    Poly.Points.Add(new Point(DotSize.X / 2, 0));
                                    Poly.Points.Add(new Point(DotSize.X, DotSize.Y));

                                    // Since there were many requests for an Invest/PTC storm type, here it is. 
                                    if (XNode.NodeType.ForceColour)
                                    {
                                        // Invest / PTC uses [255,128,204,255] ARGB format
                                        Poly.Fill = new SolidColorBrush(XNode.NodeType.ForcedColour);
                                    }
                                    else
                                    {
                                        Poly.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));

                                    }

                                    Canvas.SetLeft(Poly, XNode.Position.X);
                                    Canvas.SetTop(Poly, XNode.Position.Y);

                                    HurricaneBasin.Children.Add(Poly);
                                    continue;
                            }
                        }
                        else // handle custom node handling
                        {

                            // We ignore forcesize for custom shapes 
                            Polygon Poly = new Polygon(); 

                            foreach (Point Pt in XNode.NodeType.Shape.VPoints.Points) // needs some refactoring
                            {
                                Poly.Points.Add(Pt); 
                            }

                            if (XNode.NodeType.Shape.IsFilled)
                            {
                                if (!XNode.NodeType.ForceColour)
                                {
                                    Poly.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));
                                }
                                else
                                {
                                    if (XNode.NodeType.ForcedColour != null)
                                    {
                                        Poly.Fill = new SolidColorBrush(XNode.NodeType.ForcedColour);
                                    }
                                    else
                                    {
                                        // invalid forcedcolour state
                                        Poly.Fill = new SolidColorBrush(RenderBasedOnNodeIntensity(XStorm, XNode));
                                    }

                                }
                            }

                            Canvas.SetLeft(Poly, XNode.Position.X);
                            Canvas.SetTop(Poly, XNode.Position.Y);

                            HurricaneBasin.Children.Add(Poly); 
                        }
                    }

                    if (Setting.DefaultVisibleTextNames) Render_DrawText(XStorm, HurricaneBasin);
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
        internal void Render_DrawLines(Storm XStorm, Point DotSize, Canvas HurricaneBasin)
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

        internal void Render_DrawText(Storm XStorm, Canvas HurricaneBasin)
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

        /// <summary>
        /// this is unoptimised lol
        /// 
        /// later on we'll introduce a check to see if the internaltransformgroup changed
        /// 
        /// Zooms and pans the HurricaneBasin. 
        /// </summary>
        internal void Render_ZoomAndPan()
        {
            if (InternalTransformGroup.Count == 0)
            {
                return; 
            }
            else
            {
                ScaleTransform FirstScaleST = TransformUtil<ScaleTransform>.FindTransformWithClass(InternalTransformGroup);
                TranslateTransform FirstTranslateTT = TransformUtil<TranslateTransform>.FindTransformWithClass(InternalTransformGroup);

                TransformGroup STX = new TransformGroup();
                STX.Children.Add(FirstScaleST);
                STX.Children.Add(FirstTranslateTT);

                HurricaneBasin.RenderTransform = this; 
            }


        }

    }
}
