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
        private int yearTo;
        private DateTime date;

        public HolidayModel()
        {
            Date = DateTime.Today;
            YearTo = DateTime.Today.Year;

            PropertyChanged += HolidayModel_PropertyChanged;
        }

        private void HolidayModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Date))
                YearTo = Date.Year;
        }

        public string Name { get; set; }
        public int YearTo
        {
            get => yearTo;
            set
            {
                yearTo = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsRegularHoliday { get; set; }
        public bool IsSpecialHoliday { get; set; }
    }

    
}