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
        System.Diagnostics.Stopwatch sw;

        private readonly DPContext dPContext;

        IDTRProcessor processor;
        HolidayService holidayService;
        LeaveService leaveService;

        public DailyTransactionRecordService(Guid userId) : base(userId)
        {
            dPContext = new DPContext();
            holidayService = new HolidayService();
            leaveService = new LeaveService();

            sw = new System.Diagnostics.Stopwatch();
        }

        private void WriteToConsole(string message)
        {
            System.Diagnostics.Debug.WriteLine(sw.Elapsed + " >> " + message);
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


                List<WorkSchedule> workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                            .Where(i => i.ScheduleDate.Date >= start && i.ScheduleDate.Date <= end)
                                            .ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                List<Employee> employees = workschedules.Where(ws => jobGradeBandFilter.Any(jgb => jgb == ws.Employee.JobGradeBand)).Select(i => i.Employee).Distinct().ToList();

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
                sw.Start();

                WriteToConsole("Process start");

                WorkSchedule empWorkSched = null;

                Shift shift = null;

                WorkSite worksite = null;

                RawData timein = null,
                        timeout = null,
                        ambreakin = null,
                        ambreakout = null,
                        lunchin = null,
                        lunchout = null,
                        pmbreakin = null,
                        pmbreakout = null,
                        dinnerin = null,
                        dinnerout = null;

                iterationCallback?.Invoke($"Initializing...");

                start = start.Date;
                end = end.Date;

                if (start > end)
                    throw new Exception("Start date should not be greater than the end date");

                WorkSchedule[] workschedules = Context.WorkSchedule.Include(i => i.Employee)
                                                        .Include(i => i.Shift)
                                                        .Where(i => i.ScheduleDate.Date >= start && i.ScheduleDate.Date <= end && i.IsActive)
                                                        .ToArray();

                if (workschedules.Length == 0)
                    throw new Exception($"No work schedules were found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                Employee[] employees = workschedules.Select(i => i.Employee)
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

                GlobalSetting globalSettings = Context.GlobalSetting.Include(i => i.AutoApproveDTRFieldsList).FirstOrDefault();

                RawData[] rawDataList = Context.RawData.ToArray();

                IQueryable<DailyTransactionRecord> dtrRecords = Context.DailyTransactionRecord.Where(i => i.TransactionDate.Value.Date >= start && i.TransactionDate.Value.Date <= end);

                AutoSaveChanges = false;

                Context.ChangeTracker.AutoDetectChangesEnabled = false;

                for (int loopCounter = 0; loopCounter < employees.Length; loopCounter++)
                {
                    var employee = employees[loopCounter];

                    List<RawData> rawdata = rawDataList.Where(i => i.ScheduleDate >= start &&
                                                                   i.ScheduleDate.Date <= end &&
                                                                   string.Compare(i.BiometricsId, employee.BiometricsId) == 0).ToList();

                    IQueryable<DailyTransactionRecord> existing = dtrRecords.Where(i => i.Employee == employee);

                    if (existing.Count() > 0)
                    {
                        Context.DailyTransactionRecord.RemoveRange(existing);
                    }

                    DateTime scheduleDate = start.AddDays(-1);

                    while (scheduleDate <= end)
                    {
                        scheduleDate = scheduleDate.AddDays(1);

                        try
                        {
                            //iterationCallback?.Invoke($"Processing {employee.EmployeeCode} - {employee.FullName} - {scheduleDate.ToLongDateString()}...");

                            empWorkSched = workschedules.FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == scheduleDate);

                            shift = empWorkSched?.Shift;

                            if (shift == null)
                            {
                                iterationCallback?.Invoke($"Skipping {employee} - {scheduleDate.ToLongDateString()}");
                                continue;
                            }

                            worksite = empWorkSched?.WorkSite;

                            DateTime? timeinMin = null;
                            DateTime? timeinMax = null;

                            if (rawdata.Count(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                      (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                      i.TransactionType == (int)TransactionType.TimeIn) > 0)
                            {
                                timeinMin = rawdata.Where(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                        (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                        i.TransactionType == (int)TransactionType.TimeIn).Min(i => i.TransactionDateTime);

                                timein = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                       (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                       i.TransactionType == (int)TransactionType.TimeIn &&
                                                                       i.TransactionDateTime == timeinMin);
                            }
                            else
                            {
                                timein = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                         (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                         i.TransactionType == (int)TransactionType.TimeIn);
                            }


                            if (rawdata.Count(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                    (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                    i.TransactionType == (int)TransactionType.TimeOut) > 0)
                            {
                                timeinMax = rawdata.Where(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                          (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                          i.TransactionType == (int)TransactionType.TimeOut).Max(i => i.TransactionDateTime);

                                timeout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                           (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                           i.TransactionType == (int)TransactionType.TimeOut &&
                                                                           i.TransactionDateTime == timeinMax);
                            }
                            else
                            {
                                timeout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                           (i.ScheduleDate.Date == scheduleDate.Date) &&
                                                                           i.TransactionType == (int)TransactionType.TimeOut);
                            }

                            ambreakin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                                                (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                                                i.TransactionType == (int)TransactionType.AMBreakIn);

                            ambreakout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                         (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                         i.TransactionType == (int)TransactionType.AMBreakOut);

                            lunchin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                         (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                         i.TransactionType == (int)TransactionType.LunchIn);
                            lunchout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                         (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                         i.TransactionType == (int)TransactionType.LunchOut);
                            pmbreakin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                         (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                         i.TransactionType == (int)TransactionType.PMBreakIn);
                            pmbreakout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                         (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                         i.TransactionType == (int)TransactionType.PMBreakOut);
                            dinnerin = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
                                         (i.TransactionDateTime.Date == scheduleDate.Date) &&
                                         i.TransactionType == (int)TransactionType.DinnerIn);
                            dinnerout = rawdata.FirstOrDefault(i => string.Compare(i.BiometricsId, employee.BiometricsId) == 0 &&
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

                            if (worksite != null)
                                DTR.AddRemarks(worksite.Name);

                            IEnumerable<Holiday> holidays = null;
                            IEnumerable<Leave> leaves = null;

                            if (DTR.Shift?.FocusDate == null || DTR.Shift?.FocusDate.Value == (int)FocusDate.ScheduleIn)
                            {
                                //holidays = holidayService.GetHolidays(DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                                holidays = holidayService.GetHolidays(DTR.TimeIn ?? scheduleDate).ToList();
                                //leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeIn ?? DTR.Shift.ScheduleIn.Value);
                                leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeIn ?? scheduleDate).ToList();

                            }
                            else if (DTR.Shift?.FocusDate.Value == (int)FocusDate.ScheduleOut)
                            {
                                DateTime date = scheduleDate;
                                if (DTR.Shift?.ScheduleOut < DTR.Shift?.ScheduleIn)
                                {
                                    date = date.AddDays(1);
                                }

                                holidays = holidayService.GetHolidays(DTR.TimeOut ?? date);
                                leaves = leaveService.GetLeaves(employee.EmployeeCode, DTR.TimeOut ?? date);
                            }

                            if (leaves != null && leaves.Count() > 0)
                            {
                                DTR.LeaveType = leaves.First().LeaveType;
                            }

                            bool isLegalHoliday = false;
                            bool isSpecialHoliday = false;

                            if (holidays != null)
                            {
                                foreach (Holiday holiday in holidays)
                                {
                                    if (holiday.Type == (int)HolidayType.Legal)
                                    {
                                        isLegalHoliday = true;

                                        DTR.AddRemarks("Legal Holiday");
                                    }
                                    else if (holiday.Type == (int)HolidayType.Special)
                                    {
                                        isSpecialHoliday = true;

                                        DTR.AddRemarks("Special Holiday");
                                    }
                                }
                            }

                            decimal requiredWorkHours = DTR.Shift.RequiredWorkHours ?? Convert.ToDecimal((DTR.Shift.ScheduleOut - DTR.Shift.ScheduleOut).Value.TotalMinutes / 60);

                            bool noWork = ((timein == null && timeout == null) ||  ((timein != null && timein.TransactionDateTime == timein.TransactionDateTime.GetStartOfDay()) && (timeout != null && timeout.TransactionDateTime == timeout.TransactionDateTime.GetStartOfDay())));

                            if (noWork && holidays != null && holidays.Count() > 0)
                            {
                                if (holidays.Any(i => i.Type == (int)HolidayType.Legal))
                                {
                                    DTR.RegularWorkHours = requiredWorkHours;
                                    //DTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);
                                }
                            }
                            else if (noWork && leaves != null && leaves.Count() > 0)
                            {
                                if (leaves.First().LeaveHours == 1m)
                                    DTR.RegularWorkHours = requiredWorkHours;
                                else
                                    DTR.RegularWorkHours = requiredWorkHours / 2;
                            }
                            else if (noWork && (shift.IsRestDay.HasValue && shift.IsRestDay.Value == true))
                            {
                                DTR.AddRemarks("Rest Day");
                            }
                            else if (noWork)
                            {
                                DTR.AbsentHours = requiredWorkHours;
                            }
                            else
                            {
                                if (timein != null && timeout != null && timein.TransactionDateTime == timeout.TransactionDateTime)
                                {
                                    //DTR.RegularWorkHours = requiredWorkHours;
                                    //DTR.RemapWorkHours(isLegalHoliday, isSpecialHoliday);
                                }
                                else
                                {
                                    if (DTR.Shift?.ShiftType == null || DTR.Shift?.ShiftType == (int)ShiftType.Standard)
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

                                    if (globalSettings != null)
                                    {
                                        foreach (SelectionSetting notAutoApprove in globalSettings.AutoApproveDTRFieldsList.Where(i => !i.IsSelected))
                                        {
                                            typeof(DailyTransactionRecord).GetProperty("Approved" + notAutoApprove.Name).SetValue(DTR, 0m);
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(DTR.Remarks))
                                DTR.Remarks = DTR.Remarks.Trim().Trim(';').Trim(',');

                            Save(DTR);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    if (!AutoSaveChanges)
                    {
                        WriteToConsole($"Saving {employee} ({loopCounter + 1}/{employees.Length})...");

                        iterationCallback?.Invoke($"Saving {employee} ({loopCounter + 1}/{employees.Length})...");

                        SaveChanges();

                        iterationCallback?.Invoke($"Saved {employee} ({loopCounter + 1}/{employees.Length})...");

                        WriteToConsole($"Saved {employee} ({loopCounter + 1}/{employees.Length})");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            sw.Stop();

            AutoSaveChanges = false;

            Context.ChangeTracker.AutoDetectChangesEnabled = true;
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

                List<WorkSchedule> workschedules = Context.WorkSchedule.Include(i => i.Employee).Include(i => i.Shift)
                                           .Where(i => i.ScheduleDate.Date >= start.AddDays(-1) && i.ScheduleDate.Date <= end).ToList(); //Include work schedule of day before start day... Filter split shifts

                if (workschedules.Count == 0)
                    throw new Exception($"No work schedules found with dates between {start.ToShortDateString()} and {end.ToShortDateString()}");

                List<Employee> employees = workschedules.Where(ws => jobGradeBandFilter.Any(jgb => jgb == ws.Employee.JobGradeBand)).Select(i => i.Employee).Distinct().ToList();

                if (employees.Count == 0)
                    throw new Exception($"No employees found with Job Grade Band {jobGradeBandFilter}");

                if (employeesFilter != null && employeesFilter.Count() > 0)
                {
                    employees = employees.Where(emp => employeesFilter.Any(empF => empF.Id == emp.Id)).ToList();

                    if (employees.Count == 0)
                        throw new Exception($"Filtered employees have no records to process");
                }

                List<RawData> rawdata = Context.RawData.Where(i => i.ScheduleDate >= start.AddDays(-1) && i.ScheduleDate.Date <= end &&
                                                            employees.Any(emp => emp.BiometricsId == i.BiometricsId)).ToList();

                if (workschedules.Count == 0)
                    throw new Exception($"No Raw Data was found");

                GlobalSetting globalSettings = Context.GlobalSetting.FirstOrDefault();


                foreach (Employee employee in employees)
                {
                    iterationCallback?.Invoke($"Processing {employee.EmployeeCode} - {employee.FullName}...");

                    DailyTransactionRecord hangingDTR = new DailyTransactionRecord();

                    bool firstIteration = true;
                    DateTime dateIterator = start.AddDays(-1); //start from day before to compute hanging DTR
                    while (dateIterator <= end)
                    {
                        if (!firstIteration)
                        {
                            IQueryable<DailyTransactionRecord> existing = Context.DailyTransactionRecord.Where(i => i.Employee == employee && i.TransactionDate.Value.Date == dateIterator);

                            foreach (DailyTransactionRecord item in existing)
                            {
                                Context.DailyTransactionRecord.Remove(item);
                            }
                        }

                        IEnumerable<Holiday> holidays = holidayService.GetHolidays(dateIterator);

                        bool isLegalHoliday = false;
                        bool isSpecialHoliday = false;
                        foreach (Holiday holiday in holidays)
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

                        IOrderedEnumerable<WorkSchedule> workSchedules = workschedules.Where(ws => ws.Employee == employee && ws.ScheduleDate == dateIterator).OrderBy(ws => ws.Shift.ScheduleIn); //for standard, need to change for flex

                        foreach (WorkSchedule workSchedule in workSchedules)
                        {
                            DailyTransactionRecord DTR = new DailyTransactionRecord(); //DTR for this work schedule

                            DTR.Employee = employee;
                            DTR.Shift = workSchedule.Shift;
                            DTR.TransactionDate = dateIterator;

                            //standard
                            DateTime _schedIn = dateIterator.Add(workSchedule.Shift.ScheduleIn.Value.TimeOfDay).RemoveSeconds();
                            DateTime _schedOut = dateIterator.Add(workSchedule.Shift.ScheduleOut.Value.TimeOfDay).RemoveSeconds();

                            if (_schedOut < _schedIn)
                            {
                                _schedOut = _schedOut.AddDays(1);
                            }

                            RawData timeIn = rawDataTimeIn.FirstOrDefault(raw => raw.TransactionDateTime < _schedOut); //for standard, need to change for flex
                            if (timeIn != null)
                            {
                                DTR.TimeIn = timeIn.TransactionDateTime;
                                rawDataTimeIn.Remove(timeIn);
                                RawData timeOut = rawDataTimeOut.FirstOrDefault(raw => raw.TransactionDateTime > _schedIn);
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
                                    Tuple<DailyTransactionRecord, DailyTransactionRecord> splitDTR = ((DTRProcessorBase)processor).Split(DTR);

                                    DailyTransactionRecord tail = splitDTR.Item2; //add values to hanging DTR
                                    hangingDTR.Merge(tail);
                                }
                            }
                            else
                            {
                                if (((DTRProcessorBase)processor).IsSplittable)
                                {
                                    Tuple<DailyTransactionRecord, DailyTransactionRecord> splitDTR = ((DTRProcessorBase)processor).Split(DTR);

                                    DailyTransactionRecord head = splitDTR.Item1; //add values to current day, consider holidays and restday
                                    DailyTransactionRecord tail = splitDTR.Item2; //add values to hanging DTR

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

                foreach (string jobGradeBand in jobGradeBandFilter)
                {
                    iterationCallback?.Invoke($"Exporting employees with job grade band {jobGradeBand}...");

                    IEnumerable<IGrouping<Employee, DailyTransactionRecord>> dtrGroups = List(start, end, jobGradeBandFilter, employeesFilter).Where(i => i.TransactionDate >= start && i.TransactionDate <= end)
                                                                                         .ToArray().GroupBy(i => i.Employee).ToArray();

                    if (dtrGroups.Count() == 0)
                        throw new Exception($"No DTR records has been found from {start.ToLongDateString()} " +
                                            $"to {end.ToLongDateString()} with Pay package code '{jobGradeBand}'");

                    PayPackage payPackage = dPContext.PayPackage.AsEnumerable().First(i => i.Code == globalSettings.PayPackageMappings.AsEnumerable().First(ii => ii.Target == jobGradeBand).Source);

                    if (payPackage == null)
                        throw new Exception($"Please setup pay package mapping for Job Grade Band '{jobGradeBand}' in the Settings");

                    PayFreqCalendar payFreqCalendar = dPContext.PayPackagePayFreqCalendars
                                                   .Include(i => i.PayPackageSeq)
                                                   .Include(i => i.PayFreqCalendarSeq)
                                                   .AsEnumerable()
                                                   .First(i => i.PayPackageSeqId == payPackage.SeqId)?.PayFreqCalendarSeq;

                    if (payFreqCalendar == null)
                        throw new Exception($"Please setup Pay Frequency Calendar that corresponds to Job Grade Band '{jobGradeBand}'");

                    long maxTrxNo = dPContext.Company.AsEnumerable().First().NextPayrollTrxNo++;

                    PayrollTrx trx = new PayrollTrx()
                    {
                        CountryId = dPContext.Country.AsEnumerable().FirstOrDefault()?.CountryId,
                        Type = 1,
                        TrxNo = maxTrxNo,
                        Label = $"Imported from Servio SmartHR Timekeeping {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}",
                        Description = $"Imported from Servio SmartHR Timekeeping {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}",
                        RefDate = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        CreatedBy = Guid.Empty,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = Guid.Empty
                    };


                    foreach (IGrouping<Employee, DailyTransactionRecord> group in dtrGroups)
                    {
                        iterationCallback?.Invoke($"Exporting {group.Key}...");

                        short displayOrder = 1;

                        foreach (Mapping mapping in globalSettings.PayrollCodeMappings.Where(i => !string.IsNullOrEmpty(i.Source)))
                        {
                            decimal fieldValue = 0;

                            foreach (DailyTransactionRecord groupItem in group)
                            {
                                fieldValue += (decimal)typeof(DailyTransactionRecord).GetProperty(mapping.Target).GetValue(groupItem);
                            }

                            if (fieldValue == 0)
                                continue;

                            PayrollTrxLines line = new PayrollTrxLines()
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
                WorkSchedule ws = wsService.List().FirstOrDefault(i => i.Employee.Id == employee.Id && i.ScheduleDate == transactionDate);

                wsService.Save(new WorkSchedule()
                {
                    Id = (ws == default(WorkSchedule)) ? Guid.NewGuid() : ws.Id,
                    Employee = ws.Employee,
                    ScheduleDate = ws.ScheduleDate,
                    Shift = (ws.Shift.Id == shift.Id) ? ws.Shift : shift,
                    IsActive = ws.IsActive,
                    CreatedBy = ws.CreatedBy,
                    CreatedOn = ws.CreatedOn,
                    LastModifiedBy = CurrentUser.ToString(),
                    LastModifiedOn = DateTime.Now
                });
            }
            // Work schedule

            // Update Raw Data
            using (RawDataService service = new RawDataService(CurrentUser.Id))
            {
                RawData[] rawData = service.List().Where(i => i.BiometricsId == employee.BiometricsId && i.ScheduleDate == transactionDate).OrderBy(i => i.TransactionType).ToArray();

                if (timein.HasValue)
                {
                    RawData rawDataIn = rawData.FirstOrDefault(i => i.TransactionType == (int)TransactionType.TimeIn);

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

                    rawDataIn.TransactionDateTime = DateTimeHelpers.ConstructDate(transactionDate, timein.Value);

                    service.SaveNoAdjustment(rawDataIn);
                }
                else
                {
                    service.Delete(employee.BiometricsId, transactionDate, (int)TransactionType.TimeIn);
                }

                if (timeout.HasValue)
                {
                    RawData rawDataOut = rawData.FirstOrDefault(i => i.TransactionType == (int)TransactionType.TimeOut);

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

                    if (timein.HasValue && timein.Value.TimeOfDay > timeout.Value.TimeOfDay)
                    {
                        timeout = timeout.Value.AddDays(1);

                        rawDataOut.TransactionDateTime = DateTimeHelpers.ConstructDate(transactionDate.AddDays(1), timeout.Value);
                    }
                    else
                    {
                        rawDataOut.TransactionDateTime = DateTimeHelpers.ConstructDate(transactionDate, timeout.Value);
                    }

                    service.SaveNoAdjustment(rawDataOut);
                }
                else
                {
                    service.Delete(employee.BiometricsId, transactionDate, (int)TransactionType.TimeOut);
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
                DataTable data = DataTableHelpers.ToStringDataTable(List(start, end, jobGradeBandFilter, employeesFilter));

                foreach (System.Reflection.PropertyInfo propInfo in typeof(IEntity).GetProperties())
                {
                    data.Columns.Remove(propInfo.Name);
                }

                foreach (System.Reflection.PropertyInfo propInfo in typeof(IModel).GetProperties())
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


