using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TKProcessor.WPF.Converter
{
    public class CollectionToCommaSeparatedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = "";

            if (value is ICollection)
            {
                foreach (var item in (value as ICollection))
                    output += item.ToString() + ", ";

                if(!string.IsNullOrEmpty(output))
                    output = output.Remove(output.Length - 2);
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
