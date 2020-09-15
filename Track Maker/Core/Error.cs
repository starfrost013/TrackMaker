using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    public class Error
    {
        public enum ErrorSeverity { Message, Warning, Error, FatalError, CatastrophicError}
        public static void Throw(string caption, string Text, ErrorSeverity Severity, int ID = 0)
        {

            switch (Severity)
            {
                case ErrorSeverity.Message:
                    MessageBox.Show(caption, Text, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                case ErrorSeverity.Warning:
                    MessageBox.Show($"Warning: {caption}", Text, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                case ErrorSeverity.Error:
                    MessageBox.Show($"Error #{ID}: {caption}", Text, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                case ErrorSeverity.FatalError:
                case ErrorSeverity.CatastrophicError:

                    // Create an EUIH
                    ErrorUIHost EUIH = new ErrorUIHost(Text);
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
