using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class DailyTransactionRecord : BaseModel
    {
        private decimal _actualLate;
        private decimal _approvedLate;
        private decimal _actualUndertime;
        private decimal _approvedUndertime;
        private decimal _actualOvertime;
        private decimal _approvedOvertime;
        private decimal _actualPreOvertime;
        private decimal _approvedPreOvertime;
        private decimal _actualPostOvertime;
        private decimal _approvedPostOvertime;
        private decimal _nightDifferential;
        private decimal _nightDifferentialOt;
        private decimal _actualRestDay;
        private decimal _approvedRestDay;
        private decimal _actualRestDayOt;
        private decimal _approvedRestDayOt;
        private decimal _actualNDRD;
        private decimal _actualNDRDot;
        private decimal _approvedNDRDot;
        private decimal _actualLegHol;
        private decimal _approvedLegHol;
        private decimal _actualLegHolOt;
        private decimal _approvedLegHolOt;
        private decimal _actualSpeHol;
        private decimal _approvedSpeHol;
        private decimal _actualSpeHolOt;
        private decimal _approvedSpeHolOt;
        private decimal _actualLegSpeHol;
        private decimal _approvedLegSpeHol;
        private decimal _actualLegSpeHolOt;
        private decimal _approvedLegSpeHolOt;
        private decimal _actualLegHolRD;
        private decimal _approvedLegHolRD;
        private decimal _actualLegHolRDot;
        private decimal _approvedLegHolRDot;
        private decimal _actualSpeHolRD;
        private decimal _approvedSpeHolRD;
        private decimal _actualSpeHolRDot;
        private decimal _approvedSpeHolRDot;
        private decimal _actualLegSpeHolRD;
        private decimal _approvedLegSpeHolRD;
        private decimal _actualLegSpeHolRDot;
        private decimal _approvedLegSpeHolRDot;
        private decimal _actualNDLegHol;
        private decimal _approvedNDLegHol;
        private decimal _actualLegNDHolOt;
        private decimal _approvedNDLegHolOt;
        private decimal _actualNDSpeHol;
        private decimal _approvedNDSpeHol;
        private decimal _actualNDSpeHolOt;
        private decimal _approvedNDSpeHolOt;
        private decimal _actualNDLegSpeHol;
        private decimal _approvedNDLegSpeHol;
        private decimal _actualNDLegSpeHolOt;
        private decimal _approvedNDLegSpeHolOt;
        private decimal _actualNDLegHolRD;
        private decimal _approvedNDLegHolRD;
        private decimal _actualNDLegHolRDot;
        private decimal _approvedNDLegHolRDot;
        private decimal _actualNDSpeHolRD;
        private decimal _approvedNDSpeHolRD;
        private decimal _actualNDSpeHolRDot;
        private decimal _approvedNDSpeHolRDot;
        private decimal _actualNDLegSpeHolRD;
        private decimal _approvedNDLegSpeHolRD;
        private decimal _actualNDLegSpeHolRDot;
        private decimal _approvedNDLegSpeHolRDot;

        public DailyTransactionRecord()
        {
            PropertyChanged += DailyTransactionRecord_PropertyChanged;
        }

        private void DailyTransactionRecord_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("Approved", StringComparison.CurrentCulture))
            {
                var baseFieldName = e.PropertyName.Replace("Approved", "");

                decimal? actualValue = typeof(DailyTransactionRecord).GetProperty("Actual" + baseFieldName).GetValue(this) as decimal?;
                decimal? approvedValue = typeof(DailyTransactionRecord).GetProperty("Actual" + baseFieldName).GetValue(this) as decimal?;

                if (approvedValue.HasValue && actualValue.HasValue && approvedValue > actualValue)
                {
                    typeof(DailyTransactionRecord).GetProperty("Actual" + baseFieldName).SetValue(this, actualValue);

                    throw new Exception($"Appoved {baseFieldName} cannot be greater than Actual {baseFieldName}");
                }
            }
        }

        public Employee Employee { get; set; }
        public Shift Shift { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public decimal WorkHours { get; set; }
        public decimal AbsentHours { get; set; }
        public decimal ActualLate
        {
            get => _actualLate;
            set
            {
                _actualLate = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLate
        {
            get => _approvedLate;
            set
            {
                _approvedLate = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualUndertime
        {
            get => _actualUndertime;
            set
            {
                _actualUndertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedUndertime
        {
            get => _approvedUndertime;
            set
            {
                _approvedUndertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualOvertime
        {
            get => _actualOvertime;
            set
            {
                _actualOvertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedOvertime
        {
            get => _approvedOvertime;
            set
            {
                _approvedOvertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualPreOvertime
        {
            get => _actualPreOvertime;
            set
            {
                _actualPreOvertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedPreOvertime
        {
            get => _approvedPreOvertime;
            set
            {
                _approvedPreOvertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualPostOvertime
        {
            get => _actualPostOvertime;
            set
            {
                _actualPostOvertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedPostOvertime
        {
            get => _approvedPostOvertime;
            set
            {
                _approvedPostOvertime = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal NightDifferential
        {
            get => _nightDifferential;
            set
            {
                _nightDifferential = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal NightDifferentialOt
        {
            get => _nightDifferentialOt;
            set
            {
                _nightDifferentialOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualRestDay
        {
            get => _actualRestDay;
            set
            {
                _actualRestDay = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedRestDay
        {
            get => _approvedRestDay;
            set
            {
                _approvedRestDay = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualRestDayOt
        {
            get => _actualRestDayOt;
            set
            {
                _actualRestDayOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedRestDayOt
        {
            get => _approvedRestDayOt;
            set
            {
                _approvedRestDayOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDRD
        {
            get => _actualNDRD;
            set
            {
                _actualNDRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDRD
        {
            get;
            set;
        }
        public decimal ActualNDRDot
        {
            get => _actualNDRDot;
            set
            {
                _actualNDRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDRDot
        {
            get => _approvedNDRDot;
            set
            {
                _approvedNDRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegHol
        {
            get => _actualLegHol;
            set
            {
                _actualLegHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegHol
        {
            get => _approvedLegHol;
            set
            {
                _approvedLegHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegHolOt
        {
            get => _actualLegHolOt;
            set
            {
                _actualLegHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegHolOt
        {
            get => _approvedLegHolOt;
            set
            {
                _approvedLegHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualSpeHol
        {
            get => _actualSpeHol;
            set
            {
                _actualSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedSpeHol
        {
            get => _approvedSpeHol;
            set
            {
                _approvedSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualSpeHolOt
        {
            get => _actualSpeHolOt;
            set
            {
                _actualSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedSpeHolOt
        {
            get => _approvedSpeHolOt;
            set
            {
                _approvedSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegSpeHol
        {
            get => _actualLegSpeHol;
            set
            {
                _actualLegSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegSpeHol
        {
            get => _approvedLegSpeHol;
            set
            {
                _approvedLegSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegSpeHolOt
        {
            get => _actualLegSpeHolOt;
            set
            {
                _actualLegSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegSpeHolOt
        {
            get => _approvedLegSpeHolOt;
            set
            {
                _approvedLegSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegHolRD
        {
            get => _actualLegHolRD;
            set
            {
                _actualLegHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegHolRD
        {
            get => _approvedLegHolRD;
            set
            {
                _approvedLegHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegHolRDot
        {
            get => _actualLegHolRDot;
            set
            {
                _actualLegHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegHolRDot
        {
            get => _approvedLegHolRDot;
            set
            {
                _approvedLegHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualSpeHolRD
        {
            get => _actualSpeHolRD;
            set
            {
                _actualSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedSpeHolRD
        {
            get => _approvedSpeHolRD;
            set
            {
                _approvedSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualSpeHolRDot
        {
            get => _actualSpeHolRDot;
            set
            {
                _actualSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedSpeHolRDot
        {
            get => _approvedSpeHolRDot;
            set
            {
                _approvedSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegSpeHolRD
        {
            get => _actualLegSpeHolRD;
            set
            {
                _actualLegSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegSpeHolRD
        {
            get => _approvedLegSpeHolRD;
            set
            {
                _approvedLegSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualLegSpeHolRDot
        {
            get => _actualLegSpeHolRDot;
            set
            {
                _actualLegSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedLegSpeHolRDot
        {
            get => _approvedLegSpeHolRDot;
            set
            {
                _approvedLegSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegHol
        {
            get => _actualNDLegHol;
            set
            {
                _actualNDLegHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegHol
        {
            get => _approvedNDLegHol;
            set
            {
                _approvedNDLegHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegHolOt
        {
            get => _actualLegNDHolOt;
            set
            {
                _actualLegNDHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegHolOt
        {
            get => _approvedNDLegHolOt;
            set
            {
                _approvedNDLegHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDSpeHol
        {
            get => _actualNDSpeHol;
            set
            {
                _actualNDSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDSpeHol
        {
            get => _approvedNDSpeHol;
            set
            {
                _approvedNDSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDSpeHolOt
        {
            get => _actualNDSpeHolOt;
            set
            {
                _actualNDSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDSpeHolOt
        {
            get => _approvedNDSpeHolOt;
            set
            {
                _approvedNDSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegSpeHol
        {
            get => _actualNDLegSpeHol;
            set
            {
                _actualNDLegSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegSpeHol
        {
            get => _approvedNDLegSpeHol;
            set
            {
                _approvedNDLegSpeHol = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegSpeHolOt
        {
            get => _actualNDLegSpeHolOt;
            set
            {
                _actualNDLegSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegSpeHolOt
        {
            get => _approvedNDLegSpeHolOt;
            set
            {
                _approvedNDLegSpeHolOt = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegHolRD
        {
            get => _actualNDLegHolRD;
            set
            {
                _actualNDLegHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegHolRD
        {
            get => _approvedNDLegHolRD;
            set
            {
                _approvedNDLegHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegHolRDot
        {
            get => _actualNDLegHolRDot;
            set
            {
                _actualNDLegHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegHolRDot
        {
            get => _approvedNDLegHolRDot;
            set
            {
                _approvedNDLegHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDSpeHolRD
        {
            get => _actualNDSpeHolRD;
            set
            {
                _actualNDSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDSpeHolRD
        {
            get => _approvedNDSpeHolRD;
            set
            {
                _approvedNDSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDSpeHolRDot
        {
            get => _actualNDSpeHolRDot;
            set
            {
                _actualNDSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDSpeHolRDot
        {
            get => _approvedNDSpeHolRDot;
            set
            {
                _approvedNDSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegSpeHolRD
        {
            get => _actualNDLegSpeHolRD;
            set
            {
                _actualNDLegSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegSpeHolRD
        {
            get => _approvedNDLegSpeHolRD;
            set
            {
                _approvedNDLegSpeHolRD = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ActualNDLegSpeHolRDot
        {
            get => _actualNDLegSpeHolRDot;
            set
            {
                _actualNDLegSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal ApprovedNDLegSpeHolRDot
        {
            get => _approvedNDLegSpeHolRDot;
            set
            {
                _approvedNDLegSpeHolRDot = value;
                NotifyOfPropertyChange();
            }
        }
        public string Remarks { get; set; }
        public string LeaveType { get; set; }
    }
}
