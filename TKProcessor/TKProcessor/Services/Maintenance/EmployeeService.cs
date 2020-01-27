using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.SHR;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Maintenance
{
    public class EmployeeService : TimekeepingService<Employee>
    {
        public EmployeeService(Guid userId) : base(userId)
        {

        }

        public EmployeeService(Guid userId, TKContext context) : base(context, userId)
        {

        }


        public override IQueryable<Employee> List()
        {
            return context.Employee.Include(ews => ews.EmployeeWorkSites).ThenInclude(i => i.WorkSite); //Need to include relationship before making it to a list
        }

        public override void Add(Employee entity)
        {
            try
            {
                base.Add(entity);

                entity.EmployeeWorkSites = new List<EmployeeWorkSite>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Update(Guid id, Employee entity)
        {
            try
            {
                base.Update(id, entity);

                if (entity.EmployeeWorkSites != null)
                {
                    foreach (var existingChild in context.EmployeeWorkSite.Where(ews => ews.EmployeeId == id))
                    {
                        context.EmployeeWorkSite.Remove(existingChild);
                    }

                    foreach (var ews in entity.EmployeeWorkSites)
                    {
                        ews.Employee = context.Employee.Find(ews.Employee.Id);
                        ews.WorkSite = context.WorkSite.Find(ews.WorkSite.Id);

                        context.EmployeeWorkSite.Add(ews);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Save(Employee entity)
        {
            try
            {
                var existing = context.Employee.FirstOrDefault(i => i.EmployeeCode == entity.EmployeeCode);

                if (existing == null)
                    Add(entity);
                else
                    Update(existing.Id, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Sync(Action<Employee> iterationCallback = null)
        {
            try
            {
                int successCount = 0;

                int tempEmpNum;

                using (var hRContext = new SHRContext())
                {
                    var empList = (from p in hRContext.Personnel
                                   join p1 in hRContext.Personnel1 on p.EmployeeNum equals p1.EmployeeNum
                                   select new Employee()
                                   {
                                       EmployeeCode = p.EmployeeNum,
                                       Password = p.EmployeeNum,
                                       FullName = p.FirstName + (string.IsNullOrEmpty(p.MiddleName) ? " " : " " + p.MiddleName + " ") + p.Surname,
                                       BiometricsId = int.Parse((string.IsNullOrEmpty(p.PayrollEmployeeNum) ? (int.TryParse(p.EmployeeNum, out tempEmpNum) ? p.EmployeeNum : "") : p.PayrollEmployeeNum).Trim()).ToString(),
                                       TerminationDate = p.TerminationDate,
                                       JobGradeBand = p1.JobGradeBand,
                                       Department = p.DeptName
                                   });

                    foreach (var emp in empList)
                    {
                        try
                        {
                            Save(emp);

                            iterationCallback?.Invoke(emp);

                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
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
