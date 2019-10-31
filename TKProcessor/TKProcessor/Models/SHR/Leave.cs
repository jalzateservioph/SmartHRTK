using System;
using System.Collections.Generic;

namespace TKProcessor.Models.SHR
{
    public partial class Leave
    {
        public string CompanyNum { get; set; }
        public string EmployeeNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CaptureDate { get; set; }
        public double? Duration { get; set; }
        public string UnitOfMeassure { get; set; }
        public string LeaveType { get; set; }
        public bool? MedicalCert { get; set; }
        public string Doctor { get; set; }
        public string LeaveStatus { get; set; }
        public bool? EmailSent { get; set; }
        public string CapturedByUsername { get; set; }
        public string DeclineReason { get; set; }
        public long? PathId { get; set; }
        public bool? DocCert { get; set; }
        public string TemplateCode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public int LeaveSeq { get; set; }
    }
}
