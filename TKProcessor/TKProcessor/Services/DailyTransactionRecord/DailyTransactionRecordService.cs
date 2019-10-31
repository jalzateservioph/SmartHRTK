using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Models.DP;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class DailyTransactionRecordService : TKService<DailyTransactionRecord>
    {
        private readonly DPContext dPContext;

        IDTRProcessor processor;
        HolidayService holidayService;
        LeaveService leaveService;

        public DailyTransactionRecordService(Guid userId) : base(userId)
        {
            dPContext = new DPContext();
            holidayService = new HolidayService();
            leaveService = new LeaveService();
        }

        public IEnumerable<DailyTransactionRecord> List(DateTime start, DateTime end, string payrollCode)
        {
            try
            {
                start = start.GetStartOfDay();
                end = end.GetStartOfDay();

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (string.IsNullOrEmpty(payrollCode))
                    throw new ArgumentNullException("Payroll code cannot be empty");


                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.GetStartOfDay() >= start && i.ScheduleDate.GetStartOfDay() <= end)
                                            .ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Select(i => i.Employee).Where(i => i.JobGradeBand == payrollCode).ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with payroll code {payrollCode}");

                return Context.DailyTransactionRecord.Include(i => i.Employee).Include(i => i.Shift)
                                .Where(i => i.TransactionDate.Value.GetStartOfDay() >= start && i.TransactionDate.Value.GetStartOfDay() <= end && employees.Any(emp => emp == i.Employee));
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Process(DateTime start, DateTime end, string payrollCode)
        {
            try
            {
                start = start.GetStartOfDay();
                end = end.GetStartOfDay();

                if (start >= end)
                    throw new Exception("Start date should not be greater than the end date");

                if (string.IsNullOrEmpty(payrollCode))
                    throw new ArgumentNullException("Payroll code cannot be empty");


                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.GetStartOfDay() >= start && i.ScheduleDate.GetStartOfDay() <= end).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Select(i => i.Employee).Where(i => i.JobGradeBand == payrollCode).Distinct().ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with payroll code {payrollCode}");

                var rawdata = Context.RawData.Where(i => i.ScheduleDate >= start && i.ScheduleDate.Value.GetStartOfDay() <= end &&
                                                            employees.Any(emp => emp.BiometricsId == i.BiometricsId)).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No Raw Data was found");

                while (start <= end)
                {
                    foreach (var employee in employees)
                    {
                        try
                        {
                            var existing = Context.DailyTransactionRecord.Where(i => i.Employee == employee && i.TransactionDate.Value.GetStartOfDay() == start);

                            foreach (var item in existing)
                            {
                                Context.DailyTransactionRecord.Remove(item);
                            }

                            var shift = workschedules.FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == start);

                            var timein = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                                        (i.ScheduleDate.HasValue && i.ScheduleDate.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                                        i.TransactionType == (int)TransactionType.TimeIn);

                            var timeout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                                        (i.ScheduleDate.HasValue && i.ScheduleDate.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                                        i.TransactionType == (int)TransactionType.TimeOut);
                            //if (timeout == null)
                            //{
                            //    timeout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                            //                                            (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay().AddDays(1)) &&
                            //                                            i.TransactionType == (int) TransactionType.TimeOut);
                            //}

                            var ambreakin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                                        (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                                        i.TransactionType == (int)TransactionType.AMBreakIn);

                            var ambreakout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.AMBreakOut);

                            var lunchin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.LunchIn);
                            var lunchout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.LunchOut);
                            var pmbreakin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.PMBreakIn);
                            var pmbreakout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.PMBreakOut);
                            var dinnerin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.DinnerIn);
                            var dinnerout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.GetStartOfDay() == start.GetStartOfDay()) &&
                                                 i.TransactionType == (int)TransactionType.DinnerOut);

                            DailyTransactionRecord DTR = new DailyTransactionRecord()
                            {
                                Employee = employee,
                                TransactionDate = start,
                                Shift = workschedules.FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == start)?.Shift,
                                TimeIn = timein?.TransactionDateTime,
                                TimeOut = timeout?.TransactionDateTime,
                            };

                            IEnumerable<Holiday> holidays = null;
                            IEnumerable<Leave> leaves = null;
                            if (DTR.Shift?.FocusDate.Value == (int)FocusDate.ScheduleIn)
                            {
                                holidays = holidayService.GetHolidays(DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                                leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                            }
                            else if (DTR.Shift?.FocusDate.Value == (int)FocusDate.ScheduleOut)
                            {
                                holidays = holidayService.GetHolidays(DTR.TimeOut ?? DTR.Shift.ScheduleOut.Value);
                                leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                            }

                            if (DTR.Shift?.ShiftType == (int)ShiftType.Standard)
                            {
                                if (holidays.Count() > 0 || DTR.Shift.IsRestDay.Value)
                                {
                                    processor = new StandardDTRProcessor(DTR, leaves, holidays);
                                    processor.ComputeHolidayAndRestDay();
                                    DTR = processor.DTR;
                                }
                                else
                                {
                                    processor = new StandardDTRProcessor(DTR, leaves);
                                    processor.ComputeRegular();
                                    DTR = processor.DTR;
                                }
                            }
                            else if (DTR.Shift?.ShiftType == (int)ShiftType.Flex)
                            {
                                if (holidays.Count() > 0 || DTR.Shift.IsRestDay.Value)
                                {
                                    processor = new FlextimeDTRProcessor(DTR, leaves, holidays);
                                    processor.ComputeHolidayAndRestDay();
                                    DTR = processor.DTR;
                                }
                                else
                                {
                                    processor = new FlextimeDTRProcessor(DTR, leaves);
                                    processor.ComputeRegular();
                                    DTR = processor.DTR;
                                }
                            }

                            Save(DTR);
                        }
                        catch (Exception ex)
                        {
                            Context.ErrorLog.Add(new ErrorLog(ex));
                            Context.SaveChanges();

                            throw ex;
                        }
                    }

                    start = start.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Export(DateTime start, DateTime end, DateTime payOutDate, string payrollCode)
        {
            try
            {
                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (end > payOutDate)
                    throw new Exception("Payout date cannot be less than the set end date");

                if (string.IsNullOrEmpty(payrollCode))
                    throw new ArgumentNullException("Payroll code cannot be empty");

                GlobalSetting globalSettings = Context.GlobalSetting.Include(i => i.PayPackageMappings).Include(i => i.PayrollCodeMappings).FirstOrDefault();

                if (globalSettings == default(GlobalSetting) ||
                    globalSettings.PayPackageMappings.Count == 0 ||
                    globalSettings.PayrollCodeMappings.Count == 0)
                {
                    throw new Exception("Please setup mappings in Global Settings first");
                }

                var dtrGroups = List().Where(i => i.Employee.JobGradeBand == payrollCode)
                                        .Where(i => i.TransactionDate >= start && i.TransactionDate <= end)
                                        .ToList().GroupBy(i => i.Employee);

                if (dtrGroups.Count() == 0)
                    throw new Exception("No DTR records has been found");

                var payPackage = dPContext.PayPackage.First(i => i.Code == globalSettings.PayPackageMappings.First(ii => ii.Target == payrollCode).Source);
                var payFreqCalendar = dPContext.PayPackagePayFreqCalendars
                                                .Include(i => i.PayPackageSeq)
                                                .Include(i => i.PayFreqCalendarSeq)
                                                .First(i => i.PayPackageSeqId == payPackage.SeqId).PayFreqCalendarSeq;

                var maxTrxNo = dPContext.Company.First().NextPayrollTrxNo++;

                PayrollTrx trx = new PayrollTrx()
                {
                    CountryId = dPContext.Country.FirstOrDefault()?.CountryId,
                    Type = 1,
                    TrxNo = maxTrxNo,
                    Label = $"Imported from SmartHR Timekeeping {DateTime.Now.ToShortDateString()}",
                    Description = $"Imported from SmartHR Timekeeping {DateTime.Now.ToShortDateString()}",
                    RefDate = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = Guid.Empty
                };


                foreach (var group in dtrGroups)
                {
                    short displayOrder = 0;

                    foreach (var mapping in globalSettings.PayrollCodeMappings.Where(i => !string.IsNullOrEmpty(i.Source)))
                    {
                        decimal fieldValue = 0;

                        foreach (var groupItem in group)
                        {
                            fieldValue += (decimal)typeof(DailyTransactionRecord).GetProperty(mapping.Target).GetValue(groupItem);
                        }

                        // do not export to DP if value is 0
                        if (fieldValue == 0)
                            continue;

                        var line = new PayrollTrxLines()
                        {
                            DisplayOrder = ++displayOrder,
                            EmployeeCode = group.Key.EmployeeCode,
                            LineDate = payOutDate,
                            PayPackageCode = payPackage.Code,
                            PayFreqCalendarCode = payFreqCalendar.Code,
                            PayrollCodeCode = mapping.Source,
                            AttributeCode = "PHNumHours",
                            InputValue = fieldValue.ToString(),
                            PostingAction = 0,
                            InstanceCount = 0,
                            CreatedOn = DateTime.Now,
                            CreatedBy = Guid.Empty,
                            ModifiedOn = DateTime.Now,
                            ModifiedBy = Guid.Empty
                        };

                        trx.PayrollTrxLines.Add(line);
                    }
                }

                dPContext.PayrollTrx.Add(trx);

                dPContext.SaveChanges();
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }
    }
}
