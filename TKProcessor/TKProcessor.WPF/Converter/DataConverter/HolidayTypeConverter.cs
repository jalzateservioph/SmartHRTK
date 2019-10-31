using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TKProcessor.Models.TK;

namespace TKProcessor.WPF.Converter.DataConverter
{
    public class HolidayTypeConverter : IValueConverter
    {
        readonly Dictionary<int, HolidayType> valuesInt;
        readonly Dictionary<string, HolidayType> valuesStr;

        public HolidayTypeConverter()
        {
            valuesInt = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>().ToDictionary(i => (int)i);
            valuesStr = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>().ToDictionary(i => i.ToString());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return valuesInt[(int)value].ToString() + " Holiday";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            string val = value.ToString().Replace(" Holiday", "");

            return valuesStr[val];
        }
    }
}
