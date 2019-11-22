using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;

namespace TKProcessor.WPF.Models
{
    public class ExcelMappingModel : PropertyChangedBase
    {
        private string target;
        private string source;
        private string replacementValues;

        private Dictionary<string, string[]> values;

        public ExcelMappingModel()
        {
            PropertyChanged += ExcelMappingModel_PropertyChanged;
        }

        private void ExcelMappingModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReplacementValues))
            {
                values = replacementValues.Split(';').Select(i => i.Split('=')).ToDictionary(i => i[0]);
            }
        }

        public string Target
        {
            get => target;
            set
            {
                target = value;
                NotifyOfPropertyChange();
            }
        }

        public string Source
        {
            get => source;
            set
            {
                source = value;
                NotifyOfPropertyChange();
            }
        }

        public string ReplacementValues
        {
            get => replacementValues;
            set
            {
                replacementValues = value;
                NotifyOfPropertyChange();
            }
        }

        public string this[string value]
        {
            get
            {
                if (values == null || values.Count == 0)
                    return value;

                var replacementSetup = values[value];

                return string.IsNullOrEmpty(replacementSetup[1]) ? value : replacementSetup[1];
            }
        }
    }
}
