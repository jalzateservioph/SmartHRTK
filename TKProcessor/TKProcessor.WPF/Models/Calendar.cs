using System;
using System.Collections.ObjectModel;

namespace TKProcessor.WPF.Models
{
    public class Calendar : BaseModel
    {
        private string _name;
        private ObservableCollection<Holiday> _holidays;

        public Calendar()
        {
            PropertyChanged += CalendarDto_PropertyChanged;
        }

        private void CalendarDto_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsDirty = true;

            if (e.PropertyName == nameof(Name))
            {
                IsValid = !string.IsNullOrEmpty(Name);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }
        public ObservableCollection<Holiday> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                NotifyOfPropertyChange();
            }
        }
    }
}