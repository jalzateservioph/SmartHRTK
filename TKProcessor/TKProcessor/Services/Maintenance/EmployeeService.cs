using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.SHR;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Maintenance
{
    public class EmployeeService : TKService<Employee>
    {
        public EmployeeService(Guid userId) : base(userId)
        {

        }

        public override IEnumerable<Employee> List()
        {
            return base.List();
        }

        public override void Save(Employee entity)
        {
            try
            {
                var existing = Context.Employee.FirstOrDefault(i => i.EmployeeCode == entity.EmployeeCode);

                if (existing != default(Employee))
                    entity.Id = existing.Id;

                base.Save(entity);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Sync(Guid userId)
        {
            try
            {
                int successCount = 0;

                using (var hRContext = new SHRContext())
                {
                    var empList = (from p in hRContext.Personnel
                                   join p1 in hRContext.Personnel1 on p.EmployeeNum equals p1.EmployeeNum
                                   select new Employee()
                                   {
                                       Id = Guid.NewGuid(),
                                       EmployeeCode = p.EmployeeNum,
                                       Password = p.EmployeeNum,
                                       FullName = p.FirstName + (string.IsNullOrEmpty(p.MiddleName) ? " " : " " + p.MiddleName + " ") + p.Surname,
                                       BiometricsId = int.Parse(p.PayrollEmployeeNum.Trim()).ToString(),
                                       TerminationDate = p.TerminationDate,
                                       JobGradeBand = p1.JobGradeBand
                                   });

                    foreach (var emp in empList)
                    {
                        try
                        {
                            Save(emp);

                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    Context.AuditLog.Add(new AuditLog() { Target = nameof(Employee), Action = $"Sync {successCount} employees." });

                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }
    }
}
