using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    public enum ErrorSeverity { Message, Warning, Error, FatalError }
    public class Error
    {
        public static void Throw(string Caption, string Text, ErrorSeverity Severity, int ID = 0)
        {

            switch (Severity)
            {
                case ErrorSeverity.Message:
                    Logging.Log($"Message: {Text}");
                    MessageBox.Show(Caption, Text, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                case ErrorSeverity.Warning:
                    Logging.Log($"Warning ({ID}): {Text}");
                    MessageBox.Show($"Warning: {Caption}", Text, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                case ErrorSeverity.Error:
                    Logging.Log($"Error! ({ID}): {Text}");
                    MessageBox.Show($"Error #{ID}: {Caption}", Text, MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
