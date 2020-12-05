using Microsoft.Win32;
using Starfrost.UL5.ScaleUtilities; 
using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Track_Maker
{
    public class ExportImage : IImageExportFormat
    {
        public bool DisplayPreview { get; set; }
        public bool DisplayQualityControl { get; set; }
        internal MainWindow Xwindow { get; set; }
        public string Name { get; set; }
        public ImageQuality Quality { get; set; }

        public ExportImage()
        {
            Name = "Image";
            Xwindow = (MainWindow)Application.Current.MainWindow; // convert
            DisplayQualityControl = true;
            GlobalStateP.CurrentExportFormatName = GetType().ToString();
        }

        public void GeneratePreview(Canvas XCanvas) // test
        {
            Point XPoint = new Point(Utilities.RoundNearest(8 * (XCanvas.Width / Xwindow.Width) / 1.5, 4), Utilities.RoundNearest(8 * (XCanvas.Height / Xwindow.Height) / 1.5, 4));
            Xwindow.RenderContent(XCanvas, XPoint, Xwindow.CurrentProject.SelectedBasin.GetFlatListOfStorms());
        }

        public string GetName()
        {
            return Name;
        }

        public Project Import()
        {
            throw new NotImplementedException();
        }
        
        public bool Export(Project Project)
        {

         
            // DO CANVAS
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Title = "Save image";
                SFD.Filter = "PNG images|*.png";
                SFD.ShowDialog();

                //utilfunc v2
                if (File.Exists(SFD.FileName))
                {
                    File.Delete(SFD.FileName);
                    FileStream FS = File.Create(SFD.FileName);
                    FS.Close(); 
                }

                if (SFD.FileName == "")
                {
                    return true;
                }
                else
                {
                    // Create a new canvas and set its background. This is what we are going to be rendering to. 
                    ExportCore(Project, SFD.FileName);
                    return true; 
                }
            }
            catch (ArgumentException err)
            {
                Error.Throw("Error!", $"Unknown export error: An ArgumentException error has occurred.\n\n{err}.", ErrorSeverity.Error, 210);
                return false;
            }
            catch (FileNotFoundException err)
            {
                Error.Throw("Error!", $"Export error: File creation failed.\n\n{err}.", ErrorSeverity.Error, 211);
                return false;
            }
            catch (DirectoryNotFoundException err)
            {
                Error.Throw("Error!", $"Export error: Directory creation failed.\n\n{err}.", ErrorSeverity.Error, 212);
                return false;
            }
            catch (UnauthorizedAccessException err)
            {
                Error.Throw("Error!", $"Export error: The OS denied access to the file.\n\n{err}.", ErrorSeverity.Error, 213);
                return false;
            }
            catch (InvalidOperationException err)
            {
                Error.Throw("Error!", $"Export error: Invalid operation.\n\n{err}.", ErrorSeverity.Error, 214);
                return false;
            }
            catch (PathTooLongException err)
            {
                Error.Throw("Error!", $"Export error: A path was selected for the file that is longer than MAX_PATH, or 260 characters. Please use a shorter path to the file - try renaming folders so that they have shorter names.\n\n{err}.", ErrorSeverity.Error, 215);
                return false;
            }
        }

        public void SetImageQuality(ImageQuality ImageQuality) => Quality = ImageQuality;

        public bool ExportCore(Project Project, string FileName)
        {
            // Create a new canvas and set its background. This is what we are going to be rendering to. 
            Canvas TempCanvas = new Canvas();

            MainWindow Xwindow = (MainWindow)Application.Current.MainWindow;

            BitmapImage Bitmap = new BitmapImage();
            Bitmap.BeginInit();
            Bitmap.UriSource = new Uri(Project.SelectedBasin.ImagePath, UriKind.RelativeOrAbsolute);
            Bitmap.EndInit();
            // dumb hack (build 558)


            TempCanvas.Background = new ImageBrush(Bitmap);

            // should this still be an eXtension method? it doesn't manipulate the image object itself anymore so...
            Point CanvasSize = ScaleUtilities.ScaleToQuality(Bitmap, Quality);

            TempCanvas.Width = CanvasSize.X;
            TempCanvas.Height = CanvasSize.Y;

            Project CurrentProject = Xwindow.CurrentProject;

            CurrentProject.SelectedBasin.RecalculateNodePositions(Direction.Larger, new Point(Xwindow.Width, Xwindow.Height), new Point(TempCanvas.Width, TempCanvas.Height));

            //New scaling for picking up dev again - 2020-05-08 23:04
            //Remove storm selection functionality, replace with layer selection functionality - 2020-09-12 17:24
            //v462 - 2020-09-26 00:00
            Xwindow.RenderContent(TempCanvas, new Point(Utilities.RoundNearest(8 * (TempCanvas.Width / Xwindow.Width) / 1.5, 8), Utilities.RoundNearest(8 * (TempCanvas.Height / Xwindow.Height) / 1.5, 8)), Project.SelectedBasin.GetFlatListOfStorms());

            // Recalculate node positions on the currentbasin so they actually show up properly.

            TempCanvas.UpdateLayout();

            // create a new rendertargetbitmap and render to it

            RenderTargetBitmap RenderTarget = new RenderTargetBitmap((int)TempCanvas.Width, (int)TempCanvas.Height, 96.0, 96.0, PixelFormats.Default);

            // force WPF to render it because optimization is not always a good idea

            Viewbox ViewBox = new Viewbox();
            ViewBox.Child = TempCanvas;
            ViewBox.Measure(new Size(TempCanvas.Width, TempCanvas.Height));
            ViewBox.Arrange(new Rect(new Point(0, 0), new Point(TempCanvas.Width, TempCanvas.Height)));
            ViewBox.UpdateLayout();

            RenderTarget.Render(TempCanvas);

            // create a new PNG encoder and memory stream

            BitmapEncoder PNGEncoder = new PngBitmapEncoder();
            
            PNGEncoder.Frames.Add(BitmapFrame.Create(RenderTarget));

            MemoryStream TempCanvasms_ = new MemoryStream();

            // save the thing

            PNGEncoder.Save(TempCanvasms_);
            File.WriteAllBytes(FileName, TempCanvasms_.ToArray());

            // clean up by restoring the basin
            CurrentProject.SelectedBasin.RecalculateNodePositions(Direction.Smaller, new Point(Xwindow.Width, Xwindow.Height), new Point(TempCanvas.Width, TempCanvas.Height));

            // on success
            GlobalStateP.SetCurrentOpenFile(FileName);

            return true; // success
        }

        
    }
}
