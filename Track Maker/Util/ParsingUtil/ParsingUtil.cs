using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMaker.Core;

namespace Track_Maker
{
    public static class ParsingUtil
    {
        /// <summary>
        /// Parse an ATCF DateTime formatted string and convert it to a .NET DateTime. (yyyymmddhh)
        /// </summary>
        /// <param name="ATCFDTFormatString">ATCF formatted string</param>
        /// <returns>The DateTime you selected.</returns>
        public static DateTime ParseATCFDateTime(string ATCFDTFormatString, CoordinateFormat AF = CoordinateFormat.HURDAT2)
        {
            try
            {
                switch (AF)
                {
                    case CoordinateFormat.ATCF:
                        return ParseATCFDateTime_ATCF(ATCFDTFormatString);
                    case CoordinateFormat.HURDAT2:
                        return ParseATCFDateTime_HURDAT2(ATCFDTFormatString);
                }

                return new DateTime(240, 11, 16, 1, 33, 22); // failure state (Pre-V3)
            }
            catch (FormatException err)
            {
#if DEBUG
                Error.Throw("Error!", $"Attempted to convert an invalid HURDAT2-format DateTime.\n\n{err}", ErrorSeverity.Error, 345);
                return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
#else
                Error.Throw("Error!", "Attempted to convert an invalid HURDAT2-format DateTime.", ErrorSeverity.Error, 345);
                return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
#endif
            }
        }

        private static DateTime ParseATCFDateTime_ATCF(string ATCFDTFormatString)
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
                    Error.Throw("Error!", "Attempted to convert an invalid ATCF-format DateTime: The date supplied was outside of the 01/01/0001 to 31/12/9999 bounds.", ErrorSeverity.Error, 251);
                    return new DateTime(240, 11, 16, 1, 33, 22);
                }
                else
                {
                    return new DateTime(YearI, MonthI, DayI, HourI, 0, 0);
                }
            }
        }

        /// <summary>
        /// Parse HURDAT2 format date.
        /// 
        /// NEEDS TO BE MORE VERSATILE
        /// </summary>
        /// <param name="HURDAT2DTFormatString"></param>
        /// <returns></returns>
        private static DateTime ParseATCFDateTime_HURDAT2(string HURDAT2DTFormatString)
        {
            if (HURDAT2DTFormatString.Length != 14)
            {
                Error.Throw("Error!", "Attempted to convert an invalid HURDAT2-format DateTime.", ErrorSeverity.Error, 341);
                return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
            }
            else
            {
                List<string> DateComponents = HURDAT2DTFormatString.Split(',').ToList();

                if (DateComponents.Count != 2)
                {
                    Error.Throw("Error!", "Attempted to convert an invalid HURDAT2-format DateTime.", ErrorSeverity.Error, 342);
                    return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
                }

                string Date = DateComponents[0];
                string Time = DateComponents[1];

                // just in case
                Date = Date.Trim();
                Time = Time.Trim();

                if (Date.Length != 8 || Time.Length != 4)
                {
                    Error.Throw("Error!", "Attempted to convert an invalid HURDAT2-format DateTime.", ErrorSeverity.Error, 343);
                    return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
                }
                else
                {
                    string YearC = Date.Substring(0, 4);
                    string MonthC = Date.Substring(3, 2);
                    string DayC = Date.Substring(5, 2);
                    string HourC = Time.Substring(0, 2);
                    string MinuteC = Time.Substring(2, 2);

                    // Trim to prevent Error 344
                    YearC = YearC.Trim();
                    MonthC = MonthC.Trim();
                    DayC = DayC.Trim();
                    HourC = HourC.Trim();
                    MinuteC = MinuteC.Trim(); 

                    int YearI = Convert.ToInt32(YearC);
                    int MonthI = Convert.ToInt32(MonthC);
                    int DayI = Convert.ToInt32(DayC);
                    int HourI = Convert.ToInt32(HourC);
                    int MinuteI = Convert.ToInt32(MinuteC);

                    if (YearI < 1 || YearI > 9999
                        || MonthI < 1 || MonthI > 12
                        || (MonthI == 2 && DayI > 28)
                        || DayI < 1 || DayI > 31 // YES THIS DOESN'T WORK WITH LEAP YEARS BUT WHO FUCKING CARES
                        || HourI < 0 || HourI > 23
                        || MinuteI < 0 || MinuteI > 59)
                    {
                        Error.Throw("Error!", "Attempted to convert an invalid HURDAT2-format DateTime: The date supplied was outside of the 01/01/0001 to 31/12/9999 bounds.", ErrorSeverity.Error, 344);
                        return new DateTime(240, 11, 16, 1, 33, 22); // pre-v3
                    }
                    else
                    {
                        return new DateTime(YearI, MonthI, DayI, HourI, MinuteI, 0); 
                    }
                        
                }

            }
        }
    }
}
