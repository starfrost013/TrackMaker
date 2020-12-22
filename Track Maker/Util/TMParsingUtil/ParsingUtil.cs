using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    public static class ParsingUtil
    {
        /// <summary>
        /// Parse an ATCF DateTime formatted string and convert it to a .NET DateTime. (yyyymmddhh)
        /// </summary>
        /// <param name="ATCFDTFormatString">ATCF formatted string</param>
        /// <returns>The DateTime you selected.</returns>
        public static DateTime ParseATCFDateTime(string ATCFDTFormatString)
        {
            if (ATCFDTFormatString.Length != 10)
            {
                Error.Throw("Error!", "Attempted to convert an invalid ATCF-format DateTime.", ErrorSeverity.Error, 250);
                return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
            }
            else
            {
                // Obtain the components of the date from the string
                string Year = ATCFDTFormatString.Substring(0, 4);
                string Month = ATCFDTFormatString.Substring(4, 2);
                string Day = ATCFDTFormatString.Substring(6, 2);
                string Hour = ATCFDTFormatString.Substring(8, 2);

                // Convert them to ints
                // You should handle OverflowException, FormatException etc yourself
                int YearI = Convert.ToInt32(Year);
                int MonthI = Convert.ToInt32(Month);
                int DayI = Convert.ToInt32(Day);
                int HourI = Convert.ToInt32(Hour);

                // Check for an invalid date or time.
                if (YearI < 1 
                    || YearI > 9999 
                    || MonthI < 0
                    || (MonthI == 2 && DayI > 28) 
                    || (MonthI != 2 && DayI > 31) // not perfect 
                    || MonthI > 12
                    || HourI < 0
                    || HourI > 24
                    )
                {
                    Error.Throw("Error!", "Attempted to convert an invalid ATCF-format DateTime: one of the date components was outside of the bounds (Year [1, 9999], Month [0,12] Day [0,28/29/31], Hour [0,24])!", ErrorSeverity.Error, 251);
                    return new DateTime(240, 11, 16, 1, 33, 22);
                }
                else
                {
                    return new DateTime(YearI, MonthI, DayI, HourI, 0, 0);
                }
            }
        }
    }
}
