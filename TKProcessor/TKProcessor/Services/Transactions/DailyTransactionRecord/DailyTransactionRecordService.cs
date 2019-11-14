using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Models;
using TKProcessor.Models.DP;
using TKProcessor.Models.TK;
using TKProcessor.Services.Maintenance;
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

        public IEnumerable<DailyTransactionRecord> List(DateTime start, DateTime end, string jobGradeBand)
        {
            try
            {
                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (string.IsNullOrEmpty(jobGradeBand))
                    throw new ArgumentNullException("Job Grade Band cannot be empty");


                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.Date >= start && i.ScheduleDate.Date <= end)
                                            .ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Select(i => i.Employee).Where(i => i.JobGradeBand == jobGradeBand).ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with Job Grade Band {jobGradeBand}");

                return Context.DailyTransactionRecord.Include(i => i.Employee).Include(i => i.Shift)
                                                     .Where(i => i.TransactionDate.Value.Date >= start &&
                                                                 i.TransactionDate.Value.Date <= end &&
                                                                 employees.Any(emp => emp == i.Employee));
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Process(DateTime start, DateTime end, string jobGradeBand, Action<string> iterationCallback = null)
        {
            try
            {
                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (string.IsNullOrEmpty(jobGradeBand))
                    throw new ArgumentNullException("Job Grade Band cannot be empty");


                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.Date >= start && i.ScheduleDate.Date <= end).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Select(i => i.Employee).Where(i => i.JobGradeBand == jobGradeBand).Distinct().ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with payroll code {jobGradeBand}");

                var rawdata = Context.RawData.Where(i => i.ScheduleDate >= start && i.ScheduleDate.Value.Date <= end &&
                                                            employees.Any(emp => emp.BiometricsId == i.BiometricsId)).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No Raw Data was found");

                var globalSettings = Context.GlobalSetting.FirstOrDefault();

                while (start <= end)
                {
                    foreach (var employee in employees)
                    {
                        try
                        {
                            iterationCallback?.Invoke($"Processing {employee.EmployeeCode} - {employee.FullName} - {start.ToLongDateString()}...");

                            var existing = Context.DailyTransactionRecord.Where(i => i.Employee == employee && i.TransactionDate.Value.Date == start);

                            foreach (var item in existing)
                            {
                                Context.DailyTransactionRecord.Remove(item);
                            }

                            var shift = workschedules.FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == start);

                            var timein = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                                        (i.ScheduleDate.HasValue && i.ScheduleDate.Value.Date == start.Date) &&
                                                                        i.TransactionType == (int)TransactionType.TimeIn);

                            var timeout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                                        (i.ScheduleDate.HasValue && i.ScheduleDate.Value.Date == start.Date) &&
                                                                        i.TransactionType == (int)TransactionType.TimeOut);

                            if (globalSettings != null && !globalSettings.CreateDTRForNoWorkDays && timein == null && timeout == null)
                            {
                                iterationCallback?.Invoke($"Skipping {employee.EmployeeCode} - {employee.FullName} - {start.ToLongDateString()} due to No Work, No DTR Setup");
                                continue;
                            }

                            var ambreakin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                                        (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                                        i.TransactionType == (int)TransactionType.AMBreakIn);

                            var ambreakout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.AMBreakOut);

                            var lunchin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.LunchIn);
                            var lunchout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.LunchOut);
                            var pmbreakin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.PMBreakIn);
                            var pmbreakout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.PMBreakOut);
                            var dinnerin = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.DinnerIn);
                            var dinnerout = rawdata.FirstOrDefault(i => i.BiometricsId == employee.BiometricsId &&
                                                 (i.TransactionDateTime.HasValue && i.TransactionDateTime.Value.Date == start.Date) &&
                                                 i.TransactionType == (int)TransactionType.DinnerOut);

                            DailyTransactionRecord DTR = new DailyTransactionRecord()
                            {
                                Employee = employee,
                                TransactionDate = start,
                                Shift = workschedules.FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == start)?.Shift,
                                TimeIn = timein?.TransactionDateTime?.RemoveSeconds(),
                                TimeOut = timeout?.TransactionDateTime?.RemoveSeconds(),
                            };

                            if (DTR.Shift == null)
                                DTR.AddRemarks($"Employee has no work schdedule setup for this day");

                            if (DTR.TimeIn == null && DTR.TimeOut == null)
                                DTR.AddRemarks($"There is no time in and time out data for this day");

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

                            bool isLegalHoliday = false;
                            bool isSpecialHoliday = false;

                            if (holidays != null)
                            {
                                foreach (var holiday in holidays)
                                {
                                    if (holiday.Type == (int)HolidayType.Legal)
                                    {
                                        isLegalHoliday = true;
                                    }
                                    else if (holiday.Type == (int)HolidayType.Special)
                                    {
                                        isSpecialHoliday = true;
                                    }
                                }
                            }

                            if (DTR.Shift?.ShiftType == (int)ShiftType.Standard)
                            {
                                //if (holidays.Count() > 0 || DTR.Shift.IsRestDay.Value)
                                //{
                                //    processor = new StandardDTRProcessor(DTR, leaves, holidays);
                                //    processor.ComputeHolidayAndRestDay();
                                //    DTR = processor.DTR;
                                //}
                                //else
                                //{
                                //    processor = new StandardDTRProcessor(DTR, leaves);
                                //    processor.ComputeRegular();
                                //    DTR = processor.DTR;
                                //}

                                processor = new StandardDTRProcessor(DTR, leaves, holidays);
                                ((StandardDTRProcessor)processor).Compute();

                                DTR = processor.DTR;
                                DTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);
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

        public void ProcessSplit(DateTime start, DateTime end, string jobGradeBand, Action<string> iterationCallback = null)
        {
            try
            {
                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (string.IsNullOrEmpty(jobGradeBand))
                    throw new ArgumentNullException("Job Grade Band cannot be empty");


                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.Date >= start.AddDays(-1) && i.ScheduleDate.Date <= end).ToList(); //Include work schedule of day before start day... Filter split shifts


                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Select(i => i.Employee).Where(i => i.JobGradeBand == jobGradeBand).Distinct().ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with payroll code {jobGradeBand}");

                var rawdata = Context.RawData.Where(i => i.ScheduleDate >= start.AddDays(-1) && i.ScheduleDate.Value.Date <= end &&
                                                            employees.Any(emp => emp.BiometricsId == i.BiometricsId)).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No Raw Data was found");

                var globalSettings = Context.GlobalSetting.FirstOrDefault();


                foreach (var employee in employees)
                {
                    DailyTransactionRecord hangingDTR = new DailyTransactionRecord();

                    bool firstIteration = true;
                    start = start.AddDays(-1); //start from day before to compute hanging DTR
                    while (start <= end)
                    {
                        if (!firstIteration)
                        {
                            var existing = Context.DailyTransactionRecord.Where(i => i.Employee == employee && i.TransactionDate.Value.Date == start);

                            foreach (var item in existing)
                            {
                                Context.DailyTransactionRecord.Remove(item);
                            }
                        }

                        IEnumerable<Holiday> holidays = holidayService.GetHolidays(start);

                        var isLegalHoliday = false;
                        var isSpecialHoliday = false;
                        foreach (var holiday in holidays)
                        {
                            if (holiday.Type == (int)HolidayType.Legal)
                            {
                                isLegalHoliday = true;
                            }
                            else if (holiday.Type == (int)HolidayType.Special)
                            { 
                                isSpecialHoliday = true;
                            }
                        }

                        //remap DTR values based on holidays



                        DailyTransactionRecord currentDayDTR = hangingDTR; //consider holidays
                        currentDayDTR.Employee = employee;
                        currentDayDTR.TransactionDate = start;
                        //currentDayDTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);


                        hangingDTR = new DailyTransactionRecord(); //Reset hanging DTR

                        IList<RawData> rawDataTimeIn = rawdata.Where(raw => raw.ScheduleDate == start && raw.BiometricsId == employee.BiometricsId && raw.TransactionType.Value == (int)TransactionType.TimeIn).OrderBy(raw => raw.TransactionDateTime).ToList();
                        IList<RawData> rawDataTimeOut = rawdata.Where(raw => raw.ScheduleDate == start && raw.BiometricsId == employee.BiometricsId && raw.TransactionType.Value == (int)TransactionType.TimeOut).OrderBy(raw => raw.TransactionDateTime).ToList();

                        var workSchedules = workschedules.Where(ws => ws.Employee == employee && ws.ScheduleDate == start).OrderBy(ws => ws.Shift.ScheduleIn); //for standard, need to change for flex

                        foreach (var workSchedule in workSchedules)
                        {
                            DailyTransactionRecord DTR = new DailyTransactionRecord(); //DTR for this work schedule

                            DTR.Employee = employee;
                            DTR.Shift = workSchedule.Shift;
                            DTR.TransactionDate = start;

                            //standard
                            var _schedIn = start.Add(workSchedule.Shift.ScheduleIn.Value.TimeOfDay).RemoveSeconds();
                            var _schedOut = start.Add(workSchedule.Shift.ScheduleOut.Value.TimeOfDay).RemoveSeconds();

                            if (_schedOut < _schedIn)
                            {
                                _schedOut = _schedOut.AddDays(1);
                            }

                            var timeIn = rawDataTimeIn.FirstOrDefault(raw => raw.TransactionDateTime < _schedOut); //for standard, need to change for flex
                            if (timeIn != null)
                            {
                                DTR.TimeIn = timeIn.TransactionDateTime;
                                rawDataTimeIn.Remove(timeIn);
                                var timeOut = rawDataTimeOut.FirstOrDefault(raw => raw.TransactionDateTime > _schedIn);
                                if (timeOut != null)
                                {
                                    DTR.TimeOut = timeOut.TransactionDateTime;
                                    rawDataTimeOut.Remove(timeOut);
                                }
                            }

                            IEnumerable<Leave> leaves = null; //todo

                            if (DTR.Shift?.ShiftType == (int)ShiftType.Standard)
                            {
                                processor = new StandardDTRProcessor(DTR, leaves, holidays);
                                ((StandardDTRProcessor)processor).Compute(); // make changes to interface maybe.. temp solution
                                DTR = processor.DTR;
                            }
                            //else if (DTR.Shift?.ShiftType == (int)ShiftType.Flex) //todo
                            //{
                            //    if (holidays.Count() > 0 || DTR.Shift.IsRestDay.Value)
                            //    {
                            //        processor = new FlextimeDTRProcessor(DTR, leaves, holidays);
                            //        processor.ComputeHolidayAndRestDay();
                            //        DTR = processor.DTR;
                            //    }
                            //    else
                            //    {
                            //        processor = new FlextimeDTRProcessor(DTR, leaves);
                            //        processor.ComputeRegular();
                            //        DTR = processor.DTR;
                            //    }
                            //}

                            if (firstIteration)
                            {
                                firstIteration = false;

                                if (((DTRProcessorBase)processor).IsSplittable)
                                {
                                    var splitDTR = ((DTRProcessorBase)processor).Split(DTR);

                                    var tail = splitDTR.Item2; //add values to hanging DTR
                                    hangingDTR.Merge(tail);
                                }
                            }
                            else
                            {
                                if (((DTRProcessorBase)processor).IsSplittable)
                                {
                                    var splitDTR = ((DTRProcessorBase)processor).Split(DTR);

                                    var head = splitDTR.Item1; //add values to current day, consider holidays and restday
                                    var tail = splitDTR.Item2; //add values to hanging DTR

                                    currentDayDTR.Merge(head);
                                    hangingDTR.Merge(tail);
                                }
                                else
                                {
                                    //add DTR to currentDayDTR
                                    currentDayDTR.Merge(DTR);
                                }
                            }

                        }

                        currentDayDTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);
                        Save(currentDayDTR);

                        start = start.AddDays(1);
                    }
                }



            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void ExportToDP(DateTime start, DateTime end, DateTime payOutDate, string jobGradeBand)
        {
            try
            {
                start = start.Date;
                end = end.Date;
                payOutDate = payOutDate.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (end < payOutDate)
                    throw new Exception("Payout date cannot be less than the set end date");

                if (string.IsNullOrEmpty(jobGradeBand))
                    throw new ArgumentNullException("Job Grade Band cannot be empty");

                GlobalSetting globalSettings = Context.GlobalSetting.Include(i => i.PayPackageMappings)
                                                                    .Include(i => i.PayrollCodeMappings).FirstOrDefault();

                if (globalSettings == default(GlobalSetting) ||
                    globalSettings.PayPackageMappings.Count == 0 ||
                    globalSettings.PayPackageMappings.Count(i => i.Source != "") == 0 ||
                    globalSettings.PayrollCodeMappings.Count == 0 ||
                    globalSettings.PayrollCodeMappings.Count(i => i.Source != "") == 0)
                {
                    throw new Exception("Please setup mappings in Global Settings first");
                }

                var dtrGroups = List(start, end, jobGradeBand).Where(i => i.Employee.JobGradeBand == jobGradeBand)
                                                                .Where(i => i.TransactionDate >= start && i.TransactionDate <= end)
                                                                .ToList().GroupBy(i => i.Employee);

                if (dtrGroups.Count() == 0)
                    throw new Exception($"No DTR records has been found from {start.ToLongDateString()} " +
                                        $"to {end.ToLongDateString()} with Pay package code '{jobGradeBand}'");

                var payPackage = dPContext.PayPackage.First(i => i.Code == globalSettings.PayPackageMappings.First(ii => ii.Target == jobGradeBand).Source);

                if (payPackage == null)
                    throw new Exception($"Please setup pay package mapping for Job Grade Band '{jobGradeBand}' in the Settings");

                var payFreqCalendar = dPContext.PayPackagePayFreqCalendars
                                               .Include(i => i.PayPackageSeq)
                                               .Include(i => i.PayFreqCalendarSeq)
                                               .First(i => i.PayPackageSeqId == payPackage.SeqId)?.PayFreqCalendarSeq;

                if (payFreqCalendar == null)
                    throw new Exception($"Please setup Pay Frequency Calendar that corresponds to Job Grade Band '{jobGradeBand}'");

                var maxTrxNo = dPContext.Company.First().NextPayrollTrxNo++;

                PayrollTrx trx = new PayrollTrx()
                {
                    CountryId = dPContext.Country.FirstOrDefault()?.CountryId,
                    Type = 1,
                    TrxNo = maxTrxNo,
                    Label = $"Imported from Servio SmartHR Timekeeping {DateTime.Now.ToShortDateString()}",
                    Description = $"Imported from Servio SmartHR Timekeeping {DateTime.Now.ToShortDateString()}",
                    RefDate = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Empty,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = Guid.Empty
                };


                foreach (var group in dtrGroups)
                {
                    short displayOrder = 1;

                    foreach (var mapping in globalSettings.PayrollCodeMappings.Where(i => !string.IsNullOrEmpty(i.Source)))
                    {
                        decimal fieldValue = 0;

                        foreach (var groupItem in group)
                        {
                            fieldValue += (decimal)typeof(DailyTransactionRecord).GetProperty(mapping.Target).GetValue(groupItem);
                        }

                        if (fieldValue == 0)
                            continue;

                        var line = new PayrollTrxLines()
                        {
                            DisplayOrder = displayOrder++,
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
                            CreatedBy = CurrentUser.DPUserId ?? Guid.Empty,
                            ModifiedOn = DateTime.Now,
                            ModifiedBy = CurrentUser.DPUserId ?? Guid.Empty
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

        public DataTable GetExportExcelData(DateTime start, DateTime end, string jobGradeBand)
        {
            try
            {
                var data = DataTableHelpers.ToStringDataTable(List(start, end, jobGradeBand));

                foreach (var propInfo in typeof(IEntity).GetProperties())
                {
                    data.Columns.Remove(propInfo.Name);
                }

                foreach (var propInfo in typeof(IModel).GetProperties())
                {
                    data.Columns.Remove(propInfo.Name);
                }

                return data;
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex; ;
            }
        }
    }
}


