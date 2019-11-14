using Caliburn.Micro;
using System;

namespace TKProcessor.WPF.Models
{
    public class Holiday : BaseModel
    {
        private string _name;
        private int _type;
        private DateTime _date;

        public Holiday()
        {
            PropertyChanged += Holiday_PropertyChanged;
        }

        private void Holiday_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Name) || e.PropertyName == nameof(Type) || e.PropertyName == nameof(Date))
                IsDirty = true;
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
        public int Type
        {
            get => _type;
            set
            {
                _type = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                NotifyOfPropertyChange();
            }
        }
    }

    public class HolidayModel : BaseModel
    {
        public HolidayModel()
        {
            Date = DateTime.Today;
            YearTo = DateTime.Today.Year;
        }

        public string Name { get; set; }
        public int YearTo { get; set; }
        public DateTime Date { get; set; }
        public bool IsRegularHoliday { get; set; }
        public bool IsSpecialHoliday { get; set; }
    }
}