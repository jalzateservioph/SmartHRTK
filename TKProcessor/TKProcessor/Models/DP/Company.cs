using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class Company
    {
        public Guid SeqId { get; set; }
        public string CountryId { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencySymbol { get; set; }
        public short CurrentTaxYear { get; set; }
        public Guid? DefaultCompanyAttributeTemplateSeqId { get; set; }
        public Guid? DefaultEmployeeAttributeTemplateSeqId { get; set; }
        public Guid? DefaultPayrollTrxTemplateSeqId { get; set; }
        public short DefaultPageSize { get; set; }
        public long NextPayrollTrxNo { get; set; }
        public long NextPayRunNo { get; set; }
        public long NextPayslipNo { get; set; }
        public bool? NextPayslipNoFixed { get; set; }
        public long NextPaymentTrxNo { get; set; }
        public long NextReversalTrxNo { get; set; }
        public bool? CreateGljournalsSibilPr { get; set; }
        public bool? CreateGljournalsErpPr { get; set; }
        public bool? ContinueAfterVoidingPayment { get; set; }
        public string ErpgljournalTemplateIdPr { get; set; }
        public string ErpgljournalPrefixPr { get; set; }
        public string ErpgljournalRefPr { get; set; }
        public bool? CreateGljournalsSibilPymt { get; set; }
        public bool? CreateGljournalsErpPymt { get; set; }
        public string ErpgljournalTemplateIdPymt { get; set; }
        public string ErpgljournalPrefixPymt { get; set; }
        public string ErpgljournalRefPymt { get; set; }
        public bool? CreatePymtJournalsErp { get; set; }
        public string ErppymtJournalTemplateId { get; set; }
        public string ErppymtJournalPrefix { get; set; }
        public string ErppymtJournalRef { get; set; }
        public short? UpcomingDefaultDays { get; set; }
        public bool? UpcomingCompanyAttributesEff { get; set; }
        public bool? UpcomingConstantEff { get; set; }
        public bool? UpcomingTableLookupEff { get; set; }
        public bool? UpcomingPayrollCodeEff { get; set; }
        public bool? UpcomingPayrollCodeTaxCodeEff { get; set; }
        public bool? UpcomingPayrollTableEff { get; set; }
        public bool? UpcomingReportsEff { get; set; }
        public bool? UpcomingPropertyMapEff { get; set; }
        public bool? UpcomingCoinageEff { get; set; }
        public bool? UpcomingGlpostingSetupEff { get; set; }
        public bool? UpcomingPayPackageEff { get; set; }
        public DateTime? EarliestEffectiveDateAllowed { get; set; }
        public bool? UseMultitelSms { get; set; }
        public string Smslocation { get; set; }
        public bool? Multicurrency { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }
        public bool? RecreatePayrollTrx { get; set; }
        public bool? AutoIncrementEmpCode { get; set; }
        public string NextEmployeeCode { get; set; }
        public bool? ValidateEmployeeTaxId { get; set; }
        public byte DefaultEmployeePassword { get; set; }
        public byte? PasswordFieldFormat { get; set; }
    }
}
