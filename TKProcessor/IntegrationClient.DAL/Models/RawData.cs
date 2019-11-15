using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationClient.DAL.Models
{
    public class RawData
    {
        public string EmployeeBiometricsID { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public int TransactionType { get; set; }
    }
}
