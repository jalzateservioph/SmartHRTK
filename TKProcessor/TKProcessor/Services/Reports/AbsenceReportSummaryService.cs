using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.SHR;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Reports
{
    public class AbsenceReportSummaryService : IReportService<AbsenceReportSummaryModel>
    {


        public IEnumerable<AbsenceReportSummaryModel> GetData()
        {
            List<AbsenceReportSummaryModel> dataSet = new List<AbsenceReportSummaryModel>();

            TKContext tkcontext = new TKContext();
            SHRContext shrcontext = new SHRContext();

            try
            {
                var dtr = tkcontext.DailyTransactionRecord.Include(i => i.Employee)
                                                          .Include(i => i.Shift)
                                                          .Where(i => i.AbsentHours > 0);

                // filter using parameters
                dtr = dtr.Where(i => i.TransactionDate >= StartDate &&
                                     i.TransactionDate <= EndDate &&
                                     i.Shift.Id == Shift.Id);

                var personnelNums = shrcontext.Personnel.Where(p => Departments.Any(dept => dept == p.DeptName))
                                                        .Select(p => p.EmployeeNum);

                foreach (var item in dtr.Where(i => personnelNums.Any(ii => ii == i.Employee.EmployeeCode)))
                {

                }

                IQueryable<Personnel> personnel = shrcontext.Personnel;

                var q = (from d in dtr
                         join p in personnel on d.Employee.EmployeeCode equals p.EmployeeNum
                         where Departments.Any(d => d == p.DeptName)
                         select new
                         {
                             Department = p.DeptName,
                             Shift = d.Shift.ToString(),
                             Hours = d.AbsentHours,
                         }).GroupBy(i => i.Department);

                foreach (var group in q)
                {
                    foreach (var shiftGrouping in group.GroupBy(i => i.Shift))
                    {
                        decimal absentHours = 0;

                        foreach (var item in shiftGrouping)
                        {
                            absentHours += item.Hours;
                        }

                        dataSet.Add(new AbsenceReportSummaryModel()
                        {
                            Department = group.Key,
                            Shift = shiftGrouping.Key,
                            AbsentHours = absentHours
                        });
                    }
                }

                return dataSet;
            }
            finally
            {
                tkcontext.Dispose();
                shrcontext.Dispose();
                dataSet = null;
            }
        }

        public IEnumerable<string> Departments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Shift Shift { get; set; }
    }

    public class AbsenceReportSummaryModel
    {
        public string Department { get; set; }
        public string Shift { get; set; }
        public decimal AbsentHours { get; set; }
    }
}
