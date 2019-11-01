using Caliburn.Micro;
using System;

namespace TKProcessor.WPF.Models
{
    public class Shift : BaseModel
    {
        private string _shiftCode;
        private string _description;
        private int _shiftType;
        private int _focusDate;
        private DateTime? _scheduleIn;
        private DateTime? _scheduleOut;
        private decimal _requiredWorkHours;
        private bool _isRestDay;
        private DateTime? _nightDiffStart;
        private DateTime? _nightDiffEnd;
        private DateTime? _amBreakIn;
        private DateTime? _amBreakOut;
        private DateTime? _pmBreakIn;
        private DateTime? _pmBreakOut;
        private DateTime? _lunchIn;
        private DateTime? _lunchOut;
        private DateTime? _dinnerIn;
        private DateTime? _dinnerOut;
        private bool _isLateIn;
        private int _gracePeriodLateIn;
        private int _afterEvery;
        private int _deductionLateIn;
        private bool _isPlusLateInMinutes;
        private int _maximumMinutesConsideredAsHalfDay;
        private bool _isEarlyOut;
        private int _gracePeriodEarlyOut;
        private decimal _afterEveryEarlyOut;
        private int _deductionOfEarlyOut;
        private bool _isPlusEarlyOutMinutes;
        private int _maximumMinutesConsideredAsHalfAayEarlyOut;
        private bool _isPreShiftOt;
        private int _minimumPreShiftOt;
        private int _maximumPreShiftOt;
        private int _roundPreShiftOt;
        private bool _isPostShiftOt;
        private int _minimumPostShiftOt;
        private int _maximumPostShiftOt;
        private int _roundPostShiftOt;
        private bool _isHolidayRestDayOt;
        private int _minimumHolidayRestDayOt;
        private int _maximumHolidayRestDayOt;
        private int _roundHolidayRestDayOt;
        private bool _isValid;
        private bool _isOverbreak;
        private int _flextimeType;
        private DateTime? _earliestTimeIn;
        private DateTime? _latestTimeIn;
        private DateTime? _earliestTimeOut;
        private DateTime? _latestTimeOut;
        private int _increment;

        public Shift() : base()
        {
            PropertyChanged += Shift_PropertyChanged;
        }

