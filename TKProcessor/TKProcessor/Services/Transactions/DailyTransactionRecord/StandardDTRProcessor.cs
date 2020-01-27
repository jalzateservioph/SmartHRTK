using System;
using System.Collections.Generic;
using TKProcessor.Common;
using TKProcessor.Models.TK;
using System.Linq;
namespace TKProcessor.Services
{
    public class StandardDTRProcessor : DTRProcessorBase, IDTRProcessor
    {
        DateTime expectedTimeIn;
        DateTime expectedTimeOut;

        private void GetRequiredWorkHours()
        {
            requiredWorkHours = Math.Round(((decimal)(expectedTimeOut - expectedTimeIn).TotalMinutes - totalBreak) / 60, 2);
        }

        public StandardDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Leave> leaves) : base()
        {
            this.DTR = DTR;
            Leaves = leaves;
            totalBreak = GetTotalBreakDuration();
            GetLeaveDuration();
            GetRequiredWorkHours();
            expectedTimeIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleIn.Value.TimeOfDay);
            expectedTimeOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleOut.Value.TimeOfDay);

            if (expectedTimeOut < expectedTimeIn)
            {
                expectedTimeOut = expectedTimeOut.AddDays(1);
                IsSplittable = true;
            }
        }

        public StandardDTRProcessor(DailyTransactionRecord DTR, IEnumerable<Leave> leaves, IEnumerable<Holiday> holidays) : base()
        {
            this.DTR = DTR;
            Leaves = leaves;
            this.Holidays = holidays;
            totalBreak = GetTotalBreakDuration();
            GetLeaveDuration();
            expectedTimeIn = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleIn.Value.TimeOfDay).RemoveSeconds();
            expectedTimeOut = new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.ScheduleOut.Value.TimeOfDay).RemoveSeconds();
            if (expectedTimeOut < expectedTimeIn)
            {
                expectedTimeOut = expectedTimeOut.AddDays(1);
                IsSplittable = true;
            }
            GetRequiredWorkHours();
            HalfDayPoint = expectedTimeIn.AddHours((double)(expectedTimeOut - expectedTimeIn).TotalHours / 2).RemoveSeconds();

            if (Holidays != null)
            {
                foreach (var holiday in Holidays)
                {
                    if (holiday.Type == (int)HolidayType.Legal) isLegalHoliday = true;
                    if (holiday.Type == (int)HolidayType.Special) isSpecialHoliday = true;
                }
            }

            //if (DTR.Shift.IsRestDay.HasValue) isRestDay = DTR.Shift.IsRestDay.Value; Not used at all
        }

        public void Compute()
        {
            if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
            {
                GetActualTimeInAndOut();

                workHours = (decimal)(actualTimeOut - actualTimeIn).TotalMinutes;

                if (IsSplittable)
                {
                    //Split

                    if (actualTimeIn.Date == expectedTimeIn.Date)
                    {
                        if (actualTimeOut >= expectedTimeOut.Date) //If actual time out crosses to next day
                        {
                            splitHeadWorkHours = (decimal)(expectedTimeOut.Date - actualTimeIn).TotalMinutes;
                            if (actualTimeIn < expectedTimeIn)
                            {
                                splitHeadRegularWorkHours = (decimal)(expectedTimeOut.Date - expectedTimeIn).TotalMinutes;
                            }
                            else
                            {
                                splitHeadRegularWorkHours = (decimal)(expectedTimeOut.Date - actualTimeIn).TotalMinutes;
                            }
                        }
                        else
                        {
                            splitHeadWorkHours = workHours;
                            if (actualTimeIn < expectedTimeIn)
                            {
                                splitHeadRegularWorkHours = (decimal)(actualTimeOut - expectedTimeIn).TotalMinutes;
                            }
                            else
                            {
                                splitHeadRegularWorkHours = (decimal)(actualTimeOut - actualTimeIn).TotalMinutes;
                            }
                        }

                    }
                    if (actualTimeOut.Date == expectedTimeOut.Date)
                    {
                        if (actualTimeIn < expectedTimeOut.Date) //if actual time in is from the previous day
                        {
                            splitTailWorkHours = (decimal)(actualTimeOut - expectedTimeOut.Date).TotalMinutes;
                            if (actualTimeOut > expectedTimeOut)
                            {
                                splitTailRegularWorkHours = (decimal)(expectedTimeOut - expectedTimeOut.Date).TotalMinutes;
                            }
                            else
                            {
                                splitTailRegularWorkHours = (decimal)(actualTimeOut - expectedTimeOut.Date).TotalMinutes;
                            }
                        }
                        else
                        {
                            splitTailWorkHours = workHours;
                            if (actualTimeOut < expectedTimeOut)
                            {
                                splitTailRegularWorkHours = (decimal)(actualTimeOut - expectedTimeOut.Date).TotalMinutes;
                            }
                            else
                            {
                                splitTailRegularWorkHours = (decimal)(expectedTimeOut - actualTimeIn).TotalMinutes;
                            }
                        }
                    }

                    splitHeadWorkHours = AdjustWorkHours(actualTimeIn, actualTimeIn.AddDays(1).Date, splitHeadWorkHours);
                    splitTailWorkHours = AdjustWorkHours(actualTimeOut.Date, actualTimeOut, splitTailWorkHours);
                    splitHeadRegularWorkHours = AdjustWorkHours(actualTimeIn, actualTimeIn.AddDays(1).Date, splitHeadRegularWorkHours);
                    splitTailRegularWorkHours = AdjustWorkHours(actualTimeOut.Date, actualTimeOut, splitTailRegularWorkHours);

                }

                AdjustWorkHours();

                if (workHours > requiredWorkHours * 60)
                {
                    regularWorkHours = requiredWorkHours * 60;
                }
                else
                {
                    regularWorkHours = workHours;
                }

                ComputeLate();

                ComputeUndertime();

                if (leaveDuration > 0)
                {
                    if (regularWorkHours + requiredWorkHours * leaveDuration * 60 >= requiredWorkHours * 60)
                        regularWorkHours = requiredWorkHours * 60;
                    else
                        regularWorkHours += requiredWorkHours * leaveDuration * 60;
                }
                else
                {
                    if (regularWorkHours >= requiredWorkHours * 60)
                        ComputeOvertime();
                }

                ComputeNightDifferential();
            }
            else
            {
                //No TimeIn and TimeOut.. Handle absent hours here [abs]
                if ((!DTR.Shift.IsRestDay.HasValue || DTR.Shift.IsRestDay == false) && (Holidays == null || Holidays.Count() == 0))
                {
                    absentHours = requiredWorkHours * 60;

                    if (IsSplittable)
                    {
                        splitHeadAbsentHours = AdjustWorkHours(expectedTimeIn, expectedTimeOut.Date, (decimal)(expectedTimeOut.Date - expectedTimeIn).TotalMinutes);
                        splitTailAbsentHours = AdjustWorkHours(expectedTimeOut.Date, expectedTimeOut, (decimal)(expectedTimeOut - expectedTimeOut.Date).TotalMinutes);
                    }
                }
            }

            //MapFieldsToDTR();
            MapFields();
        }

        public void ComputeRegular()
        {
            if (leaveDuration == 1M || leaveDuration == 0.5M)
            {
                workHours = DTR.Shift.RequiredWorkHours.Value * leaveDuration;
            }

            if (DTR.TimeIn.HasValue && DTR.TimeOut.HasValue)
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

                ComputeLate();
                ComputeUndertime();
                //ComputeOvertime();

                if (leaveDuration > 0)
                {
                    if (regularWorkHours + requiredWorkHours * leaveDuration * 60 >= requiredWorkHours * 60)
                        regularWorkHours = requiredWorkHours * 60;
                    else
                        regularWorkHours += requiredWorkHours * leaveDuration * 60;
                }
                else
                {
                    if (regularWorkHours >= requiredWorkHours * 60)
                        ComputeOvertime();
                }

                ComputeNightDifferential();
            }
            else
            {
                absentHours = (decimal)(DTR.Shift.ScheduleOut.Value.RemoveSeconds() - DTR.Shift.ScheduleIn.Value.RemoveSeconds()).TotalMinutes - totalBreak;
            }

            MapFieldsToDTR();
        }

        private void AdjustForLeaves()
        {
            if (late > 0)
            {

            }
            else if (undertime > 0)
            {

            }
        }

        private void ComputeLate()
        {
            if (actualTimeIn > expectedTimeIn)
                late = (decimal)(actualTimeIn - expectedTimeIn).TotalMinutes;

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
                    if (actualTimeIn > expectedTimeIn.AddMinutes(gracePeriodMinutes))
                    {
                        approvedLate = late;

                        if (DTR.Shift.IsPlusLateInMinutes == false)
                            approvedLate -= gracePeriodMinutes;
                    }
                    else
                    {
                        regularWorkHours += late;
                    }

                    latePeriodStart = expectedTimeIn;
                    latePeriodEnd = actualTimeIn;

                    if (IsSplittable)
                    {
                        if (latePeriodEnd.Value.Date > latePeriodStart.Value.Date)
                        {
                            splitTailLate += (decimal)(latePeriodEnd.Value.RemoveSeconds() - latePeriodEnd.Value.Date).TotalMinutes;
                            splitHeadLate += (decimal)(latePeriodStart.Value.Date.AddDays(1) - latePeriodStart.Value.RemoveSeconds()).TotalMinutes;
                        }
                        else
                        {
                            splitHeadLate += (decimal)(latePeriodEnd.Value.RemoveSeconds() - latePeriodStart.Value.RemoveSeconds()).TotalMinutes;
                        }
                    }
                }

                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfDay.HasValue && DTR.Shift.MaximumMinutesConsideredAsHalfDay.Value > 0)
                {
                    if (late > DTR.Shift.MaximumMinutesConsideredAsHalfDay.Value)
                    {
                        absentHours = Math.Round(requiredWorkHours * 60 / 2, 2);

                        if (IsSplittable)
                        {
                            if (HalfDayPoint.Value.Date > expectedTimeIn.Date)
                            {
                                splitHeadAbsentHours += AdjustWorkHours(expectedTimeIn, HalfDayPoint.Value.Date, absentHours); //assuming that shift is split exactly in half.
                                splitTailAbsentHours += AdjustWorkHours(HalfDayPoint.Value.Date, HalfDayPoint.Value, absentHours);
                            }
                            else
                            {
                                splitHeadAbsentHours += AdjustWorkHours(expectedTimeIn, HalfDayPoint.Value, absentHours); //assuming that shift is split exactly in half.
                            }
                        }

                        approvedLate = 0;
                    }
                }
                #endregion
            }
        }

        private void ComputeUndertime()
        {
            if (actualTimeOut < expectedTimeOut)
                undertime = (decimal)((expectedTimeOut - actualTimeOut).TotalMinutes);

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
                    if (actualTimeOut < expectedTimeOut.AddMinutes(-gracePeriodMinutes))
                    {
                        approvedUndertime = undertime;

                        if (DTR.Shift.IsPlusEarlyOutMinutes == false)
                            approvedUndertime -= gracePeriodMinutes;
                    }
                    else
                    {
                        regularWorkHours += undertime;
                    }

                    undertimePeriodStart = actualTimeOut;
                    undertimePeriodEnd = expectedTimeOut;

                    if (actualTimeOut.Date > actualTimeIn.Date)
                    {
                        if (undertimePeriodEnd.Value.Date > undertimePeriodStart.Value.Date)
                        {
                            splitTailUndertime += (decimal)(undertimePeriodEnd.Value - undertimePeriodEnd.Value.Date).TotalMinutes;
                            splitHeadUndertime += (decimal)(undertimePeriodStart.Value.Date.AddDays(1) - undertimePeriodStart.Value).TotalMinutes;
                        }
                        else
                        {
                            splitTailUndertime += (decimal)(undertimePeriodEnd.Value.RemoveSeconds() - undertimePeriodStart.Value.RemoveSeconds()).TotalMinutes;
                        }
                    }
                }

                #region Half day
                if (DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.HasValue && DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.Value > 0)
                {
                    if (undertime > DTR.Shift.MaximumMinutesConsideredAsHalfAayEarlyOut.Value)
                    {
                        absentHours = Math.Round(requiredWorkHours * 60 / 2, 2);
                        if (IsSplittable)
                        {
                            if (HalfDayPoint.Value.Date < expectedTimeOut.Date)
                            {
                                splitHeadAbsentHours += AdjustWorkHours(HalfDayPoint.Value, expectedTimeOut.Date, absentHours);
                                splitTailAbsentHours += AdjustWorkHours(expectedTimeOut.Date, expectedTimeOut, absentHours);
                            }
                            else
                            {
                                splitTailAbsentHours += AdjustWorkHours(expectedTimeIn, HalfDayPoint.Value, absentHours);
                            }
                        }
                        approvedUndertime = 0;
                    }
                }
                #endregion
            }
        }

        private void ComputeOvertime()
        {
            #region Pre-Overtime
            if (actualTimeIn < expectedTimeIn)
                preShiftOvertime = (decimal)(expectedTimeIn - actualTimeIn).TotalMinutes;

            if (DTR.Shift.IsPreShiftOt == true)
            {
                if (actualTimeIn < expectedTimeIn)
                {
                    preShiftOvertime = (decimal)(expectedTimeIn - actualTimeIn).TotalMinutes;
                    approvedPreShiftOvertime = preShiftOvertime;
                    preShiftOvertimePeriodStart = actualTimeIn;
                    preShiftOvertimePeriodEnd = expectedTimeIn;

                    if (IsSplittable)
                    {
                        if (preShiftOvertimePeriodEnd.Value.Date > preShiftOvertimePeriodStart.Value.Date)
                        {
                            splitTailPreShiftOvertime += (decimal)(preShiftOvertimePeriodEnd.Value - preShiftOvertimePeriodEnd.Value.Date).TotalMinutes;
                            splitHeadPreShiftOvertime += (decimal)(preShiftOvertimePeriodStart.Value.AddDays(1).Date - preShiftOvertimePeriodStart.Value).TotalMinutes;

                            splitTailTotalOvertime += splitTailPreShiftOvertime;
                            splitHeadTotalOvertime += splitHeadPreShiftOvertime;
                        }
                        else
                        {
                            splitHeadPreShiftOvertime += (decimal)(preShiftOvertimePeriodEnd.Value - preShiftOvertimePeriodStart.Value).TotalMinutes;
                            splitHeadTotalOvertime += splitHeadPreShiftOvertime;
                        }
                    }
                }

                if (DTR.Shift.MinimumPreShiftOt.HasValue && DTR.Shift.MinimumPreShiftOt.Value > preShiftOvertime)
                {
                    approvedPreShiftOvertime = 0;
                }

                if (DTR.Shift.MaximumPreShiftOt.HasValue && (DTR.Shift.MaximumPreShiftOt.Value < preShiftOvertime || DTR.Shift.MaximumPreShiftOt == 0))
                {
                    approvedPreShiftOvertime = DTR.Shift.MaximumPreShiftOt.Value;
                }

                if (DTR.Shift.RoundPreShiftOt.HasValue && approvedPreShiftOvertime < DTR.Shift.RoundPreShiftOt.Value && DTR.Shift.RoundPreShiftOt > 1)
                {
                    approvedPreShiftOvertime = approvedPreShiftOvertime - (approvedPreShiftOvertime % DTR.Shift.RoundPreShiftOt.Value);
                }

                approvedOvertime += approvedPreShiftOvertime;
            }

            totalOvertime += preShiftOvertime;
            #endregion

            #region Post-Overtime
            if (actualTimeOut > expectedTimeOut)
                postShiftOvertime = (decimal)(actualTimeOut - expectedTimeOut).TotalMinutes;

            if (DTR.Shift.IsPostShiftOt == true)
            {
                if (actualTimeOut > expectedTimeOut)
                {
                    postShiftOvertime = (decimal)(actualTimeOut - expectedTimeOut).TotalMinutes;
                    approvedPostShiftOvertime = postShiftOvertime;
                    postShiftOvertimePeriodStart = expectedTimeOut;
                    postShiftOvertimePeriodEnd = actualTimeOut;


                    if (IsSplittable)
                    {
                        if (postShiftOvertimePeriodEnd.Value.Date > postShiftOvertimePeriodStart.Value.Date)
                        {
                            splitTailPostShiftOvertime += (decimal)(postShiftOvertimePeriodEnd.Value - postShiftOvertimePeriodEnd.Value.Date).TotalMinutes;
                            splitHeadPostShiftOvertime += (decimal)(postShiftOvertimePeriodStart.Value.AddDays(1).Date - postShiftOvertimePeriodStart.Value).TotalMinutes;

                            splitTailTotalOvertime += splitTailPostShiftOvertime;
                            splitHeadTotalOvertime += splitHeadPostShiftOvertime;
                        }
                        else
                        {
                            splitTailPostShiftOvertime += (decimal)(postShiftOvertimePeriodEnd.Value - postShiftOvertimePeriodStart.Value).TotalMinutes;
                            splitTailTotalOvertime += splitTailPostShiftOvertime;
                        }
                    }
                }

                if (DTR.Shift.MinimumPostShiftOt.HasValue && DTR.Shift.MinimumPostShiftOt.Value > postShiftOvertime)
                {
                    approvedPostShiftOvertime = 0;
                }

                if (DTR.Shift.MaximumPostShiftOt.HasValue && DTR.Shift.MaximumPostShiftOt.Value < postShiftOvertime)
                {
                    approvedPostShiftOvertime = DTR.Shift.MaximumPostShiftOt.Value;
                }

                if (DTR.Shift.RoundPostShiftOt.HasValue && approvedPostShiftOvertime > DTR.Shift.RoundPostShiftOt.Value && DTR.Shift.RoundPostShiftOt > 1)
                {
                    approvedPostShiftOvertime = approvedPostShiftOvertime - (approvedPostShiftOvertime % DTR.Shift.RoundPostShiftOt.Value);
                }

                approvedOvertime += approvedPostShiftOvertime;
            }

            totalOvertime += postShiftOvertime;
            #endregion
        }

        private decimal GetDuration(DateTime transactionDate, DateTime actualTimein, DateTime actualTimeout, DateTime start, DateTime end)
        {
            decimal duration = 0m;

            if (((actualTimein <= start && actualTimein <= end) &&
                 (actualTimeout >= start && actualTimeout >= end)) ||
              actualTimein == start && actualTimeout == end)
            {
                duration += Convert.ToDecimal((end.RemoveSeconds().TimeOfDay - start.RemoveSeconds().TimeOfDay).TotalMinutes);
            }
            else
            {
                if( (actualTimein.TimeOfDay > start.TimeOfDay && actualTimein.TimeOfDay < end.TimeOfDay) || (actualTimein.TimeOfDay > start.TimeOfDay && actualTimein.TimeOfDay < end.TimeOfDay))
                {
                    duration += Convert.ToDecimal((end.RemoveSeconds().TimeOfDay - actualTimein.RemoveSeconds().TimeOfDay).TotalMinutes);
                }
                if (actualTimeout > start && actualTimein < end)
                {
                    duration += Convert.ToDecimal((actualTimeout.RemoveSeconds().TimeOfDay - start.RemoveSeconds().TimeOfDay).TotalMinutes);
                }
            }

            return duration;
        }

        private void ComputeNightDifferential()
        {
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
                    nightDifferential = AdjustWorkHours(actualTimeIn, expectedNightDifferentialEnd, nightDifferential);
                    nightDifferentialPeriodStart = actualTimeIn;
                    nightDifferentialPeriodEnd = expectedNightDifferentialEnd;
                }
                else if (expectedNightDifferentialStart >= actualTimeIn && expectedNightDifferentialEnd <= actualTimeOut)
                {
                    nightDifferential = (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                    nightDifferential = AdjustWorkHours(expectedNightDifferentialStart, expectedNightDifferentialEnd, nightDifferential);
                    nightDifferentialPeriodStart = expectedNightDifferentialStart;
                    nightDifferentialPeriodEnd = expectedNightDifferentialEnd;
                }
                else if (expectedNightDifferentialStart < actualTimeOut && actualTimeOut < expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(actualTimeOut - expectedNightDifferentialStart).TotalMinutes;
                    nightDifferential = AdjustWorkHours(expectedNightDifferentialStart, actualTimeOut, nightDifferential);
                    nightDifferentialPeriodStart = expectedNightDifferentialStart;
                    nightDifferentialPeriodEnd = actualTimeOut;
                }
                else if (actualTimeIn >= expectedNightDifferentialStart && actualTimeOut <= expectedNightDifferentialEnd)
                {
                    nightDifferential = (decimal)(actualTimeOut - actualTimeIn).TotalMinutes;
                    nightDifferential = AdjustWorkHours(actualTimeIn, actualTimeOut, nightDifferential);
                    nightDifferentialPeriodStart = actualTimeIn;
                    nightDifferentialPeriodEnd = actualTimeOut;
                }

                if (IsSplittable)
                {
                    if (nightDifferentialPeriodStart.HasValue && nightDifferentialPeriodEnd.HasValue)
                    {
                        if (nightDifferentialPeriodEnd.Value.Date > nightDifferentialPeriodStart.Value.Date)
                        {
                            splitTailNightDifferential += (decimal)(nightDifferentialPeriodEnd.Value - nightDifferentialPeriodEnd.Value.Date).TotalMinutes;
                            splitTailNightDifferential = AdjustWorkHours(nightDifferentialPeriodEnd.Value.Date, nightDifferentialPeriodEnd.Value, splitTailNightDifferential);
                            splitHeadNightDifferential += (decimal)(nightDifferentialPeriodStart.Value.AddDays(1).Date - nightDifferentialPeriodStart.Value).TotalMinutes;
                            splitHeadNightDifferential = AdjustWorkHours(nightDifferentialPeriodStart.Value, nightDifferentialPeriodStart.Value.AddDays(1).Date, splitHeadNightDifferential);
                        }
                        else
                        {
                            splitHeadNightDifferential += (decimal)(nightDifferentialPeriodEnd.Value - nightDifferentialPeriodStart.Value).TotalMinutes;
                            splitHeadNightDifferential = AdjustWorkHours(nightDifferentialPeriodStart.Value, nightDifferentialPeriodEnd.Value, splitHeadNightDifferential);
                        }
                    }
                }

                #region Night Differential Overtime

                #region Pre-shift Overtime
                if (DTR.Shift.IsPreShiftOt == true)
                {
                    //if (expectedTimeIn > actualTimeIn)
                    //{
                    //    if (expectedNightDifferentialEnd >= expectedTimeIn)
                    //    {
                    //        if (expectedNightDifferentialStart < actualTimeIn)
                    //        {
                    //            nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedTimeIn).TotalMinutes;
                    //            nightDifferentialPreShiftOvertimePeriodStart = expectedTimeIn;
                    //            nightDifferentialPreShiftOvertimePeriodEnd = expectedNightDifferentialEnd;
                    //        }
                    //        else
                    //        {
                    //            nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                    //            nightDifferentialPreShiftOvertimePeriodStart = expectedNightDifferentialStart;
                    //            nightDifferentialPreShiftOvertimePeriodEnd = expectedNightDifferentialEnd;
                    //        }
                    //    }
                    //    else if (expectedNightDifferentialEnd > actualTimeIn)
                    //    {
                    //        if (expectedNightDifferentialStart <= actualTimeIn)
                    //        {
                    //            nightDifferentialOvertime += (decimal)(actualTimeIn - expectedNightDifferentialStart).TotalMinutes;
                    //            nightDifferentialPreShiftOvertimePeriodStart = expectedNightDifferentialStart;
                    //            nightDifferentialPreShiftOvertimePeriodEnd = actualTimeIn;
                    //        }
                    //        else
                    //        {
                    //            nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                    //            nightDifferentialPreShiftOvertimePeriodStart = expectedNightDifferentialStart;
                    //            nightDifferentialPreShiftOvertimePeriodEnd = expectedNightDifferentialEnd;
                    //        }
                    //    }

                    //    if (IsSplittable)
                    //    {
                    //        if (nightDifferentialPreShiftOvertimePeriodStart.HasValue && nightDifferentialPreShiftOvertimePeriodEnd.HasValue)
                    //        {
                    //            if (nightDifferentialPreShiftOvertimePeriodEnd.Value.Date > nightDifferentialPreShiftOvertimePeriodStart.Value.Date)
                    //            {
                    //                splitTailNightDifferentialPreShiftOvertime += (decimal)(nightDifferentialPreShiftOvertimePeriodEnd.Value - nightDifferentialPreShiftOvertimePeriodEnd.Value.Date).TotalMinutes;
                    //                splitHeadNightDifferentialPreShiftOvertime += (decimal)(nightDifferentialPreShiftOvertimePeriodStart.Value.AddDays(1).Date - nightDifferentialPreShiftOvertimePeriodStart.Value).TotalMinutes;
                    //            }
                    //            else
                    //            { 
                    //                splitHeadNightDifferentialPreShiftOvertime += (decimal)(nightDifferentialPreShiftOvertimePeriodEnd.Value - nightDifferentialPreShiftOvertimePeriodStart.Value).TotalMinutes;
                    //            }
                    //        }
                    //    }
                    //}

                    if (preShiftOvertimePeriodStart.HasValue && preShiftOvertimePeriodEnd.HasValue && nightDifferentialPeriodStart.HasValue && nightDifferentialPeriodEnd.HasValue)
                    {
                        if (nightDifferentialPeriodStart.Value >= preShiftOvertimePeriodStart.Value)
                        {
                            if (nightDifferentialPeriodEnd.Value < preShiftOvertimePeriodEnd.Value)
                            {
                                nightDifferentialOvertime += (decimal)(nightDifferentialPeriodEnd.Value - nightDifferentialPeriodStart.Value).TotalMinutes;
                                nightDifferentialPreShiftOvertimePeriodStart = nightDifferentialPeriodStart;
                                nightDifferentialPreShiftOvertimePeriodEnd = nightDifferentialPeriodEnd;
                            }
                            else if (preShiftOvertimePeriodEnd.Value >= nightDifferentialPeriodStart.Value)
                            {
                                nightDifferentialOvertime += (decimal)(preShiftOvertimePeriodEnd.Value - nightDifferentialPeriodStart.Value).TotalMinutes;
                                nightDifferentialPreShiftOvertimePeriodStart = nightDifferentialPeriodStart;
                                nightDifferentialPreShiftOvertimePeriodEnd = preShiftOvertimePeriodEnd;
                            }
                        }
                        else
                        {
                            if (nightDifferentialPeriodEnd.Value < preShiftOvertimePeriodEnd.Value)
                            {
                                nightDifferentialOvertime += (decimal)(nightDifferentialPeriodEnd.Value - preShiftOvertimePeriodStart.Value).TotalMinutes;
                                nightDifferentialPreShiftOvertimePeriodStart = preShiftOvertimePeriodStart;
                                nightDifferentialPreShiftOvertimePeriodEnd = nightDifferentialPeriodEnd;
                            }
                            else if (preShiftOvertimePeriodEnd.Value >= nightDifferentialPeriodEnd.Value)
                            {
                                nightDifferentialOvertime += (decimal)(preShiftOvertimePeriodEnd.Value - preShiftOvertimePeriodStart.Value).TotalMinutes;
                                nightDifferentialPreShiftOvertimePeriodStart = preShiftOvertimePeriodStart;
                                nightDifferentialPreShiftOvertimePeriodEnd = preShiftOvertimePeriodEnd;
                            }
                        }

                        if (IsSplittable)
                        {
                            if (nightDifferentialPreShiftOvertimePeriodStart.HasValue && nightDifferentialPreShiftOvertimePeriodEnd.HasValue)
                            {
                                if (nightDifferentialPreShiftOvertimePeriodEnd.Value.Date > nightDifferentialPreShiftOvertimePeriodStart.Value.Date)
                                {
                                    splitTailNightDifferentialPreShiftOvertime += (decimal)(nightDifferentialPreShiftOvertimePeriodEnd.Value - nightDifferentialPreShiftOvertimePeriodEnd.Value.Date).TotalMinutes;
                                    splitHeadNightDifferentialPreShiftOvertime += (decimal)(nightDifferentialPreShiftOvertimePeriodStart.Value.AddDays(1).Date - nightDifferentialPreShiftOvertimePeriodStart.Value).TotalMinutes;
                                }
                                else
                                {
                                    splitHeadNightDifferentialPreShiftOvertime += (decimal)(nightDifferentialPreShiftOvertimePeriodEnd.Value - nightDifferentialPreShiftOvertimePeriodStart.Value).TotalMinutes;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Post-shift Overtime
                if (DTR.Shift.IsPostShiftOt == true)
                {
                    //if (actualTimeOut > expectedTimeOut)
                    //{
                    //    if (expectedNightDifferentialStart <= expectedTimeOut)
                    //    {
                    //        if (expectedNightDifferentialEnd > actualTimeOut)
                    //        {
                    //            nightDifferentialOvertime += (decimal)(actualTimeOut - expectedTimeOut).TotalMinutes;
                    //            nightDifferentialPostShiftOvertimePeriodStart = expectedTimeOut;
                    //            nightDifferentialPostShiftOvertimePeriodEnd = actualTimeOut;
                    //        }
                    //        else
                    //        {
                    //            nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedTimeOut).TotalMinutes;
                    //            nightDifferentialPostShiftOvertimePeriodStart = expectedTimeOut;
                    //            nightDifferentialPostShiftOvertimePeriodEnd = expectedNightDifferentialEnd;
                    //        }
                    //    }
                    //    else if (expectedNightDifferentialStart < actualTimeOut)
                    //    {
                    //        if (expectedNightDifferentialEnd > actualTimeOut)
                    //        {
                    //            nightDifferentialOvertime += (decimal)(actualTimeOut - expectedNightDifferentialStart).TotalMinutes;
                    //            nightDifferentialPostShiftOvertimePeriodStart = expectedNightDifferentialStart;
                    //            nightDifferentialPostShiftOvertimePeriodEnd = actualTimeOut;
                    //        }
                    //        else
                    //        {
                    //            nightDifferentialOvertime += (decimal)(expectedNightDifferentialEnd - expectedNightDifferentialStart).TotalMinutes;
                    //            nightDifferentialPostShiftOvertimePeriodStart = expectedNightDifferentialStart;
                    //            nightDifferentialPostShiftOvertimePeriodEnd = expectedNightDifferentialEnd;
                    //        }
                    //    }

                    //    if (IsSplittable)
                    //    {
                    //        if (nightDifferentialPostShiftOvertimePeriodStart.HasValue && nightDifferentialPostShiftOvertimePeriodEnd.HasValue)
                    //        {
                    //            if (nightDifferentialPostShiftOvertimePeriodEnd.Value.Date > nightDifferentialPostShiftOvertimePeriodStart.Value.Date)
                    //            {
                    //                splitTailNightDifferentialPostShiftOvertime += (decimal)(nightDifferentialPostShiftOvertimePeriodEnd.Value - nightDifferentialPostShiftOvertimePeriodEnd.Value.Date).TotalMinutes;
                    //                splitHeadNightDifferentialPostShiftOvertime += (decimal)(nightDifferentialPostShiftOvertimePeriodStart.Value.AddDays(1).Date - nightDifferentialPostShiftOvertimePeriodStart.Value).TotalMinutes;
                    //            }
                    //            else
                    //            {
                    //                splitHeadNightDifferentialPostShiftOvertime += (decimal)(nightDifferentialPostShiftOvertimePeriodEnd.Value - nightDifferentialPostShiftOvertimePeriodStart.Value).TotalMinutes;
                    //            }
                    //        }
                    //    }
                    //}

                    if (postShiftOvertimePeriodStart.HasValue && postShiftOvertimePeriodEnd.HasValue && nightDifferentialPeriodStart.HasValue && nightDifferentialPeriodEnd.HasValue)
                    {
                        if (nightDifferentialPeriodStart.Value >= postShiftOvertimePeriodStart.Value)
                        {
                            if (nightDifferentialPeriodEnd.Value < postShiftOvertimePeriodEnd.Value)
                            {
                                nightDifferentialOvertime += (decimal)(nightDifferentialPeriodEnd.Value - nightDifferentialPeriodStart.Value).TotalMinutes;
                                nightDifferentialPostShiftOvertimePeriodStart = nightDifferentialPeriodStart;
                                nightDifferentialPostShiftOvertimePeriodEnd = nightDifferentialPeriodEnd;
                            }
                            else if (postShiftOvertimePeriodEnd.Value > nightDifferentialPeriodStart.Value)
                            {
                                nightDifferentialOvertime += (decimal)(postShiftOvertimePeriodEnd.Value - nightDifferentialPeriodStart.Value).TotalMinutes;
                                nightDifferentialPostShiftOvertimePeriodStart = nightDifferentialPeriodStart;
                                nightDifferentialPostShiftOvertimePeriodEnd = postShiftOvertimePeriodEnd;
                            }
                        }
                        else
                        {
                            if (nightDifferentialPeriodEnd.Value < postShiftOvertimePeriodEnd.Value)
                            {
                                nightDifferentialOvertime += (decimal)(nightDifferentialPeriodEnd.Value - postShiftOvertimePeriodStart.Value).TotalMinutes;
                                nightDifferentialPostShiftOvertimePeriodStart = postShiftOvertimePeriodStart;
                                nightDifferentialPostShiftOvertimePeriodEnd = nightDifferentialPeriodEnd;
                            }
                            else if (postShiftOvertimePeriodEnd.Value > nightDifferentialPeriodStart.Value)
                            {
                                nightDifferentialOvertime += (decimal)(postShiftOvertimePeriodEnd.Value - postShiftOvertimePeriodStart.Value).TotalMinutes;
                                nightDifferentialPostShiftOvertimePeriodStart = postShiftOvertimePeriodStart;
                                nightDifferentialPostShiftOvertimePeriodEnd = postShiftOvertimePeriodEnd;
                            }
                        }

                        if (IsSplittable)
                        {
                            if (nightDifferentialPostShiftOvertimePeriodStart.HasValue && nightDifferentialPostShiftOvertimePeriodEnd.HasValue)
                            {
                                if (nightDifferentialPostShiftOvertimePeriodEnd.Value.Date > nightDifferentialPostShiftOvertimePeriodStart.Value.Date)
                                {
                                    splitTailNightDifferentialPostShiftOvertime += (decimal)(nightDifferentialPostShiftOvertimePeriodEnd.Value - nightDifferentialPostShiftOvertimePeriodEnd.Value.Date).TotalMinutes;
                                    splitHeadNightDifferentialPostShiftOvertime += (decimal)(nightDifferentialPostShiftOvertimePeriodStart.Value.AddDays(1).Date - nightDifferentialPostShiftOvertimePeriodStart.Value).TotalMinutes;
                                }
                                else
                                {
                                    splitTailNightDifferentialPostShiftOvertime += (decimal)(nightDifferentialPostShiftOvertimePeriodEnd.Value - nightDifferentialPostShiftOvertimePeriodStart.Value).TotalMinutes;
                                }
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
                    legalSpecialHolidayRestDay = regularWorkHours;
                    legalSpecialHolidayRestDayOvertime = totalOvertime;
                    approvedLegalSpecialHolidayRestDayOvertime = approvedOvertime;
                    legalSpecialHolidayRestDayNightDifferential = nightDifferential;
                    legalSpecialHolidayRestDayNightDifferentialOvertime = nightDifferentialOvertime;
                    approvedLegalSpecialHolidayRestDayNightDifferentialOvertime = approvedOvertime;
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
                    absentHours = (decimal)(DTR.Shift.ScheduleOut.Value.RemoveSeconds() - DTR.Shift.ScheduleIn.Value.RemoveSeconds()).TotalMinutes - totalBreak;
                }
            }

            MapFieldsToDTR();
        }
    }
}
