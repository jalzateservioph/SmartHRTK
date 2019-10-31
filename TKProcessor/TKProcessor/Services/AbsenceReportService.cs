using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;

namespace TKProcessor.Services
{
    public class AbsenceReportService : IReportService<AbsenceReportModel>
    {
        readonly TKContext context;

        public AbsenceReportService()
        {
            context = new TKContext();
        }

        public IEnumerable<AbsenceReportModel> GetData()
        {
            List<AbsenceReportModel> dataSet = new List<AbsenceReportModel>();

            var dtr = context.DailyTransactionRecord
                                .Include(i => i.Employee)
                                .Include(i => i.Shift)
                                .Where(i=>i.AbsentHours > 0);

            foreach(var item in dtr)
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