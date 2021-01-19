using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackMaker.Core
{
    public enum ErrorSeverity { Message, Warning, Error, FatalError }
    public class Error
    {
        public static void Throw(string Caption, string Text, ErrorSeverity Severity, int ID = 0)
        {

#if PRISCILLA
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            MnWindow.TickTimer.Stop(); 
#endif
            switch (Severity)
            {
                case ErrorSeverity.Message:
                    Logging.Log($"Message: {Text}");
                    MessageBox.Show(Text, Caption, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                case ErrorSeverity.Warning:
                    Logging.Log($"Warning ({ID}): {Text}");
                    MessageBox.Show($"Warning: {Text}", Caption, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                case ErrorSeverity.Error:
                    Logging.Log($"Error! ({ID}): {Text}");
                    MessageBox.Show($"Error #{ID}: {Text}", Caption, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                case ErrorSeverity.FatalError:
                    Logging.Log($"Fatal Error\n\n:Error ID:{ID}: Status: {Text}");
                    // Create an EUIH
                    ErrorUIHost EUIH = new ErrorUIHost(ID, Text);
                    EUIH.ShowDialog();

                    uint ECode;

                    // Get our nice fancy 0x<whatever>DEAD error code. 
                    try
                    {
                        ECode = 0xDEAD + (0x10000 * Convert.ToUInt32(ID));
                        //ECode = (id << 16) + 0xDEAD;
                    }
                    catch (OverflowException)
                    {
                        ECode = Convert.ToUInt32(ID);
                    }
                    catch (FormatException)
                    {
                        ECode = 0xDEADDEAD;
                    }

                    Application.Current.Shutdown(Convert.ToInt32(ECode));
                    Environment.Exit(Convert.ToInt32(ECode));

                    return; 

            }
            
        }

        /// <summary>
        /// Show a beta warning.
        /// </summary>
        public static void ShowBetaWarning() => MessageBox.Show("Information:\n\n" +
                "You are running a pre-release version of the Track Maker.\n\n"
                + "There may be issues, bugs, and impaired or inoperative functionality that would not exist in a final release.\n"
                + "These may impair functionality in a way that would not be expected in a final release. There is no guarantee of quality provided with this version of the software.\n"
                + "Please report any bugs that you discover at https://github.com/Cosmo224/TrackMaker. \n\n"
                + "Thank you for using and testing the Track Maker. Your feedback is vital to its future and success!", "Beta Software Notice", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
