using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Common
{
    public static class DateTimeHelpers
    {
        public static DateTime GetTimeOnly(this DateTime time)
        {
            return new DateTime(
                        DateTime.MinValue.Year,
                        DateTime.MinValue.Month,
                        DateTime.MinValue.Day,
                        time.Hour,
                        time.Minute,
                        time.Second,
                        time.Millisecond
                    );
        }

        public static DateTime RemoveSeconds(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0, 0);
        }

        public static DateTime ConstructDate(DateTime datePart, DateTime timePart)
        {
            return new DateTime(datePart.Year, datePart.Month, datePart.Day, timePart.TimeOfDay.Hours, timePart.TimeOfDay.Minutes, timePart.TimeOfDay.Seconds);
        }

        public static DateTime? Parse(object value)
        {
            var rowValue = value.ToString();

            if (DateTime.TryParse(rowValue, out DateTime dt))
            {
                return dt;
            }
            else
            {
                return DateTime.FromOADate(double.Parse(rowValue));
            }

            return null;
        }
    }
}
