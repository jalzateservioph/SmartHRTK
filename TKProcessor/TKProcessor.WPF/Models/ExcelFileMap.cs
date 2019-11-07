using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class ExcelFileMap : PropertyChangedBase
    {
        private Dictionary<string, string> replacementRuleDictionary;

        private string target;
        private string source;
        private string replaceRules;

        public ExcelFileMap()
        {
            Id = Guid.NewGuid();

            replacementRuleDictionary = new Dictionary<string, string>();
        }

        private void LoadReplacementDictionary()
        {
            replacementRuleDictionary = new Dictionary<string, string>();

            foreach (var rule in ReplaceRules.Split(';'))
            {
                for (int i = 0; i < rule.Length; i++)
                {

                }
            }
        }

        public string this[string value]
        {
            get
            {
                var possibleReplacements = replacementRuleDictionary[value].Split(',');

                var replacement = "";

                

                return replacement;
            }
        }

        public Guid Id { get; }
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
        public string ReplaceRules
        {
            get => replaceRules;
            set
            {
                replaceRules = value;
                LoadReplacementDictionary();
                NotifyOfPropertyChange();
            }
        }
    }
}
