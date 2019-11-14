using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TKProcessor.WPF.Converter
{
    public class NumberAbbrevConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
                return FormatDecimal((decimal)value);

            return FormatInt((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static string FormatInt(int num)
        {
            if (num >= 100000)
                return FormatInt(num / 1000) + "K";
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.#") + "K";
            }
            return num.ToString("#,0");
        }

        static string FormatDecimal(decimal num)
        {
            if (num >= 100000)
                return FormatDecimal(num / 1000) + "K";
            if (num >= 10000)
            {
                return (num / 1000M).ToString("0.#") + "K";
            }
            return num.ToString("#,0");
        }
    }
}
