using System.Globalization;
using System.Windows.Controls;

namespace TKProcessor.WPF.Validators
{
    public class StringRangeValidationRule : ValidationRule
    {
        public int MinimumLength { get; set; } = -1;

        public int MaximumLength { get; set; } = -1;

        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            string inputString = (value ?? string.Empty).ToString();

            if (inputString.Length < MinimumLength ||
                   (MaximumLength > 0 &&
                    inputString.Length > MaximumLength))
            {
                result = new ValidationResult(false, ErrorMessage);
            }

            return result;
        }
    }
}
