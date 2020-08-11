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
        public static void Throw(string caption, string text, ErrorSeverity severity, int id = 0)
        {
            if (severity == ErrorSeverity.Message) // message
            {
                MessageBox.Show(caption, text, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (severity == ErrorSeverity.Warning) // warning
            {
                MessageBox.Show($"Warning: {caption}", text, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (severity == ErrorSeverity.Error) // non-fatal error - the program can continue running
            {
                MessageBox.Show($"Error #{id}: {caption}", text, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (severity == ErrorSeverity.FatalError) // fatal error - the program has to exit
            {
                MessageBox.Show($"Fatal Error #{id}: {caption}", text, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(id);
            }
            if (severity == ErrorSeverity.CatastrophicError) // catastrophic error - there is an issue with a file related to the program and the program probably has to be reinstalled
            {
                MessageBox.Show($"Catastrophic Unrecoverable Irrepairable Failure (CUIF): {caption}\n\nPlease reinstall the Track Maker.", text, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
