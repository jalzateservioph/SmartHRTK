using System;
using System.Collections.Generic;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class StandardDTRProcessor : DTRProcessorBase, IDTRProcessor
    {
        DateTime expectedTimeIn;
        DateTime expectedTimeOut;

        public StandardDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Leave> leaves) : base()
        {
            this.DTR = DTR;
            Leaves = leaves;
            totalBreak = GetTotalBreakDuration();
            GetLeaveDuration();
            GetRequiredWorkHours();
            expectedTimeIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleIn.Value.TimeOfDay);
            expectedTimeOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleOut.Value.TimeOfDay);
        }

        public StandardDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Leave> leaves, IEnumerable<Holiday> holidays) : base()
        {
            this.DTR = DTR;
            Leaves = leaves;
            this.Holidays = holidays;
            totalBreak = GetTotalBreakDuration();
            GetLeaveDuration();
            GetRequiredWorkHours();
            expectedTimeIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleIn.Value.TimeOfDay);
            expectedTimeOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleOut.Value.TimeOfDay);
        }

        public void ComputeRegular()
        {
            if (leaveDuration == 1M || leaveDuration == 0.5M)
            {
                workHours = DTR.Shift.RequiredWorkHours.Value * leaveDuration;
            }

            else if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                GetActualTimeInAndOut();
                workHours = (decimal)(actualTimeOut - actualTimeIn).TotalMinutes;
                AdjustWorkHours();

                if (workHours > requiredWorkHours * 60)
                {
                    regularWorkHours = requiredWorkHours * 60;
                }
                else
                {
                    regularWorkHours = workHours;
                }

                if (expectedTimeOut < expectedTimeIn)
                {
                    expectedTimeOut = expectedTimeOut.AddDays(1);
                }

                ComputeLate();
                ComputeUndertime();
                ComputeOvertime();
                ComputeNightDifferential();
            }
            else
            {
                absentHours = (decimal)(DTR.Shift.ScheduleOut.Value - DTR.Shift.ScheduleIn.Value).TotalMinutes - totalBreak;
            }

            MapFieldsToDTR();
        }

        private void ComputeLate()
        {

            if (DTR.Shift.IsLateIn == true)
            {
                int gracePeriodMinutes = 0;
                //DateTime latestIn = DTR.Shift.LatestTimeIn.Value.TimeOfDay;
                if (DTR.Shift.GracePeriodLateIn.HasValue)
                {
                    gracePeriodMinutes = DTR.Shift.GracePeriodLateIn.Value;
                }
                if (actualTimeIn > expectedTimeIn)
                {
                    late = (decimal)(actualTimeIn - expectedTimeIn).TotalMinutes;

                    if (actualTimeIn > expectedTimeIn.AddMinutes(gracePeriodMinutes))
                    {
                        approvedLate = late;
                    }
                }

                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfDay.HasValue)
                {
                    if (late > DTR.Shift.MaximumMinutesConsideredAsHalfDay.Value)
                    {
                        absentHours = Math.Round(((decimal)(DTR.Shift.ScheduleOut.Value - DTR.Shift.ScheduleIn.Value).TotalMinutes - totalBreak) / 2, 2);
                        approvedLate = 0;
                    }
                }
                #endregion
            }
            
        }

        private void ComputeUndertime()
        {
            if (DTR.Shift.IsEarlyOut == true)
            {
                int gracePeriodMinutes = 0;
                if (DTR.Shift.GracePeriodEarlyOut.HasValue)
                {
                    //expectedTimeOut.AddMinutes(-DTR.Shift.GracePeriodEarlyOut.Value);
                    gracePeriodMinutes = DTR.Shift.GracePeriodEarlyOut.Value;
                }
                if (actualTimeOut < expectedTimeOut)
                {
                    undertime = (decimal)((expectedTimeOut - actualTimeOut).TotalMinutes);

                    if (actualTimeOut < expectedTimeOut.AddMinutes(-gracePeriodMinutes))
                    {
                        approvedUndertime = undertime;
                    }
                }

                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.HasValue)
                {
                    if (undertime > DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.Value)
                    {
                        absentHours = Math.Round(((decimal)(DTR.Shift.ScheduleOut.Value - DTR.Shift.ScheduleIn.Value).TotalMinutes - totalBreak) / 2, 2);
                        approvedUndertime = 0;
                    }
                }
                #endregion
            }
        }

        private void ComputeOvertime()
        {
            #region Pre-Overtime
            if (DTR.Shift.IsPreShiftOt == true)
            {
                if (actualTimeIn < expectedTimeIn)
                {
                    preShiftOvertime = (decimal)(expectedTimeIn - actualTimeIn).TotalMinutes;
                    approvedPreShiftOvertime = preShiftOvertime;
                }

                if (DTR.Shift.MinimumPreShiftOt.HasValue && DTR.Shift.MinimumPreShiftOt.Value > preShiftOvertime)
                {
                    approvedPreShiftOvertime = 0;
                }

                if (DTR.Shift.MaximumPreShiftOt.HasValue && DTR.Shift.MaximumPreShiftOt.Value < preShiftOvertime)
                {
                    approvedPreShiftOvertime = DTR.Shift.MaximumPreShiftOt.Value;
                }
                totalOvertime += preShiftOvertime;
                approvedOvertime += approvedPreShiftOvertime;
            }
            #endregion

            #region Post-Overtime
            if (DTR.Shift.IsPostShiftOt == true)
            {
                if (actualTimeOut > expectedTimeOut)
                {
                    postShiftOvertime = (decimal)(actualTimeOut - expectedTimeOut).TotalMinutes;
                    approvedPostShiftOvertime = postShiftOvertime;
                }

                if (DTR.Shift.MinimumPostShiftOt.HasValue && DTR.Shift.MinimumPostShiftOt.Value > postShiftOvertime)
                {
                    approvedPostShiftOvertime = 0;
                }

                if (DTR.Shift.MaximumPostShiftOt.HasValue && DTR.Shift.MaximumPostShiftOt.Value < postShiftOvertime)
                {
                    approvedPostShiftOvertime = DTR.Shift.MaximumPostShiftOt.Value;
                }

                totalOvertime += postShiftOvertime;
                approvedOvertime += approvedPostShiftOvertime;
            }
            #endregion
        }

        private void ComputeNightDifferential()
        {
            if (DTR.Shift.NightDiffStart.HasValue && DTR.Shift.NightDiffEnd.HasValue)
            {
                DateTime expectedNightDifferentialStart = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.NightDiffStart.Value.TimeOfDay);
                DateTime expectedNightDifferentialEnd = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.NightDiffEnd.Value.TimeOfDay);

                if (expectedNightDifferentialEnd < expectedNightDifferentialStart)
                {
                    expectedNightDifferentialEnd = expectedNightDifferentialEnd.AddDays(1);
                }

                if (actualTimeIn >= expectedNightDifferentialStart && actualTimeIn < expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(expectedNightDifferentialEnd - actualTimeIn).TotalMinutes;
                }
                else if (expectedNightDifferentialStart >= actualTimeIn && expectedNightDifferentialEnd <= actualTimeOut)
                {
                    nightDifferential = (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                }
                else if (expectedNightDifferentialStart < actualTimeOut && actualTimeOut < expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(actualTimeOut - expectedNightDifferentialStart).TotalMinutes;
                }
                else if (actualTimeIn >= expectedNightDifferentialStart && actualTimeOut <= expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(actualTimeOut - actualTimeIn).TotalMinutes;
                }

                #region Night Differential Overtime

                #region Pre-shift Overtime
                if (DTR.Shift.IsPreShiftOt == true)
                {
                    if (expectedTimeIn > actualTimeIn)
                    {
                        if (expectedNightDifferentialEnd >= expectedTimeIn)
                        {
                            if (expectedNightDifferentialStart < actualTimeIn)
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedTimeIn).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                            }
                        }
                        else if (expectedNightDifferentialEnd > actualTimeIn)
                        {
                            if (expectedNightDifferentialStart <= actualTimeIn)
                            {
                                nightDifferentialOvertime += (decimal)(actualTimeIn- expectedNightDifferentialStart).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                            }
                        }
                    }
                }
                #endregion

                #region Post-shift Overtime
                if (DTR.Shift.IsPostShiftOt == true)
                {
                    if (actualTimeOut > expectedTimeOut)
                    {
                        if (expectedNightDifferentialStart <= expectedTimeOut)
                        {
                            if (expectedNightDifferentialEnd > actualTimeOut)
                            {
                                nightDifferentialOvertime += (decimal)(actualTimeOut - expectedTimeOut).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedTimeOut).TotalMinutes;
                            }
                        }
                        else if (expectedNightDifferentialStart < actualTimeOut)
                        {
                            if (expectedNightDifferentialEnd > actualTimeOut)
                            {
                                nightDifferentialOvertime += (decimal)(actualTimeOut - expectedNightDifferentialStart).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                            }
                        }
                    }
                }
                #endregion
                #endregion
            }
        }

        public void ComputeHolidayAndRestDay()
        {
            if(DTR.TransactionDate.Value == DateTime.Parse("9/20/2019"))
            {
                var breakHere = true;
            }

            if (leaveDuration == 1M || leaveDuration == 0.5M)
            {
                workHours = DTR.Shift.RequiredWorkHours.Value * leaveDuration;
            }
            else if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                GetActualTimeInAndOut();
                workHours = (decimal)(actualTimeOut - actualTimeIn).TotalMinutes;
                AdjustWorkHours();

                if (workHours > requiredWorkHours)
                {
                    regularWorkHours = requiredWorkHours;
                }
                else
                {
                    regularWorkHours = workHours;
                }

                if (expectedTimeOut < expectedTimeIn)
                {
                    expectedTimeOut = expectedTimeOut.AddDays(1);
                }


                bool isLegalHoliday = false;
                bool isSpecialHoliday = false;
                bool isRestDay = false;

                if (Holidays != null)
                {
                    foreach (var holiday in Holidays)
                    {
                        if (holiday.Type == (int)HolidayType.Legal) isLegalHoliday = true;
                        if (holiday.Type == (int)HolidayType.Special) isSpecialHoliday = true;
                    }
                }

                if (DTR.Shift.IsRestDay.HasValue) isRestDay = DTR.Shift.IsRestDay.Value;

                ComputeLate();
                ComputeUndertime();
                ComputeOvertime();
                ComputeNightDifferential();


                #region Legal and Special Holiday plus Rest Day
                if (isLegalHoliday && isSpecialHoliday && isRestDay)
                {
                    legalSpecialHolidayRestDay = workHours;
                    legalSpecialHolidayRestDayOvertime = totalOvertime;
                    approvedLegalSpecialHolidayRestDayOvertime = approvedOvertime;
                    legalSpecialHolidayRestDayNightDifferential = nightDifferential;
                    legalSpecialHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedLegalSpecialHolidayNightDifferentialOvertime = approvedOvertime;
                }
                #endregion

                #region Legal and Special Holiday
                else if (isLegalHoliday && isSpecialHoliday)
                {
                    legalSpecialHoliday = workHours;
                    legalSpecialHolidayOvertime = totalOvertime;
                    approvedLegalSpecialHolidayOvertime = approvedOvertime;
                    legalSpecialHolidayNightDifferential = nightDifferential;
                    legalSpecialHolidayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedLegalSpecialHolidayNightDifferentialOvertime = approvedOvertime;
                }
                #endregion

                #region Special Holiday and Rest Day
                else if (isSpecialHoliday && isRestDay)
                {
                    specialHolidayRestDay = workHours;
                    specialHolidayRestDayOvertime = totalOvertime;
                    approvedSpecialHolidayRestDayOvertime = approvedOvertime;
                    specialHolidayRestDayNightDifferential = nightDifferential;
                    specialHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedSpecialHolidayRestDayNightDifferentialOvertime = approvedOvertime;
                }
                #endregion

                #region Legal Holiday and Rest Day
                else if (isLegalHoliday && isRestDay)
                {
                    legalHolidayRestDay = workHours;
                    legalHolidayRestDayOvertime = totalOvertime;
                    approvedLegalHolidayRestDayOvertime = approvedOvertime;
                    legalHolidayRestDayNightDifferential = nightDifferential;
                    legalHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedLegalHolidayRestDayNightDifferentialOvertime = approvedOvertime;
                }
                #endregion

                #region Legal Holiday
                else if (isLegalHoliday)
                {
                    legalHoliday = workHours;
                    legalHolidayOvertime = totalOvertime;
                    approvedLegalHolidayOvertime = approvedOvertime;
                    legalHolidayNightDifferential = nightDifferential;
                    legalHolidayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedLegalHolidayOvertime = approvedOvertime;
                }
                #endregion

                #region Special Holiday
                else if (isSpecialHoliday)
                {
                    specialHoliday = workHours;
                    specialHolidayOvertime = totalOvertime;
                    approvedSpecialHolidayOvertime = approvedOvertime;
                    specialHolidayNightDifferential = nightDifferential;
                    specialHolidayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedSpecialHolidayNightDifferentialOvertime = approvedOvertime;
                }
                #endregion

                #region Rest Day
                else if (isRestDay)
                {
                    restDay = workHours;
                    restDayOvertime = totalOvertime;
                    approvedRestDayOvertime = approvedOvertime;
                    restDayNightDifferential = nightDifferential;
                    restDayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedRestDayNightDifferentialOvertime = approvedOvertime;
                }
                #endregion

            }
            else
            {
                if (!DTR.Shift.IsRestDay.HasValue || DTR.Shift.IsRestDay == false)
                {
                    absentHours = (decimal)(DTR.Shift.ScheduleOut.Value - DTR.Shift.ScheduleIn.Value).TotalMinutes - totalBreak;
                }
            }

            MapFieldsToDTR();
        }
    }
}
