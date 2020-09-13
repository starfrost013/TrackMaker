using Microsoft.Win32;
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
    public class ExportImage : IExportFormat
    {
        public bool AutoStart { get; set; }
        internal MainWindow Xwindow { get; set; }
        public string Name { get; set; }

        public ExportImage()
        {
            Name = "Image";
            Xwindow = (MainWindow)Application.Current.MainWindow; // convert
            AutoStart = true; // no more previews so.
        }

        public void GeneratePreview(Canvas XCanvas) // test
        {
            Point XPoint = new Point(Utilities.RoundNearest(8 * (XCanvas.Width / Xwindow.Width) / 1.5, 4), Utilities.RoundNearest(8 * (XCanvas.Height / Xwindow.Height) / 1.5, 4));
            Xwindow.RenderContent(XCanvas, XPoint, Xwindow.CurrentProject.SelectedBasin.Storms);
        }

        public string GetName()
        {
            return Name;
        }

        public Basin Import()
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
                MessageBox.Show($"Unknown export error: An ArgumentException error has occurred. [Error code: EI1]\n\n{err}.");
                return false;
            }
            catch (FileNotFoundException err)
            {
                MessageBox.Show($"Export error: File creation failed. [Error code: EI2]\n\n{err}.");
                return false;
            }
            catch (DirectoryNotFoundException err)
            {
                MessageBox.Show($"Export error: Directory creation failed (???). [Error code: EI3]\n\n{err}.");
                return false;
            }
            catch (UnauthorizedAccessException err)
            {
                MessageBox.Show($"Export error: The OS denied access to the file. [Error code: EI4]\n\n{err}.");
                return false;
            }
            catch (SecurityException err)
            {
                MessageBox.Show($"Export error: A security error has occurred. [Error code: EI5]\n\n{err}.");
                return false;
            }
            catch (InvalidOperationException err)
            {
                MessageBox.Show($"Export error: An invalid operation for the current program state has occurred. [Error code: EI6]\n\n{err}.");
                return false;
            }
            catch (PathTooLongException err)
            {
                MessageBox.Show($"Export error: A path was selected for the file that is longer than MAX_PATH, or 260 chars, because Windows sucks (although it doesn't, but 8.3 compatibility!!!!1). Please use a shorter path file - try renaming folders to be shorter, or something. [Error code: EI7]\n\n{err}.");
                return false;
            }
        }

        public bool ExportCore(Project Project, string FileName)
        {
            // Create a new canvas and set its background. This is what we are going to be rendering to. 
            Canvas _temp_ = new Canvas();

            MainWindow Xwindow = (MainWindow)Application.Current.MainWindow;

            BitmapImage _temp_bi_ = new BitmapImage();
            _temp_bi_.BeginInit();
            _temp_bi_.UriSource = new Uri(Project.SelectedBasin.BasinImagePath, UriKind.RelativeOrAbsolute);
            _temp_bi_.EndInit();

            _temp_.Background = new ImageBrush(_temp_bi_);
            _temp_.Width = _temp_bi_.PixelWidth;
            _temp_.Height = _temp_bi_.PixelHeight;

            Xwindow.CurrentProject.SelectedBasin.RecalculateNodePositions(Direction.Larger, new Point(Xwindow.Width, Xwindow.Height), new Point(_temp_.Width, _temp_.Height));

            //New scaling for picking up dev again - 2020-05-08 23:04
            //Remove storm selection functionality, replace with layer selection functionality - 2020-09-12 17:24
            Xwindow.RenderContent(_temp_, new Point(Utilities.RoundNearest(8 * (_temp_.Width / Xwindow.Width) / 1.5, 8), Utilities.RoundNearest(8 * (_temp_.Height / Xwindow.Height) / 1.5, 8)), Project.SelectedBasin.Storms);

            // Recalculate node positions on the currentbasin so they actually show up properly.

            _temp_.UpdateLayout();

            // create a new rendertargetbitmap and render to it

            RenderTargetBitmap _temp_rtb_ = new RenderTargetBitmap((int)_temp_.Width, (int)_temp_.Height, 96.0, 96.0, PixelFormats.Default);

            // force WPF to render it because optimization is not always a good idea

            Viewbox ViewBox = new Viewbox();
            ViewBox.Child = _temp_;
            ViewBox.Measure(new Size(_temp_.Width, _temp_.Height));
            ViewBox.Arrange(new Rect(new Point(0, 0), new Point(_temp_.Width, _temp_.Height)));
            ViewBox.UpdateLayout();

            _temp_rtb_.Render(_temp_);

            // create a new PNG encoder and memory stream

            BitmapEncoder _temp_be_ = new PngBitmapEncoder();
            _temp_be_.Frames.Add(BitmapFrame.Create(_temp_rtb_));

            MemoryStream _temp_ms_ = new MemoryStream();

            // save the thing

            _temp_be_.Save(_temp_ms_);
            File.WriteAllBytes(FileName, _temp_ms_.ToArray());

            // clean up by restoring the basin
            Xwindow.CurrentProject.SelectedBasin.RecalculateNodePositions(Direction.Smaller, new Point(Xwindow.Width, Xwindow.Height), new Point(_temp_.Width, _temp_.Height));

            return true; // success
        }
    }
}
