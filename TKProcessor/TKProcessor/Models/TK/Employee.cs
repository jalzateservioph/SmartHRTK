using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class Employee : IEntity, IComparable<Employee>
    {
        public Employee()
        {
            Id = Guid.NewGuid();
        }

        public int CompareTo(Employee other)
        {
            return EmployeeCode.CompareTo(other.EmployeeCode);
        }

        public override string ToString()
        {
            return $"{EmployeeCode} - {FullName}";
        }

        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public string EmployeeCode { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string BiometricsId { get; set; }
        public string JobGradeBand { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
