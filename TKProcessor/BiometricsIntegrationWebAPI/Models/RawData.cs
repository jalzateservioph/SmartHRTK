using System;

namespace BiometricsIntegrationWebAPI.Models
{
    public class RawData
    {
        public Guid Id { get; set; }
        public string EmployeeBiometricsID { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public int TransactionType { get; set; }
    }
}
