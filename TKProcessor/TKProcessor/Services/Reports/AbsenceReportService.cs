using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Reports
{
    public class AbsenceReportService : ErrorLogBase, IReportService<AbsenceReportModel>
    {
        public IEnumerable<AbsenceReportModel> GetData()
        {
            List<AbsenceReportModel> dataSet = new List<AbsenceReportModel>();

            using (TKContext tkcontext = new TKContext())
            {
                try
                {
                    var dtr = tkcontext.DailyTransactionRecord.Include(i => i.Employee)
                                                              .Include(i => i.Shift)
                                                              .Where(i => i.AbsentHours > 0);

                    // filter using parameters
                    dtr = dtr.Where(i => Employees.Any(emp => emp.Id == i.Employee.Id) &&
                                         Shifts.Any(emp => emp.Id == i.Shift.Id) && 
                                         i.TransactionDate >= StartDate &&
                                         i.TransactionDate <= EndDate);

                    foreach (var item in dtr)
                    {
                        dataSet.Add(new AbsenceReportModel()
                        {
                            AbsentHours = (decimal)item.Shift.RequiredWorkHours,
                            Date = item.TransactionDate.Value,
                            EmployeeName = item.Employee.FullName,
                            EmployeeNumber = item.Employee.EmployeeCode,
                            Shift = item.Shift.ShiftCode
                        });
                    }

                    return dataSet;
                }
                catch(Exception ex)
                {
                    CreateErrorLog(ex);

                    throw ex;
                }
            }
        }


        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Shift> Shifts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AbsenceReportModel
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Shift { get; set; }
        public DateTime Date { get; set; }
        public decimal AbsentHours { get; set; }
    }


}