using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TKProcessor.WPF.Validators
{
    public class StringIntegerValidationRule : ValidationRule
    {
        public int MinValue { get; set; } = int.MinValue;

        public int MaxValue { get; set; } = int.MaxValue;

        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            string inputString = (value ?? string.Empty).ToString();

            if (!int.TryParse(inputString, out int i))
            {
                result = new ValidationResult(false, ErrorMessage);
            }
            else if (i > MaxValue || i < MinValue)
            {
                result = new ValidationResult(false, ErrorMessage);
            }

            return result;
        }
    }
}
