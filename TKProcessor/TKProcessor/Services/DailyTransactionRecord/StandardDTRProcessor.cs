﻿using System;
using System.Collections.Generic;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class StandardDTRProcessor : DTRProcessorBase, IDTRProcessor
    {
        DateTime expectedTimeIn;
        DateTime expectedTimeOut;

        public StandardDTRProcessor(DailyTransactionRecord DTR) : base()
        {
            this.DTR = DTR;
            totalBreak = GetTotalBreakDuration();
            expectedTimeIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleIn.Value.TimeOfDay);
            expectedTimeOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleOut.Value.TimeOfDay);
        }

        public StandardDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Holiday> holidays) : base()
        {
            this.DTR = DTR;
            this.Holidays = holidays;
            totalBreak = GetTotalBreakDuration();
            expectedTimeIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleIn.Value.TimeOfDay);
            expectedTimeOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleOut.Value.TimeOfDay);
        }

        public void ComputeRegular()
        {
            if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                workHours = (decimal)(DTR.TimeOut.Value - DTR.TimeIn.Value).TotalMinutes;
                AdjustWorkHours();

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
                if (DTR.Shift.IsLateIn == true)
                {
                    int gracePeriodMinutes = 0;
                    //DateTime latestIn = DTR.Shift.LatestTimeIn.Value.TimeOfDay;
                    if (DTR.Shift.GracePeriodLateIn.HasValue)
                    {
                        gracePeriodMinutes = DTR.Shift.GracePeriodLateIn.Value;
                    }
                    if (DTR.TimeIn.Value > expectedTimeIn)
                    {
                        late = (decimal)(DTR.TimeIn.Value - expectedTimeIn).TotalMinutes;

                        if (DTR.TimeIn.Value > expectedTimeIn.AddMinutes(gracePeriodMinutes))
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
                if (DTR.TimeOut.Value < expectedTimeOut)
                {
                    undertime = (decimal)((expectedTimeOut - DTR.TimeOut.Value).TotalMinutes);

                    if (DTR.TimeOut.Value < expectedTimeOut.AddMinutes(-gracePeriodMinutes))
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
                if (DTR.TimeIn < expectedTimeIn)
                {
                    preShiftOvertime = (decimal)(expectedTimeIn - DTR.TimeIn.Value).TotalMinutes;
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
                if (DTR.TimeOut > expectedTimeOut)
                {
                    postShiftOvertime = (decimal)(DTR.TimeOut.Value - expectedTimeOut).TotalMinutes;
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

                if (DTR.TimeIn >= expectedNightDifferentialStart && DTR.TimeIn < expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(expectedNightDifferentialEnd - DTR.TimeIn.Value).TotalMinutes;
                }
                else if (expectedNightDifferentialStart >= DTR.TimeIn && expectedNightDifferentialEnd <= DTR.TimeOut)
                {
                    nightDifferential = (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                }
                else if (expectedNightDifferentialStart < DTR.TimeOut && DTR.TimeOut < expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(DTR.TimeOut.Value - expectedNightDifferentialStart).TotalMinutes;
                }
                else if (DTR.TimeIn >= expectedNightDifferentialStart && DTR.TimeOut <= expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(DTR.TimeOut.Value - DTR.TimeIn.Value).TotalMinutes;
                }

                #region Night Differential Overtime

                #region Pre-shift Overtime
                if (DTR.Shift.IsPreShiftOt == true)
                {
                    if (expectedTimeIn > DTR.TimeIn)
                    {
                        if (expectedNightDifferentialEnd >= expectedTimeIn)
                        {
                            if (expectedNightDifferentialStart < DTR.TimeIn)
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedTimeIn).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                            }
                        }
                        else if (expectedNightDifferentialEnd > DTR.TimeIn)
                        {
                            if (expectedNightDifferentialStart <= DTR.TimeIn)
                            {
                                nightDifferentialOvertime += (decimal)(DTR.TimeIn.Value - expectedNightDifferentialStart).TotalMinutes;
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
                    if (DTR.TimeOut > expectedTimeOut)
                    {
                        if (expectedNightDifferentialStart <= expectedTimeOut)
                        {
                            if (expectedNightDifferentialEnd > DTR.TimeOut)
                            {
                                nightDifferentialOvertime += (decimal)(DTR.TimeOut.Value - expectedTimeOut).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedTimeOut).TotalMinutes;
                            }
                        }
                        else if (expectedNightDifferentialStart < DTR.TimeOut)
                        {
                            if (expectedNightDifferentialEnd > DTR.TimeOut)
                            {
                                nightDifferentialOvertime += (decimal)(DTR.TimeOut.Value - expectedNightDifferentialStart).TotalMinutes;
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
            if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                workHours = (decimal)(DTR.TimeOut.Value - DTR.TimeIn.Value).TotalMinutes;
                AdjustWorkHours();

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
                    legalSpecialHolidayRestDayNightDifferential = nightDifferential;
                    legalSpecialHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                }
                #endregion

                #region Legal and Special Holiday
                else if (isLegalHoliday && isSpecialHoliday)
                {
                    legalSpecialHoliday = workHours;
                    legalSpecialHolidayOvertime = totalOvertime;
                    legalSpecialHolidayNightDifferential = nightDifferential;
                    legalSpecialHolidayNightDifferentialOvertime = nightDifferentialOvertime;
                }
                #endregion

                #region Special Holiday and Rest Day
                else if (isSpecialHoliday && isRestDay)
                {
                    specialHolidayRestDay = workHours;
                    specialHolidayRestDayOvertime = totalOvertime;
                    specialHolidayRestDayNightDifferential = nightDifferential;
                    specialHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                }
                #endregion

                #region Legal Holiday and Rest Day
                else if (isLegalHoliday && isRestDay)
                {
                    legalHolidayRestDay = workHours;
                    legalHolidayRestDayOvertime = totalOvertime;
                    legalHolidayRestDayNightDifferential = nightDifferential;
                    legalHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                }
                #endregion

                #region Legal Holiday
                else if (isLegalHoliday)
                {
                    legalHoliday = workHours;
                    legalHolidayOvertime = totalOvertime;
                    legalHolidayNightDifferential = nightDifferential;
                    legalHolidayNightDifferentialOvertime = nightDifferentialOvertime;
                }
                #endregion

                #region Special Holiday
                else if (isSpecialHoliday)
                {
                    specialHoliday = workHours;
                    specialHolidayOvertime = totalOvertime;
                    specialHolidayNightDifferential = nightDifferential;
                    specialHolidayNightDifferentialOvertime = nightDifferentialOvertime;
                }
                #endregion

                #region Rest Day
                else if (isRestDay)
                {
                    restDay = workHours;
                    restDayOvertime = totalOvertime;
                    restDayNightDifferential = nightDifferential;
                    restDayNightDifferentialOvertime = nightDifferentialOvertime;
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
