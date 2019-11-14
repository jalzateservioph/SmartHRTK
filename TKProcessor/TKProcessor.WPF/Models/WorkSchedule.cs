using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;

namespace TKProcessor.WPF.Models
{
    public class WorkSchedule: BaseModel
    {
        private Employee _employee;
        private DateTime _scheduleDate;
        private Shift _shift;

        public Employee Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime ScheduleDate
        {
            get => _scheduleDate;
            set
            {
                _scheduleDate = value.Date;
                NotifyOfPropertyChange();
            }
        }
        public Shift Shift
        {
            get => _shift;
            set
            {
                _shift = value;
                NotifyOfPropertyChange();
            }
        }

        //Advanced Fields
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Shift Monday { get; set; }
        public Shift Tuesday { get; set; }
        public Shift Wednesday { get; set; }
        public Shift Thursday { get; set; }
        public Shift Friday { get; set; }
        public Shift Saturday { get; set; }
        public Shift Sunday { get; set; }
        //Advanced Fields
    }
}