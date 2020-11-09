using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Track_Maker
{
    /// <summary>
    /// WPF Stupidity Workaround ValueConverter 
    /// 
    /// 2020-11-09
    /// </summary>
    public class IHateWPF : IValueConverter
    {
        /// <summary>
        /// String / uri to BitmapImage converter
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="TargetType"></param>
        /// <param name="Parameter"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            
            if (Value == null)
            {
                Error.Throw("Fatal", "Attempted to convert null value to BitmapImage!", ErrorSeverity.FatalError, 197);
                return null; // oops (pre-standard input validation)
            }

            // Strict type-correctness for this
            bool IsString = false;
            bool IsUri = false;

            IsString = Value is string;
            IsUri = Value is Uri;

            // The final image uri
            Uri ImageUri = null;

            if (!IsString && !IsUri)
            {
                Error.Throw("Fatal", "An error has occurred loading the basin image.\n\nError Converting Uri or String to BitmapImage (Attempted to convert an object that is not a uri or string to a BitmapImage!)", ErrorSeverity.FatalError, 195);

                return default(BitmapImage);
            }
            else
            {
                if (IsString)
                {
                    IsString = false;
                    string ImageString = (string)Value;
                    ImageUri = new Uri(ImageString); 
                }
                else if (IsUri)
                {
                    ImageUri = (Uri)Value; 
                }

                BitmapImage BI = new BitmapImage();
                BI.BeginInit();
                BI.UriSource = ImageUri;
                BI.EndInit();
                return BI;
            }
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            NotImplementedException NIE = new NotImplementedException("FatalERROR [196]: A FatalERR");
            Error.Throw("NO NO NO", "Attempted to call ConvertBack on IHateWPF; you cannot do this - cannot convert BitmapImage to string/uri", ErrorSeverity.FatalError, 196);
#if DEBUG
            throw NIE; 
#else
            return null;
#endif


        }
    }
}
