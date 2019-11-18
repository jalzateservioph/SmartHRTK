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

            string output = $"{hours} hr{(hours > 0 ? "s" : "")}  " +
                            $"{mins} min{(mins > 0 ? "s" : "")}";

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
