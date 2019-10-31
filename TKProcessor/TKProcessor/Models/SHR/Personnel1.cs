using System;
using System.Collections.Generic;

namespace TKProcessor.Models.SHR
{
    public partial class Personnel1
    {
        public string CompanyNum { get; set; }
        public string EmployeeNum { get; set; }
        public string ShiftType { get; set; }
        public string Location { get; set; }
        public float? NumberOfHours { get; set; }
        public float? DaysPerWeek { get; set; }
        public string SalaryIncGridCode { get; set; }
        public string SkillCategory { get; set; }
        public DateTime? BlackListDate { get; set; }
        public string BlacklistComments { get; set; }
        public string JobGradeBand { get; set; }
        public byte? BlackListed { get; set; }
        public string EthnicBackground { get; set; }
        public string Idtype { get; set; }
        public int? Breadwinner { get; set; }
        public string TownOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string SpouseSurname { get; set; }
        public string SpouseTitle { get; set; }
        public DateTime? MarriageDate { get; set; }
        public string MarriageContract { get; set; }
        public DateTime? SpouseDob { get; set; }
        public string SpouseMaidenName { get; set; }
        public string SpouseId { get; set; }
        public string SpousePassport { get; set; }
        public string IndividualJobTitle { get; set; }
        public string GradeBand { get; set; }
        public string ResignationReason { get; set; }
        public DateTime? DateNoticeHandedIn { get; set; }
        public DateTime? LastWorkingDay { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public string WithdrawalReason { get; set; }
        public DateTime? PoliceClearanceChkDate { get; set; }
        public string PoliceClearanceRemarks { get; set; }
        public DateTime? CreditCheckDate { get; set; }
        public string CreditCheckRemarks { get; set; }
        public string OtherCheck { get; set; }
        public string OtherCheckRemarks { get; set; }
        public string Position { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public float? FullTimeEquivalent { get; set; }
        public string JobFunction { get; set; }
        public string Ofocode { get; set; }
        public string Ofooccupation { get; set; }
        public string Adusername { get; set; }
        public string LocationCategory { get; set; }
        public string Citizenship { get; set; }
        public string DepartmentSection { get; set; }
        public string AccountCurrency { get; set; }
        public bool? DoEmploy { get; set; }
        public string TerminationRemarks { get; set; }
        public string TaxState { get; set; }
        public string TaxCountryCode { get; set; }
    }
}
