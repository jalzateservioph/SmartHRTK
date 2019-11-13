using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class DailyTransactionRecord : IEntity
    {
        public DailyTransactionRecord()
        {
            Id = Guid.NewGuid();
        }

        public void AddRemarks(string message)
        {
            if (!string.IsNullOrEmpty(Remarks))
                Remarks += "; ";

            Remarks += "Employee has no work schedule setup for this date";
        }

        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }


        public Employee Employee { get; set; }
        public Shift Shift { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public decimal RegularWorkHours { get; set; }
        public decimal WorkHours { get; set; }
        public decimal AbsentHours { get; set; }
        public decimal ActualLate { get; set; }
        public decimal ApprovedLate { get; set; }
        public decimal ActualUndertime { get; set; }
        public decimal ApprovedUndertime { get; set; }
        public decimal ActualOvertime { get; set; }
        public decimal ApprovedOvertime { get; set; }
        public decimal ActualPreOvertime { get; set; }
        public decimal ApprovedPreOvertime { get; set; }
        public decimal ActualPostOvertime { get; set; }
        public decimal ApprovedPostOvertime { get; set; }
        public decimal NightDifferential { get; set; }
        public decimal NightDifferentialOt { get; set; }
        public decimal ActualRestDay { get; set; }
        public decimal ApprovedRestDay { get; set; }
        public decimal ActualRestDayOt { get; set; }
        public decimal ApprovedRestDayOt { get; set; }
        public decimal ActualNDRD { get; set; }
        public decimal ApprovedNDRD { get; set; }
        public decimal ActualNDRDot { get; set; }
        public decimal ApprovedNDRDot { get; set; }
        public decimal ActualLegHol { get; set; }
        public decimal ApprovedLegHol { get; set; }
        public decimal ActualLegHolOt { get; set; }
        public decimal ApprovedLegHolOt { get; set; }
        public decimal ActualSpeHol { get; set; }
        public decimal ApprovedSpeHol { get; set; }
        public decimal ActualSpeHolOt { get; set; }
        public decimal ApprovedSpeHolOt { get; set; }
        public decimal ActualLegSpeHol { get; set; }
        public decimal ApprovedLegSpeHol { get; set; }
        public decimal ActualLegSpeHolOt { get; set; }
        public decimal ApprovedLegSpeHolOt { get; set; }
        public decimal ActualLegHolRD { get; set; }
        public decimal ApprovedLegHolRD { get; set; }
        public decimal ActualLegHolRDot { get; set; }
        public decimal ApprovedLegHolRDot { get; set; }
        public decimal ActualSpeHolRD { get; set; }
        public decimal ApprovedSpeHolRD { get; set; }
        public decimal ActualSpeHolRDot { get; set; }
        public decimal ApprovedSpeHolRDot { get; set; }
        public decimal ActualLegSpeHolRD { get; set; }
        public decimal ApprovedLegSpeHolRD { get; set; }
        public decimal ActualLegSpeHolRDot { get; set; }
        public decimal ApprovedLegSpeHolRDot { get; set; }
        public decimal ActualNDLegHol { get; set; }
        public decimal ApprovedNDLegHol { get; set; }
        public decimal ActualNDLegHolOt { get; set; }
        public decimal ApprovedNDLegHolOt { get; set; }
        public decimal ActualNDSpeHol { get; set; }
        public decimal ApprovedNDSpeHol { get; set; }
        public decimal ActualNDSpeHolOt { get; set; }
        public decimal ApprovedNDSpeHolOt { get; set; }
        public decimal ActualNDLegSpeHol { get; set; }
        public decimal ApprovedNDLegSpeHol { get; set; }
        public decimal ActualNDLegSpeHolOt { get; set; }
        public decimal ApprovedNDLegSpeHolOt { get; set; }
        public decimal ActualNDLegHolRD { get; set; }
        public decimal ApprovedNDLegHolRD { get; set; }
        public decimal ActualNDLegHolRDot { get; set; }
        public decimal ApprovedNDLegHolRDot { get; set; }
        public decimal ActualNDSpeHolRD { get; set; }
        public decimal ApprovedNDSpeHolRD { get; set; }
        public decimal ActualNDSpeHolRDot { get; set; }
        public decimal ApprovedNDSpeHolRDot { get; set; }
        public decimal ActualNDLegSpeHolRD { get; set; }
        public decimal ApprovedNDLegSpeHolRD { get; set; }
        public decimal ActualNDLegSpeHolRDot { get; set; }
        public decimal ApprovedNDLegSpeHolRDot { get; set; }
        public string Remarks { get; set; }
        public string LeaveType { get; set; }

        public void Merge(DailyTransactionRecord mergee)
        {
            this.RegularWorkHours += mergee.RegularWorkHours;
            this.WorkHours += mergee.WorkHours;
            this.AbsentHours += mergee.AbsentHours;
            this.ActualLate += mergee.ActualLate;
            this.ApprovedLate += mergee.ApprovedLate;
            this.ActualUndertime += mergee.ActualUndertime;
            this.ApprovedUndertime += mergee.ApprovedUndertime;
            this.ActualOvertime += mergee.ActualOvertime;
            this.ApprovedOvertime += mergee.ApprovedOvertime;
            this.ActualPreOvertime += mergee.ActualPreOvertime;
            this.ApprovedPreOvertime += mergee.ApprovedPreOvertime;
            this.ActualPostOvertime += mergee.ActualPostOvertime;
            this.ApprovedPostOvertime += mergee.ApprovedPostOvertime;
            this.NightDifferential += mergee.NightDifferential;
            this.NightDifferentialOt += mergee.NightDifferentialOt;
            this.ActualRestDay += mergee.ActualRestDay;
            this.ApprovedRestDay += mergee.ApprovedRestDay;
            this.ActualRestDayOt += mergee.ActualRestDayOt;
            this.ApprovedRestDayOt += mergee.ApprovedRestDayOt;
            this.ActualNDRD += mergee.ActualNDRD;
            this.ApprovedNDRD += mergee.ApprovedNDRD;
            this.ActualNDRDot += mergee.ActualNDRDot;
            this.ApprovedNDRDot += mergee.ApprovedNDRDot;
            this.ActualLegHol += mergee.ActualLegHol;
            this.ApprovedLegHol += mergee.ApprovedLegHol;
            this.ActualLegHolOt += mergee.ActualLegHolOt;
            this.ApprovedLegHolOt += mergee.ApprovedLegHolOt;
            this.ActualSpeHol += mergee.ActualSpeHol;
            this.ApprovedSpeHol += mergee.ApprovedSpeHol;
            this.ActualSpeHolOt += mergee.ActualSpeHolOt;
            this.ApprovedSpeHolOt += mergee.ApprovedSpeHolOt;
            this.ActualLegSpeHol += mergee.ActualLegSpeHol;
            this.ApprovedLegSpeHol += mergee.ApprovedLegSpeHol;
            this.ActualLegSpeHolOt += mergee.ActualLegSpeHolOt;
            this.ApprovedLegSpeHolOt += mergee.ApprovedLegSpeHolOt;
            this.ActualLegHolRD += mergee.ActualLegHolRD;
            this.ApprovedLegHolRD += mergee.ApprovedLegHolRD;
            this.ActualLegHolRDot += mergee.ActualLegHolRDot;
            this.ApprovedLegHolRDot += mergee.ApprovedLegHolRDot;
            this.ActualSpeHolRD += mergee.ActualSpeHolRD;
            this.ApprovedSpeHolRD += mergee.ApprovedSpeHolRD;
            this.ActualSpeHolRDot += mergee.ActualSpeHolRDot;
            this.ApprovedSpeHolRDot += mergee.ApprovedSpeHolRDot;
            this.ActualLegSpeHolRD += mergee.ActualLegSpeHolRD;
            this.ApprovedLegSpeHolRD += mergee.ApprovedLegSpeHolRD;
            this.ActualLegSpeHolRDot += mergee.ActualLegSpeHolRDot;
            this.ApprovedLegSpeHolRDot += mergee.ApprovedLegSpeHolRDot;
            this.ActualNDLegHol += mergee.ActualNDLegHol;
            this.ApprovedNDLegHol += mergee.ApprovedNDLegHol;
            this.ActualNDLegHolOt += mergee.ActualNDLegHolOt;
            this.ApprovedNDLegHolOt += mergee.ApprovedNDLegHolOt;
            this.ActualNDSpeHol += mergee.ActualNDSpeHol;
            this.ApprovedNDSpeHol += mergee.ApprovedNDSpeHol;
            this.ActualNDSpeHolOt += mergee.ActualNDSpeHolOt;
            this.ApprovedNDSpeHolOt += mergee.ApprovedNDSpeHolOt;
            this.ActualNDLegSpeHol += mergee.ActualNDLegSpeHol;
            this.ApprovedNDLegSpeHol += mergee.ApprovedNDLegSpeHol;
            this.ActualNDLegSpeHolOt += mergee.ActualNDLegSpeHolOt;
            this.ApprovedNDLegSpeHolOt += mergee.ApprovedNDLegSpeHolOt;
            this.ActualNDLegHolRD += mergee.ActualNDLegHolRD;
            this.ApprovedNDLegHolRD += mergee.ApprovedNDLegHolRD;
            this.ActualNDLegHolRDot += mergee.ActualNDLegHolRDot;
            this.ApprovedNDLegHolRDot += mergee.ApprovedNDLegHolRDot;
            this.ActualNDSpeHolRD += mergee.ActualNDSpeHolRD;
            this.ApprovedNDSpeHolRD += mergee.ApprovedNDSpeHolRD;
            this.ActualNDSpeHolRDot += mergee.ActualNDSpeHolRDot;
            this.ApprovedNDSpeHolRDot += mergee.ApprovedNDSpeHolRDot;
            this.ActualNDLegSpeHolRD += mergee.ActualNDLegSpeHolRD;
            this.ApprovedNDLegSpeHolRD += mergee.ApprovedNDLegSpeHolRD;
            this.ActualNDLegSpeHolRDot += mergee.ActualNDLegSpeHolRDot;
            this.ApprovedNDLegSpeHolRDot += mergee.ApprovedNDLegSpeHolRDot;
        }

        private void ResetRegularHours()
        {
            RegularWorkHours = 0;
            ActualOvertime = 0;
            ApprovedOvertime = 0;
            NightDifferential = 0;
            NightDifferentialOt = 0;
        }

        private void ResetRestDayHours()
        {
            ActualRestDay = 0;
            ApprovedRestDay = 0;
            ActualRestDayOt = 0;
            ApprovedRestDayOt = 0;
            ActualNDRD = 0;
            ApprovedNDRD = 0;
            ActualNDRDot = 0;
            ApprovedNDRDot = 0;
        }

        public void RemapWorkHours(bool isLegalHoliday, bool isSpecialHoliday)
        {
            #region Holiday Mapping

            #region Legal and Special Holiday
            if (isLegalHoliday && isSpecialHoliday)
            {
                if (ActualRestDay > 0) //if rest day
                {
                    ActualLegSpeHolRD += ActualRestDay;
                    ApprovedLegSpeHolRD += ApprovedRestDay;
                    ActualLegSpeHolRDot += ActualRestDayOt;
                    ApprovedLegSpeHolRDot += ApprovedRestDayOt;
                    ActualNDLegSpeHolRD += ActualNDRD;
                    ApprovedNDLegSpeHolRD += ApprovedNDRD;
                    ActualNDLegSpeHolRDot += ActualNDRDot;
                    ApprovedNDLegSpeHolRDot += ApprovedNDRDot;
                    ResetRestDayHours();
                }
                else 
                {
                    ActualLegSpeHol += RegularWorkHours;
                    //ApprovedLegSpeHol
                    ActualLegSpeHolOt += ActualOvertime;
                    ApprovedLegSpeHolOt += ApprovedOvertime;
                    ActualNDLegSpeHol += NightDifferential;
                    //ApprovedNDLegSpeHoldRD
                    ActualNDLegSpeHolOt += NightDifferentialOt;
                    //ApprovedNDLegSpeHolOt
                    ResetRegularHours();
                }
            }
            #endregion

            #region Legal Holiday
            else if (isLegalHoliday)
            {
                if (ActualRestDay > 0)
                {
                    ActualLegHolRD += ActualRestDay;
                    ApprovedLegHolRD += ApprovedRestDay;
                    ActualLegHolRDot += ActualRestDayOt;
                    ApprovedLegHolRDot += ApprovedRestDayOt;
                    ActualNDLegHolRD += ActualNDRD;
                    ApprovedNDLegHolRD += ApprovedNDRD;
                    ActualNDLegHolRDot += ActualNDRDot;
                    ApprovedNDLegHolRDot += ApprovedNDRDot;
                    ResetRestDayHours();
                }
                else 
                {
                    ActualLegHol += RegularWorkHours;
                    //ApprovedLegHol
                    ActualLegHolOt += ActualOvertime;
                    ApprovedLegHolOt += ApprovedOvertime;
                    ActualNDLegHol += NightDifferential;
                    //ApprovedNDLegHol
                    ActualNDLegHolOt += NightDifferentialOt;
                    //ApprovedNDLegHolOt
                    ResetRegularHours();
                }
            }
            #endregion

            #region Special Holiday
            else if (isSpecialHoliday)
            {
                if (ActualRestDay > 0)
                {
                    ActualSpeHolRD += ActualRestDay;
                    ApprovedSpeHolRD += ApprovedRestDay;
                    ActualSpeHolRDot += ActualRestDayOt;
                    ApprovedSpeHolRDot += ApprovedRestDayOt;
                    ActualNDSpeHolRD += ActualNDRD;
                    ApprovedNDSpeHolRD += ApprovedNDRD;
                    ActualNDSpeHolRDot += ActualNDRDot;
                    ApprovedNDSpeHolRDot += ApprovedNDRDot;
                    ResetRestDayHours();
                }
                else
                {
                    ActualSpeHol += RegularWorkHours;
                    //ApprovedSpeHol
                    ActualSpeHolOt += ActualOvertime;
                    ApprovedSpeHolOt += ApprovedOvertime;
                    ActualNDSpeHol += NightDifferential;
                    //ApprovedNDSpeHol
                    ActualNDSpeHolOt += NightDifferentialOt;
                    //ApprovedNDSpeHolOt
                    ResetRegularHours();
                }

            }
            #endregion

            #endregion
        }
    }
}
