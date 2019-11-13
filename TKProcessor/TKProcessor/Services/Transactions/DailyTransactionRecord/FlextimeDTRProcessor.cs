using System;
using System.Collections.Generic;
using System.Linq;
using TKProcessor.Common;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class FlextimeDTRProcessor : DTRProcessorBase, IDTRProcessor
    {
        private void GetRequiredWorkHours()
        {
            if (DTR.Shift.RequiredWorkHours.HasValue)
            {
                requiredWorkHours = DTR.Shift.RequiredWorkHours.Value;
            }
        }
        public FlextimeDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Leave> leaves) : base()
        {
            this.DTR = DTR;
            Leaves = leaves;
            totalBreak = GetTotalBreakDuration();
            GetLeaveDuration();
            GetRequiredWorkHours();
        }

        public FlextimeDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Leave> leaves, IEnumerable<Holiday> holidays) : base()
        {
            this.DTR = DTR;
            this.Holidays = holidays;
            Leaves = leaves;
            totalBreak = GetTotalBreakDuration();
            GetLeaveDuration();
            GetRequiredWorkHours();
        }

        public void ComputeRegular()
        {
            if (leaveDuration == 1M || leaveDuration == 0.5M)
            {
                workHours = requiredWorkHours * leaveDuration;
            }
            else if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                GetActualTimeInAndOut();
                workHours = Convert.ToDecimal((actualTimeOut - actualTimeIn).TotalMinutes);
                AdjustWorkHours();

                if (workHours > requiredWorkHours * 60)
                {
                    regularWorkHours = requiredWorkHours * 60;
                }
                else
                {
                    regularWorkHours = workHours;
                }

                #region Full Flex
                if (DTR.Shift.FlextimeType.Value == (int)FlextimeType.Full)
                {
                    FullFlex();
                }
                #endregion

                #region Semi - On the dot
                else if (DTR.Shift.FlextimeType.Value == (int)FlextimeType.SemiOnTheDot)
                {
                    SemiOnTheDot();
                }
                #endregion

                #region Semi - Fixed Increments
                else if (DTR.Shift.FlextimeType.Value == (int)FlextimeType.SemiFixedIncrements)
                {
                    SemiFixedIncrements();
                }
                #endregion
            }
            else
            {
                absentHours = (decimal)(requiredWorkHours * 60);
            }


            MapFieldsToDTR();
        }

        public void ComputeHolidayAndRestDay()
        {

            if (leaveDuration == 1M || leaveDuration == 0.5M)
            {
                workHours = requiredWorkHours * leaveDuration;
            }
            else if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                workHours = Convert.ToDecimal((actualTimeOut - actualTimeIn).TotalMinutes);
                AdjustWorkHours();

                if (workHours > requiredWorkHours * 60)
                {
                    regularWorkHours = requiredWorkHours * 60;
                }
                else
                {
                    regularWorkHours = workHours;
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


                #region Legal and Special Holiday plus Rest Day
                if (isLegalHoliday && isSpecialHoliday && isRestDay)
                {
                    legalSpecialHolidayRestDay = regularWorkHours;
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
                    legalSpecialHoliday = regularWorkHours;
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
                    specialHolidayRestDay = regularWorkHours;
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
                    legalHolidayRestDay = regularWorkHours;
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
                    legalHoliday = regularWorkHours;
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
                    specialHoliday = regularWorkHours;
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
                    restDay = regularWorkHours;
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
                    absentHours = requiredWorkHours * 60;
                }
            }


            MapFieldsToDTR();
        }

        private void FullFlex()
        {
            DateTime expectedTimeOut = actualTimeIn.AddMinutes((double)(requiredWorkHours + totalBreak));

            #region Undertime
            if (DTR.Shift.IsEarlyOut == true)
            {
                int gracePeriodMinutes = 0;
                if (DTR.Shift.GracePeriodEarlyOut.HasValue) gracePeriodMinutes = DTR.Shift.GracePeriodEarlyOut.Value;
                if (workHours < requiredWorkHours * 60)
                {
                    undertime = (requiredWorkHours * 60) - workHours;
                    if (workHours < (requiredWorkHours * 60) - gracePeriodMinutes)
                    {
                        approvedUndertime = undertime;
                    }
                }
                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.HasValue)
                {
                    if (undertime > DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.Value)
                    {
                        absentHours = Math.Round((requiredWorkHours * 60) / 2, 2);
                        approvedUndertime = 0;
                    }
                }
                #endregion

            }
            #endregion

            #region Post-Overtime
            if (DTR.Shift.IsPostShiftOt == true)
            {
                if (workHours > requiredWorkHours * 60)
                {
                    postShiftOvertime = workHours - (requiredWorkHours * 60);
                    approvedPostShiftOvertime = postShiftOvertime;

                    if (DTR.Shift.MinimumPostShiftOt.HasValue && DTR.Shift.MinimumPostShiftOt.Value > postShiftOvertime)
                    {
                        approvedPostShiftOvertime = 0;
                    }
                    else if (DTR.Shift.MaximumPostShiftOt.HasValue && DTR.Shift.MaximumPostShiftOt.Value < postShiftOvertime)
                    {
                        approvedPostShiftOvertime = DTR.Shift.MaximumPostShiftOt.Value;
                    }
                    totalOvertime += postShiftOvertime;
                    approvedOvertime += approvedPostShiftOvertime;
                }
            }
            #endregion

            #region Night Differential
            if (DTR.Shift.NightDiffStart.HasValue && DTR.Shift.NightDiffEnd.HasValue)
            {
                DateTime expectedNightDifferentialStart = new DateTime(DTR.TransactionDate.Value.Year,
                                                                       DTR.TransactionDate.Value.Month,
                                                                       DTR.TransactionDate.Value.Day).Add(DTR.Shift.NightDiffStart.Value.TimeOfDay).RemoveSeconds();

                DateTime expectedNightDifferentialEnd = new DateTime(DTR.TransactionDate.Value.Year,
                                                                     DTR.TransactionDate.Value.Month,
                                                                     DTR.TransactionDate.Value.Day).Add(DTR.Shift.NightDiffEnd.Value.TimeOfDay).RemoveSeconds();

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
            #endregion
        }

        private void SemiOnTheDot()
        {
            DateTime latestIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LatestTimeIn.Value.TimeOfDay);

            /*
             * expectedOut should be time in + total required work hours + total break
             * example
             * RequiredWorkHours = 7.5 hours
             * AM Break = 15 minutes
             * PM Break = 15 minutes
             * Lunch Break = 1 hour
             * Dinner Break = 0
             * Total Break = (.25 + .25 + 1 + 0) = 1.5
             * 
             */

            //DateTime earliestOut = new DateTime(DTR.TimeIn.Value.Year, DTR.TimeIn.Value.Month, DTR.TimeIn.Value.Day).AddHours((double) DTR.Shift.RequiredWorkHours.Value + (double) totalBreak);
            DateTime expectedTimeOut = actualTimeIn //Time in
                .AddMinutes((double)((requiredWorkHours * 60) + totalBreak)); //Required work hours + total break
            var shiftEarliestTimeOut = DTR.Shift.EarliestTimeOut.Value;
            var shiftEarliestTimeIn = DTR.Shift.EarliestTimeIn.Value;
            DateTime earliestOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(shiftEarliestTimeOut.TimeOfDay);
            DateTime earliestIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(shiftEarliestTimeIn.TimeOfDay);

            if (earliestOut < earliestIn)
            {
                earliestOut.AddDays(1);
            }

            /*
             * example
             * Time in = 6:45am
             * Shift earliest time out = 4:00pm
             * Expected time out = 3:45pm
             * Actual time out = 3:45pm
             * 
             * should be undertime
             * 
             */
            if (expectedTimeOut < earliestOut)
            {
                expectedTimeOut = earliestOut;
            }

            #region Late
            if (DTR.Shift.IsLateIn == true)
            {
                int gracePeriodMinutes = 0;
                //DateTime latestIn = DTR.Shift.LatestTimeIn.Value.TimeOfDay;
                if (DTR.Shift.GracePeriodLateIn.HasValue)
                {
                    gracePeriodMinutes = DTR.Shift.GracePeriodLateIn.Value;
                }
                if (actualTimeIn > latestIn)
                {
                    late = Convert.ToDecimal((actualTimeIn - latestIn).TotalMinutes);
                    if (actualTimeIn > latestIn.AddMinutes(gracePeriodMinutes))
                    {
                        approvedLate = late;
                    }
                }

                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfDay.HasValue)
                {
                    if (late > DTR.Shift.MaximumMinutesConsideredAsHalfDay.Value)
                    {
                        absentHours = Math.Round(((decimal)(DTR.Shift.ScheduleOut.Value.RemoveSeconds() - DTR.Shift.ScheduleIn.Value.RemoveSeconds()).TotalMinutes - totalBreak) / 2, 2);
                    }
                }
                #endregion
            }
            #endregion

            #region Undertime
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
                    //DTR.ActualUndertime = DTR.Shift.RequiredWorkHours.Value + totalBreak - DTR.WorkHours;
                    /*
                     * example
                     * Shift earliest time out = 4:00pm
                     * DTR.TimeOut = 3:45pm
                     * expectedOut = 4:00pm
                     * 
                     * DTR.ActualUndertime should be 15
                     */
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
                        absentHours = Math.Round((requiredWorkHours * 60) / 2, 2);
                        approvedUndertime = 0;
                    }
                }
                #endregion
            }
            #endregion

            #region Overtime
            if (workHours > requiredWorkHours * 60)
            {
                #region Pre-Overtime
                if (DTR.Shift.IsPreShiftOt == true)
                {
                    if (actualTimeIn < earliestIn)
                    {
                        preShiftOvertime = (decimal)(earliestIn - actualTimeIn).TotalMinutes;
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
            #endregion

            #region Night Differential
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
                    if (earliestIn > actualTimeIn)
                    {
                        if (expectedNightDifferentialEnd >= earliestIn)
                        {
                            if (expectedNightDifferentialStart < actualTimeIn)
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - earliestIn).TotalMinutes;
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
                                nightDifferentialOvertime += (decimal)(actualTimeIn - expectedNightDifferentialStart).TotalMinutes;
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
            #endregion
        }

        private void SemiFixedIncrements()
        {
            DateTime earliestIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.EarliestTimeIn.Value.TimeOfDay);
            DateTime latestIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LatestTimeIn.Value.TimeOfDay);

            if (latestIn < earliestIn)
            {
                latestIn = latestIn.AddDays(1);
            }

            //DateTime earliestOut = new DateTime(DTR.TimeIn.Value.Year, DTR.TimeIn.Value.Month, DTR.TimeIn.Value.Day).Add(DTR.Shift.EarliestTimeOut.Value.TimeOfDay);
            //DateTime latestOut = new DateTime(DTR.TimeIn.Value.Year, DTR.TimeIn.Value.Month, DTR.TimeIn.Value.Day).Add(DTR.Shift.LatestTimeOut.Value.TimeOfDay);

            Increment i = (Increment)DTR.Shift.Increment.Value;

            DateTime windowIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day);
            DateTime windowOut;


            #region Late
            IEnumerable<DateTime> timeInWindow = GetTimeRange(earliestIn, latestIn, i);
            for (var x = 0; x < timeInWindow.Count(); x++)
            {
                DateTime point = timeInWindow.ElementAt(x);
                int gracePeriodMinutes = 0;
                if (DTR.Shift.GracePeriodLateIn.HasValue)
                {
                    //point.AddMinutes(DTR.Shift.GracePeriodLateIn.Value);
                    gracePeriodMinutes = DTR.Shift.GracePeriodLateIn.Value;
                }

                if (actualTimeIn < point.AddMinutes(gracePeriodMinutes))
                {
                    windowIn = timeInWindow.ElementAt(x);
                    break; //It should find the right time in at this point
                }

                if (x == timeInWindow.Count() - 1) //if current item is last and DTR.TimeIn is still greater than point, compute late
                {
                    windowIn = timeInWindow.ElementAt(x);
                    if (DTR.Shift.IsLateIn == true)
                    {
                        late = Convert.ToDecimal((actualTimeIn - latestIn).TotalMinutes);

                        if (actualTimeIn > point.AddMinutes(gracePeriodMinutes))
                        {
                            approvedLate = late;
                        }

                        #region Half day
                        if (DTR.Shift.MaximumMinutesConsideredAsHalfDay.HasValue)
                        {
                            if (late > DTR.Shift.MaximumMinutesConsideredAsHalfDay.Value)
                            {
                                absentHours = Math.Round((requiredWorkHours * 60) / 2, 2);
                            }
                        }
                        #endregion
                    }
                }

            }
            #endregion

            windowOut = windowIn.AddMinutes((double)((requiredWorkHours * 60) + totalBreak));

            #region Undertime
            if (DTR.Shift.IsEarlyOut == true)
            {
                int gracePeriodMinutes = 0;
                if (DTR.Shift.GracePeriodEarlyOut.HasValue)
                {
                    //expectedOut.AddMinutes(-DTR.Shift.GracePeriodEarlyOut.Value);
                    gracePeriodMinutes = DTR.Shift.GracePeriodEarlyOut.Value;
                }

                if (actualTimeOut < windowOut)
                {
                    undertime = (decimal)(windowOut - actualTimeOut).TotalMinutes;

                    if (actualTimeOut < windowOut.AddMinutes(-gracePeriodMinutes))
                    {
                        approvedUndertime = undertime;
                    }
                }

                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.HasValue)
                {
                    if (undertime > DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.Value)
                    {
                        absentHours = Math.Round((requiredWorkHours * 60) / 2, 2);
                        approvedUndertime = 0;
                    }
                }
                #endregion
            }
            #endregion

            #region Overtime
            if (workHours > requiredWorkHours * 60)
            {
                #region Pre-Overtime
                if (DTR.Shift.IsPreShiftOt == true)
                {
                    preShiftOvertime = 0;

                    if (actualTimeIn < windowIn)
                    {
                        preShiftOvertime = (decimal)(windowIn - actualTimeIn).TotalMinutes;
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

                    if (actualTimeOut > windowOut)
                    {
                        postShiftOvertime = (decimal)(actualTimeOut - windowOut).TotalMinutes;
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
            #endregion

            #region Night Differential
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
                    if (windowIn > actualTimeIn)
                    {
                        if (expectedNightDifferentialEnd >= windowIn)
                        {
                            if (expectedNightDifferentialStart < actualTimeIn)
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - windowIn).TotalMinutes;
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
                                nightDifferentialOvertime += (decimal)(actualTimeIn - expectedNightDifferentialStart).TotalMinutes;
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
                    if (actualTimeOut > windowOut)
                    {
                        if (expectedNightDifferentialStart <= windowOut)
                        {
                            if (expectedNightDifferentialEnd > actualTimeOut)
                            {
                                nightDifferentialOvertime += (decimal)(actualTimeOut - windowOut).TotalMinutes;
                            }
                            else
                            {
                                nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - windowOut).TotalMinutes;
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
            #endregion
        }

        private IEnumerable<DateTime> GetTimeRange(DateTime earliest, DateTime latest, Increment increment)
        {
            DateTime currentItem = earliest;

            do
            {
                yield return currentItem;
                if (increment == Increment.HalfHour) currentItem = currentItem.AddMinutes(30);
                if (increment == Increment.WholeHour) currentItem = currentItem.AddMinutes(60);
            }
            while (currentItem <= latest);
        }
    }
}
