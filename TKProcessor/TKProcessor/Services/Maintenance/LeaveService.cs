using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using SHR = TKProcessor.Models.SHR;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.Services.Maintenance
{
    public class LeaveService : TKService<TK.Leave>
    {
        readonly SHRContext sHRContext;
        readonly TKContext tKContext;

        public LeaveService() : base()
        {
            sHRContext = new SHRContext();
            tKContext = new TKContext();
        }

        public LeaveService(Guid userId) : base(userId)
        {
            sHRContext = new SHRContext();
            tKContext = new TKContext();
        }

        public void Sync(Action<TK.Leave> iterationCallback = null)
        {
            foreach (TK.Leave leave in Context.Leave.ToList())
                DeleteHard(leave);

            List<SHR.Leave> SHRLeaves = sHRContext.Leave.Where(i => i.LeaveStatus.ToUpper() == "HR GRANTED").ToList();

            foreach (SHR.Leave leave in SHRLeaves)
            {
                TK.Leave tkLeave = new TK.Leave()
                {
                    EmployeeCode = leave.EmployeeNum,
                    LeaveDate = leave.StartDate,
                    LeaveType = leave.LeaveType,
                    LeaveHours = (decimal) leave.Duration
                };

                Save(tkLeave);

                iterationCallback?.Invoke(tkLeave);
            }
        }

        public IEnumerable<TK.Leave> GetLeaves(string employeeCode, DateTime date)
        {
            return tKContext.Leave.Where(l => l.EmployeeCode == employeeCode && l.LeaveDate == date.Date);
        }
    }
}
