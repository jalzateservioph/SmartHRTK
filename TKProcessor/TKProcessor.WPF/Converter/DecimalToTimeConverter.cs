using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TKProcessor.WPF.Converter
{
    public class DecimalToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal hourValue = value == null ? 0 : (decimal)value;

            TimeSpan timespan = TimeSpan.FromHours(System.Convert.ToDouble(hourValue));

            int hours = timespan.Hours,
                mins = timespan.Minutes + (timespan.Seconds > 30 ? 1 : 0);

            string output = $"{hours} hr{(hours > 1 ? "s" : "")}  " +
                            $"{mins} min{(mins > 1 ? "s" : "")}";

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DecimalToHourPartConverter : IValueConverter
    {
        decimal originalHourValue = 0;
        decimal originalMinValue = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal timeValue = value == null ? 0 : (decimal)value;

            TimeSpan timespan = TimeSpan.FromHours(System.Convert.ToDouble(timeValue));

            originalHourValue = timespan.Hours;
            originalMinValue = timespan.Minutes + (timespan.Seconds > 30 ? 1 : 0);

            return originalHourValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal hourValue = System.Convert.ToDecimal(string.IsNullOrEmpty(value.ToString()) ? 0 : value);

            if (hourValue > 23)
                hourValue = 23;

            return hourValue + (originalMinValue / 60);
        }
    }

    public class DecimalToMinutePartConverter : IValueConverter
    {
        decimal originalHourValue = 0;
        decimal originalMinValue = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal timeValue = value == null ? 0 : (decimal)value;

            TimeSpan timespan = TimeSpan.FromHours(System.Convert.ToDouble(timeValue));

            originalHourValue = timespan.Hours;
            originalMinValue = timespan.Minutes + (timespan.Seconds > 30 ? 1 : 0);

            return originalMinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal minPartValue = System.Convert.ToDecimal(string.IsNullOrEmpty(value.ToString()) ? 0 : value);

            if (minPartValue > 59)
                minPartValue = 59;

            return originalHourValue + (minPartValue / 60);
        }
    }
}
