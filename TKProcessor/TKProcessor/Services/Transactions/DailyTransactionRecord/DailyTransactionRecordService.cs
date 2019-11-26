using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Contexts;
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

        public IEnumerable<DailyTransactionRecord> List(DateTime start, DateTime end, IEnumerable<string> jobGradeBandFilter, IEnumerable<Employee> employeesFilter = null)
        {
            try
            {
                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (jobGradeBandFilter.Count() == 0)
                    throw new ArgumentNullException("Job Grade Band cannot be empty");


                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.Date >= start && i.ScheduleDate.Date <= end)
                                            .ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Where(ws => jobGradeBandFilter.Any(jgb => jgb == ws.Employee.JobGradeBand)).Select(i => i.Employee).Distinct().ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with Job Grade Band {jobGradeBandFilter}");

                if (employeesFilter != null && employeesFilter.Count() > 0)
                {
                    employees = employees.Where(emp => employeesFilter.Any(empF => empF.Id == emp.Id)).ToList();

                    if (employees.Count == 0)
                        throw new Exception($"Filtered employees have no records to process");
                }

                return Context.DailyTransactionRecord.Include(i => i.Employee)
                                                     .Include(i => i.Shift)
                                                     .Where(i => i.TransactionDate.Value.Date >= start &&
                                                                 i.TransactionDate.Value.Date <= end &&
                                                                 employees.Any(emp => emp == i.Employee))
                                                     .OrderBy(i => i.Employee.EmployeeCode)
                                                     .OrderBy(i => i.TransactionDate);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Process(DateTime start, DateTime end, IEnumerable<string> jobGradeBandFilter = null, IEnumerable<Employee> employeesFilter = null,
                            Action<string> iterationCallback = null)
        {
            try
            {
                iterationCallback?.Invoke($"Initializing...");

                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                var workschedules = Context.WorkSchedule.Include(i => i.Employee)
                                                        .Include(i => i.Shift)
                                                        .Where(i => i.ScheduleDate.Date >= start && i.ScheduleDate.Date <= end && i.IsActive)
                                                        .ToArray();

                if (workschedules.Length == 0)
                    throw new Exception($"No work schedules were found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Select(i => i.Employee)
                                             .Distinct()
                                             .ToArray();

                if (jobGradeBandFilter != null && jobGradeBandFilter.Count() > 0)
                {
                    employees = employees.Where(emp => jobGradeBandFilter.Any(jgb => jgb == emp.JobGradeBand)).ToArray();
                }

                if (employees.Length == 0)
                    throw new Exception($"No employees were found with Job Grade Band {jobGradeBandFilter}");

                if (employeesFilter != null && employeesFilter.Count() > 0)
                {
                    employees = employees.Where(emp => employeesFilter.Any(empF => empF.Id == emp.Id)).ToArray();

                    if (employees.Length == 0)
                        throw new Exception($"Filtered employees have no records to process");
                }

                var globalSettings = Context.GlobalSetting.Include(i => i.AutoApproveDTRFieldsList).FirstOrDefault();

                var rawDataList = Context.RawData.ToArray();

                var dtrRecords = Context.DailyTransactionRecord.Where(i => i.TransactionDate.Value.Date >= start && i.TransactionDate.Value.Date <= end);

                foreach (var employee in employees)
                {
                    var rawdata = rawDataList.Where(i => i.ScheduleDate >= start &&
                                                        i.ScheduleDate.Date <= end &&
                                                        string.Compare(i.BiometricsId, employee.BiometricsId) == 0).ToList();

                    var existing = dtrRecords.Where(i => i.Employee == employee);

                    if (existing.Count() > 0)
                    {
                        Context.DailyTransactionRecord.RemoveRange(existing);

                        Context.SaveChanges();
                    }

                    var scheduleDate = start;

                    while (scheduleDate <= end)
                    {
                        try
                        {
                            iterationCallback?.Invoke($"Processing {employee.EmployeeCode} - {employee.FullName} - {scheduleDate.ToLongDateString()}...");

                            var shift = workschedules.FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == scheduleDate)?.Shift;

                            var timein = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                        (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                        i.TransactionType == (int)TransactionType.TimeIn);

                            var timeout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                        (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                        i.TransactionType == (int)TransactionType.TimeOut);

                            var ambreakin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                        (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                                        i.TransactionType == (int)TransactionType.AMBreakIn);

                            var ambreakout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.AMBreakOut);

                            var lunchin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.LunchIn);
                            var lunchout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.LunchOut);
                            var pmbreakin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.PMBreakIn);
                            var pmbreakout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.PMBreakOut);
                            var dinnerin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.DinnerIn);
                            var dinnerout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                 (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                 i.TransactionType == (int)TransactionType.DinnerOut);

                            DailyTransactionRecord DTR = new DailyTransactionRecord()
                            {
                                Employee = employee,
                                TransactionDate = scheduleDate,
                                Shift = shift == null ? null : Context.Shift.Find(shift.Id),
                                TimeIn = timein?.TransactionDateTime.RemoveSeconds(),
                                TimeOut = timeout?.TransactionDateTime.RemoveSeconds(),
                            };

                            if (shift == null)
                            {
                                iterationCallback?.Invoke($"Skipping {employee} - {scheduleDate.ToLongDateString()}");
                                scheduleDate = scheduleDate.AddDays(1);
                                continue;
                            }

                            IEnumerable<Holiday> holidays = null;
                            IEnumerable<Leave> leaves = null;

                            if (DTR.Shift?.FocusDate.Value == (int)FocusDate.ScheduleIn)
                            {
                                //holidays = holidayService.GetHolidays(DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                                holidays = holidayService.GetHolidays(DTR.TimeIn ?? scheduleDate);
                                //leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                                leaves = leaveService.GetLeaves(employee.EmployeeCode,DTR.TimeIn ?? scheduleDate);

                            }
                            else if (DTR.Shift?.FocusDate.Value == (int)FocusDate.ScheduleOut)
                            {
                                DateTime date = scheduleDate;
                                if(DTR.Shift?.ScheduleOut < DTR.Shift?.ScheduleIn)
                                {
                                    date = date.AddDays(1);
                                }

                                holidays = holidayService.GetHolidays(DTR.TimeOut ?? date);
                                leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeOut ?? date);
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

                                        DTR.AddRemarks("Legal Holiday");
                                    }
                                    else if (holiday.Type == (int)HolidayType.Special)
                                    {
                                        isSpecialHoliday = true;

                                        DTR.AddRemarks("Special Holiday, ");
                                    }
                                }
                            }

                            var requiredWorkHours = DTR.Shift.RequiredWorkHours ?? Convert.ToDecimal((DTR.Shift.ScheduleOut - DTR.Shift.ScheduleOut).Value.TotalMinutes / 60);

                            if (timein == null && timeout == null && (!shift.IsRestDay.HasValue || shift.IsRestDay.Value == false))
                            {

                                if (holidays != null && holidays.Count() > 0)
                                    DTR.RegularWorkHours = requiredWorkHours;
                                else
                                    DTR.AbsentHours = requiredWorkHours;
                            }
                            else
                            {
                                if (timein != null && timeout != null && timein.TransactionDateTime == timeout.TransactionDateTime)
                                {
                                    DTR.RegularWorkHours = requiredWorkHours;
                                    DTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);
                                }
                                else
                                {
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

                                    foreach (var notAutoApprove in globalSettings.AutoApproveDTRFieldsList.Where(i => !i.IsSelected))
                                    {
                                        typeof(DailyTransactionRecord).GetProperty("Approved" + notAutoApprove.Name).SetValue(DTR, 0m);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(DTR.Remarks))
                                DTR.Remarks = DTR.Remarks.Trim().Trim(';').Trim(',');

                            Save(DTR);
                        }
                        catch (Exception ex)
                        {
                            Context.ErrorLog.Add(new ErrorLog(ex));
                            Context.SaveChanges();

                            throw ex;
                        }

                        scheduleDate = scheduleDate.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void ProcessSplit(DateTime start, DateTime end, IList<string> jobGradeBandFilter, IEnumerable<Employee> employeesFilter = null,
                                 Action<string> iterationCallback = null)
        {
            try
            {
                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                if (jobGradeBandFilter.Count() == 0)
                    throw new ArgumentNullException("Job Grade Band cannot be empty");

                var workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                           .Where(i => i.ScheduleDate.Date >= start.AddDays(-1) && i.ScheduleDate.Date <= end).ToList(); //Include work schedule of day before start day... Filter split shifts

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                var employees = workschedules.Where(ws => jobGradeBandFilter.Any(jgb => jgb == ws.Employee.JobGradeBand)).Select(i => i.Employee).Distinct().ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with Job Grade Band {jobGradeBandFilter}");

                if (employeesFilter != null && employeesFilter.Count() > 0)
                {
                    employees = employees.Where(emp => employeesFilter.Any(empF => empF.Id == emp.Id)).ToList();

                    if (employees.Count == 0)
                        throw new Exception($"Filtered employees have no records to process");
                }

                var rawdata = Context.RawData.Where(i => i.ScheduleDate >= start.AddDays(-1) && i.ScheduleDate.Date <= end &&
                                                            employees.Any(emp => emp.BiometricsId == i.BiometricsId)).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No Raw Data was found");

                var globalSettings = Context.GlobalSetting.FirstOrDefault();


                foreach (var employee in employees)
                {
                    iterationCallback?.Invoke($"Processing {employee.EmployeeCode} - {employee.FullName}...");

                    DailyTransactionRecord hangingDTR = new DailyTransactionRecord();

                    bool firstIteration = true;
                    DateTime dateIterator = start.AddDays(-1); //start from day before to compute hanging DTR
                    while (dateIterator <= end)
                    {
                        if (!firstIteration)
                        {
                            var existing = Context.DailyTransactionRecord.Where(i => i.Employee == employee && i.TransactionDate.Value.Date == dateIterator);

                            foreach (var item in existing)
                            {
                                Context.DailyTransactionRecord.Remove(item);
                            }
                        }

                        IEnumerable<Holiday> holidays = holidayService.GetHolidays(dateIterator);

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
                        currentDayDTR.TransactionDate = dateIterator;
                        //currentDayDTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);


                        hangingDTR = new DailyTransactionRecord(); //Reset hanging DTR

                        IList<RawData> rawDataTimeIn = rawdata.Where(raw => raw.ScheduleDate == dateIterator && raw.BiometricsId == employee.BiometricsId && raw.TransactionType == (int)TransactionType.TimeIn).OrderBy(raw => raw.TransactionDateTime).ToList();
                        IList<RawData> rawDataTimeOut = rawdata.Where(raw => raw.ScheduleDate == dateIterator && raw.BiometricsId == employee.BiometricsId && raw.TransactionType == (int)TransactionType.TimeOut).OrderBy(raw => raw.TransactionDateTime).ToList();

                        var workSchedules = workschedules.Where(ws => ws.Employee == employee && ws.ScheduleDate == dateIterator).OrderBy(ws => ws.Shift.ScheduleIn); //for standard, need to change for flex

                        foreach (var workSchedule in workSchedules)
                        {
                            DailyTransactionRecord DTR = new DailyTransactionRecord(); //DTR for this work schedule

                            DTR.Employee = employee;
                            DTR.Shift = workSchedule.Shift;
                            DTR.TransactionDate = dateIterator;

                            //standard
                            var _schedIn = dateIterator.Add(workSchedule.Shift.ScheduleIn.Value.TimeOfDay).RemoveSeconds();
                            var _schedOut = dateIterator.Add(workSchedule.Shift.ScheduleOut.Value.TimeOfDay).RemoveSeconds();

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

                        dateIterator = dateIterator.AddDays(1);
                    }
                }



            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void ExportToDP(DateTime start, DateTime end, DateTime payOutDate, IEnumerable<string> jobGradeBandFilter, IEnumerable<Employee> employeesFilter = null,
                               Action<string> iterationCallback = null)
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

                if (jobGradeBandFilter.Count() == 0)
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

                foreach (var jobGradeBand in jobGradeBandFilter)
                {
                    iterationCallback?.Invoke($"Exporting employees with job grade band {jobGradeBand}...");

                    var dtrGroups = List(start, end, jobGradeBandFilter, employeesFilter).Where(i => i.TransactionDate >= start && i.TransactionDate <= end)
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
                        iterationCallback?.Invoke($"Exporting {group.Key}...");

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
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Adjust(Employee employee, DateTime transactionDate, Shift shift, DateTime? timein, DateTime? timeout)
        {
            // Update work schedule
            using (WorkScheduleService wsService = new WorkScheduleService(CurrentUser.Id))
            {
                var ws = wsService.List().FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == transactionDate);

                wsService.Save(new WorkSchedule()
                {
                    Id = (ws == default(WorkSchedule)) ? Guid.NewGuid() : ws.Id,
                    Employee = ws.Employee,
                    ScheduleDate = ws.ScheduleDate,
                    Shift = (ws.Shift.Id == shift.Id) ? ws.Shift : shift,
                    IsActive = ws.IsActive,
                    CreatedBy = ws.CreatedBy,
                    CreatedOn = ws.CreatedOn,
                    LastModifiedBy = CurrentUser,
                    LastModifiedOn = DateTime.Now
                });
            }
            // Work schedule


            // Update Raw Data
            using (RawDataService service = new RawDataService(CurrentUser.Id))
            {
                var rawData = service.List().Where(i => i.BiometricsId == employee.BiometricsId && i.ScheduleDate == transactionDate).OrderBy(i => i.TransactionType).ToArray();

                var rawDataIn = rawData.FirstOrDefault(i => i.TransactionType == (int)TransactionType.TimeIn);

                if (rawDataIn == default(RawData))
                {
                    rawDataIn = new RawData()
                    {
                        BiometricsId = employee.BiometricsId,
                        IsActive = true,
                        TransactionType = (int)TransactionType.TimeIn,
                        ScheduleDate = transactionDate
                    };
                }

                if (rawDataIn.TransactionDateTime != timein.Value)
                {
                    rawDataIn.TransactionDateTime = timein.Value;

                    service.Save(rawDataIn);
                }

                var rawDataOut = rawData.FirstOrDefault(i => i.TransactionType == (int)TransactionType.TimeOut);

                if (rawDataOut == default(RawData))
                {
                    rawDataOut = new RawData()
                    {
                        BiometricsId = employee.BiometricsId,
                        IsActive = true,
                        TransactionType = (int)TransactionType.TimeOut,
                        ScheduleDate = transactionDate
                    };
                }

                if (rawDataOut.TransactionDateTime != timeout.Value)
                {
                    rawDataOut.TransactionDateTime = timeout.Value;

                    service.Save(rawDataOut);
                }

            }
            // Raw data

            Context = new TKContext();

            Process(transactionDate, transactionDate, null, new Employee[] { employee });
        }

        public DataTable GetExportExcelData(DateTime start, DateTime end, IEnumerable<string> jobGradeBandFilter, IEnumerable<Employee> employeesFilter = null)
        {
            try
            {
                var data = DataTableHelpers.ToStringDataTable(List(start, end, jobGradeBandFilter, employeesFilter));

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

                throw ex;
            }
        }
    }
}


