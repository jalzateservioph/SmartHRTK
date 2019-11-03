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
    }
}
