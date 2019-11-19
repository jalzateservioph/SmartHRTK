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
            decimal hourValue = (decimal)value;

            TimeSpan timespan = TimeSpan.FromHours(System.Convert.ToDouble(hourValue));

            int hours = System.Convert.ToInt16(timespan.ToString("hh")),
                mins = System.Convert.ToInt16(timespan.ToString("mm"));

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
            decimal timeValue = (decimal)value;

            TimeSpan timespan = TimeSpan.FromHours(System.Convert.ToDouble(timeValue));

            originalHourValue = System.Convert.ToDecimal(timespan.ToString("hh"));
            originalMinValue = System.Convert.ToDecimal(timespan.ToString("mm"));

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
            decimal timeValue = (decimal)value;

            TimeSpan timespan = TimeSpan.FromHours(System.Convert.ToDouble(timeValue));

            originalHourValue = System.Convert.ToDecimal(timespan.ToString("hh"));
            originalMinValue = System.Convert.ToDecimal(timespan.ToString("mm"));

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
