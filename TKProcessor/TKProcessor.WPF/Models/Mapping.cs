using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class Mapping : PropertyChangedBase
    {
        private string _target;
        private string _source;

        public Mapping()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Target
        {
            get => _target;
            set
            {
                _target = value;
                NotifyOfPropertyChange();
            }
        }
        public string Source
        {
            get => _source;
            set
            {
                _source = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