        private void Shift_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsValid) || e.PropertyName == nameof(IsDirty) || e.PropertyName == nameof(IsValid))
                return;

            IsDirty = true;

            IsValid = true;

            if (IsValid && (e.PropertyName == nameof(AmBreakIn) || e.PropertyName == nameof(AmBreakOut)))
                IsValid = AmBreakOut <= AmBreakIn;

            if (IsValid && (e.PropertyName == nameof(PmBreakIn) || e.PropertyName == nameof(PmBreakOut)))
                IsValid = PmBreakOut <= PmBreakIn;

            if (IsValid && (e.PropertyName == nameof(LunchIn) || e.PropertyName == nameof(LunchOut)))
                IsValid = LunchOut <= LunchIn;

            if (IsValid && (e.PropertyName == nameof(DinnerIn) || e.PropertyName == nameof(DinnerOut)))
                IsValid = DinnerOut <= DinnerIn;
        }

        public string ShiftCode
        {
            get => _shiftCode;
            set
            {
                _shiftCode = value;
                NotifyOfPropertyChange();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyOfPropertyChange();
            }
        }
        public int ShiftType
        {
            get => _shiftType;
            set
            {
                _shiftType = value;
                NotifyOfPropertyChange();
            }
        }
        public int FocusDate
        {
            get => _focusDate;
            set
            {
                _focusDate = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? ScheduleIn
        {
            get => _scheduleIn;
            set
            {
                _scheduleIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;

                NotifyOfPropertyChange();
            }
        }
        public DateTime? ScheduleOut
        {
            get => _scheduleOut;
            set
            {
                _scheduleOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public decimal RequiredWorkHours
        {
            get => _requiredWorkHours;
            set
            {
                _requiredWorkHours = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsRestDay
        {
            get => _isRestDay;
            set
            {
                _isRestDay = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? NightDiffStart
        {
            get => _nightDiffStart;
            set
            {
                _nightDiffStart = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? NightDiffEnd
        {
            get => _nightDiffEnd;
            set
            {
                _nightDiffEnd = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? AmBreakIn
        {
            get => _amBreakIn;
            set
            {
                _amBreakIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? AmBreakOut
        {
            get => _amBreakOut;
            set
            {
                _amBreakOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? PmBreakIn
        {
            get => _pmBreakIn;
            set
            {
                _pmBreakIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? PmBreakOut
        {
            get => _pmBreakOut;
            set
            {
                _pmBreakOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? LunchIn
        {
            get => _lunchIn;
            set
            {
                _lunchIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? LunchOut
        {
            get => _lunchOut;
            set
            {
                _lunchOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? DinnerIn
        {
            get => _dinnerIn;
            set
            {
                _dinnerIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? DinnerOut
        {
            get => _dinnerOut;
            set
            {
                _dinnerOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsOverbreak
        {
            get => _isOverbreak;
            set
            {
                _isOverbreak = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsLateIn
        {
            get => _isLateIn;
            set
            {
                _isLateIn = value;
                NotifyOfPropertyChange();
            }
        }
        public int GracePeriodLateIn
        {
            get => _gracePeriodLateIn;
            set
            {
                _gracePeriodLateIn = value;
                NotifyOfPropertyChange();
            }
        }
        public int AfterEvery
        {
            get => _afterEvery;
            set
            {
                _afterEvery = value;
                NotifyOfPropertyChange();
            }
        }
        public int DeductionLateIn
        {
            get => _deductionLateIn;
            set
            {
                _deductionLateIn = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsPlusLateInMinutes
        {
            get => _isPlusLateInMinutes;
            set
            {
                _isPlusLateInMinutes = value;
                NotifyOfPropertyChange();
            }
        }
        public int MaximumMinutesConsideredAsHalfDay
        {
            get => _maximumMinutesConsideredAsHalfDay;
            set
            {
                _maximumMinutesConsideredAsHalfDay = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsEarlyOut
        {
            get => _isEarlyOut;
            set
            {
                _isEarlyOut = value;
                NotifyOfPropertyChange();
            }
        }
        public int GracePeriodEarlyOut
        {
            get => _gracePeriodEarlyOut;
            set
            {
                _gracePeriodEarlyOut = value;
                NotifyOfPropertyChange();
            }
        }
        public decimal AfterEveryEarlyOut
        {
            get => _afterEveryEarlyOut;
            set
            {
                _afterEveryEarlyOut = value;
                NotifyOfPropertyChange();
            }
        }
        public int DeductionOfEarlyOut
        {
            get => _deductionOfEarlyOut;
            set
            {
                _deductionOfEarlyOut = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsPlusEarlyOutMinutes
        {
            get => _isPlusEarlyOutMinutes;
            set
            {
                _isPlusEarlyOutMinutes = value;
                NotifyOfPropertyChange();
            }
        }
        public int MaximumMinutesConsideredAsHalfAayEarlyOut
        {
            get => _maximumMinutesConsideredAsHalfAayEarlyOut;
            set
            {
                _maximumMinutesConsideredAsHalfAayEarlyOut = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsPreShiftOt
        {
            get => _isPreShiftOt;
            set
            {
                _isPreShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int MinimumPreShiftOt
        {
            get => _minimumPreShiftOt;
            set
            {
                _minimumPreShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int MaximumPreShiftOt
        {
            get => _maximumPreShiftOt;
            set
            {
                _maximumPreShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int RoundPreShiftOt
        {
            get => _roundPreShiftOt;
            set
            {
                _roundPreShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsPostShiftOt
        {
            get => _isPostShiftOt;
            set
            {
                _isPostShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int MinimumPostShiftOt
        {
            get => _minimumPostShiftOt;
            set
            {
                _minimumPostShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int MaximumPostShiftOt
        {
            get => _maximumPostShiftOt;
            set
            {
                _maximumPostShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int RoundPostShiftOt
        {
            get => _roundPostShiftOt;
            set
            {
                _roundPostShiftOt = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsHolidayRestDayOt
        {
            get => _isHolidayRestDayOt;
            set
            {
                _isHolidayRestDayOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int MinimumHolidayRestDayOt
        {
            get => _minimumHolidayRestDayOt;
            set
            {
                _minimumHolidayRestDayOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int MaximumHolidayRestDayOt
        {
            get => _maximumHolidayRestDayOt;
            set
            {
                _maximumHolidayRestDayOt = value;
                NotifyOfPropertyChange();
            }
        }
        public int RoundHolidayRestDayOt
        {
            get => _roundHolidayRestDayOt;
            set
            {
                _roundHolidayRestDayOt = value;
                NotifyOfPropertyChange();
            }
        }

        public int FlextimeType
        {
            get => _flextimeType;
            set
            {
                _flextimeType = value;
                NotifyOfPropertyChange();
            }
        }
        public int Increment
        {
            get => _increment;
            set
            {
                _increment = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? EarliestTimeIn
        {
            get => _earliestTimeIn;
            set
            {
                _earliestTimeIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? LatestTimeIn
        {
            get => _latestTimeIn;
            set
            {
                _latestTimeIn = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? EarliestTimeOut
        {
            get => _earliestTimeOut;
            set
            {
                _earliestTimeOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime? LatestTimeOut
        {
            get => _latestTimeOut;
            set
            {
                _latestTimeOut = value.HasValue ? value.Value.AddSeconds(-value.Value.Second) : value;
                NotifyOfPropertyChange();
            }
        }

        public override bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                NotifyOfPropertyChange();
            }
        }

        public override string ToString()
        {
            return ShiftCode;
        }
    }
}
