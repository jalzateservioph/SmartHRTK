using System;
using System.Collections.Generic;
using TKProcessor.Models.TK;
using TKProcessor.Common;
using System.Linq;

namespace TKProcessor.Services
{
    public interface IDTRProcessor
    {

        void ComputeRegular();
        void ComputeHolidayAndRestDay();

        DailyTransactionRecord DTR { get; set; }
    }

    public class DTRProcessorBase
    {

        public DailyTransactionRecord DTR { get; set; }
        protected IEnumerable<Holiday> Holidays;
        protected IEnumerable<Leave> Leaves;

        #region Initialization
        protected decimal requiredWorkHours = 0;
        protected decimal leaveDuration = 0;
        protected DateTime actualTimeIn;
        protected DateTime actualTimeOut;
        protected decimal regularWorkHours = 0;
        protected decimal totalBreak = 0;
        protected decimal workHours = 0;
        protected decimal totalOvertime = 0;
        protected decimal approvedOvertime = 0;
        protected decimal absentHours = 0;
        protected decimal undertime = 0;
        protected decimal approvedUndertime = 0;
        protected decimal preShiftOvertime = 0;
        protected decimal approvedPreShiftOvertime = 0;
        protected decimal postShiftOvertime = 0;
        protected decimal approvedPostShiftOvertime = 0;
        protected decimal late = 0;
        protected decimal approvedLate = 0;
        protected decimal nightDifferential = 0;
        protected decimal nightDifferentialOvertime = 0;

        protected decimal legalHoliday = 0;
        protected decimal legalHolidayOvertime = 0;
        protected decimal approvedLegalHolidayOvertime = 0;
        protected decimal legalHolidayNightDifferential = 0;
        protected decimal legalHolidayNightDifferentialOvertime = 0;
        protected decimal approvedLegalHolidayNightDifferentialOvertime = 0;

        protected decimal specialHoliday = 0;
        protected decimal specialHolidayOvertime = 0;
        protected decimal approvedSpecialHolidayOvertime = 0;
        protected decimal specialHolidayNightDifferential = 0;
        protected decimal specialHolidayNightDifferentialOvertime = 0;
        protected decimal approvedSpecialHolidayNightDifferentialOvertime = 0;

        protected decimal restDay = 0;
        protected decimal restDayOvertime = 0;
        protected decimal approvedRestDayOvertime = 0;
        protected decimal restDayNightDifferential = 0;
        protected decimal restDayNightDifferentialOvertime = 0;
        protected decimal approvedRestDayNightDifferentialOvertime = 0;

        protected decimal legalSpecialHoliday = 0;
        protected decimal legalSpecialHolidayOvertime = 0;
        protected decimal approvedLegalSpecialHolidayOvertime = 0;
        protected decimal legalSpecialHolidayNightDifferential = 0;
        protected decimal legalSpecialHolidayNightDifferentialOvertime = 0;
        protected decimal approvedLegalSpecialHolidayNightDifferentialOvertime = 0;

        protected decimal specialHolidayRestDay = 0;
        protected decimal specialHolidayRestDayOvertime = 0;
        protected decimal approvedSpecialHolidayRestDayOvertime = 0;
        protected decimal specialHolidayRestDayNightDifferential = 0;
        protected decimal specialHolidayRestDayNightDifferentialOvertime = 0;
        protected decimal approvedSpecialHolidayRestDayNightDifferentialOvertime = 0;

        protected decimal legalHolidayRestDay = 0;
        protected decimal legalHolidayRestDayOvertime = 0;
        protected decimal approvedLegalHolidayRestDayOvertime = 0;
        protected decimal legalHolidayRestDayNightDifferential = 0;
        protected decimal legalHolidayRestDayNightDifferentialOvertime = 0;
        protected decimal approvedLegalHolidayRestDayNightDifferentialOvertime = 0;

        protected decimal legalSpecialHolidayRestDay = 0;
        protected decimal legalSpecialHolidayRestDayOvertime = 0;
        protected decimal approvedLegalSpecialHolidayRestDayOvertime = 0;
        protected decimal legalSpecialHolidayRestDayNightDifferential = 0;
        protected decimal legalSpecialHolidayRestDayNightDifferentialOvertime = 0;
        protected decimal approvedLegalSpecialHolidayRestDayNightDifferential = 0;

        protected bool isLegalHoliday = false;
        protected bool isSpecialHoliday = false;
        protected bool isRestDay = false;

        protected DateTime? HalfDayPoint;

        protected DateTime? latePeriodStart;
        protected DateTime? latePeriodEnd;
        protected DateTime? undertimePeriodStart;
        protected DateTime? undertimePeriodEnd;

        protected DateTime? preShiftOvertimePeriodStart;
        protected DateTime? preShiftOvertimePeriodEnd;
        protected DateTime? postShiftOvertimePeriodStart;
        protected DateTime? postShiftOvertimePeriodEnd;
        protected DateTime? nightDifferentialPeriodStart;
        protected DateTime? nightDifferentialPeriodEnd;
        protected DateTime? nightDifferentialPreShiftOvertimePeriodStart;
        protected DateTime? nightDifferentialPreShiftOvertimePeriodEnd;
        protected DateTime? nightDifferentialPostShiftOvertimePeriodStart;
        protected DateTime? nightDifferentialPostShiftOvertimePeriodEnd;

        public bool IsSplittable = false;

        protected decimal splitHeadWorkHours = 0;
        protected decimal splitHeadLate = 0;
        protected decimal splitHeadUndertime = 0;
        protected decimal splitHeadAbsentHours = 0;
        protected decimal splitHeadRegularWorkHours = 0;
        protected decimal splitHeadPreShiftOvertime = 0;
        protected decimal splitHeadPostShiftOvertime = 0;
        protected decimal splitHeadTotalOvertime = 0;
        protected decimal splitHeadNightDifferential = 0;
        protected decimal splitHeadNightDifferentialPreShiftOvertime = 0;
        protected decimal splitHeadNightDifferentialPostShiftOvertime = 0;

        protected decimal splitTailWorkHours = 0;
        protected decimal splitTailLate = 0;
        protected decimal splitTailUndertime = 0;
        protected decimal splitTailAbsentHours = 0;
        protected decimal splitTailRegularWorkHours = 0;
        protected decimal splitTailPreShiftOvertime = 0;
        protected decimal splitTailPostShiftOvertime = 0;
        protected decimal splitTailTotalOvertime = 0;
        protected decimal splitTailNightDifferential = 0;
        protected decimal splitTailNightDifferentialPreShiftOvertime = 0;
        protected decimal splitTailNightDifferentialPostShiftOvertime = 0;
        #endregion

        public DTRProcessorBase()
        {

        }

        protected void GetActualTimeInAndOut()
        {
            actualTimeIn = DateTimeHelpers.RemoveSeconds(DTR.TimeIn.Value);
            actualTimeOut = DateTimeHelpers.RemoveSeconds(DTR.TimeOut.Value);

            if (actualTimeOut.Date > actualTimeIn.Date)
            {
                IsSplittable = true;
            }
        }

        protected decimal GetTotalBreakDuration()
        {
            decimal sum = 0;

            if (DTR.Shift.AmbreakIn.HasValue && DTR.Shift.AmbreakOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.AmbreakIn.Value.RemoveSeconds() - DTR.Shift.AmbreakOut.Value.RemoveSeconds()).TotalMinutes);
            }
            if (DTR.Shift.PmbreakIn.HasValue && DTR.Shift.PmbreakOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.PmbreakIn.Value.RemoveSeconds() - DTR.Shift.PmbreakOut.Value.RemoveSeconds()).TotalMinutes);
            }
            if (DTR.Shift.LunchIn.HasValue && DTR.Shift.LunchOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.LunchIn.Value.RemoveSeconds() - DTR.Shift.LunchOut.Value.RemoveSeconds()).TotalMinutes);
            }
            if (DTR.Shift.DinnerIn.HasValue && DTR.Shift.DinnerOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.DinnerIn.Value.RemoveSeconds() - DTR.Shift.DinnerOut.Value.RemoveSeconds()).TotalMinutes);
            }
            return sum;
        }

        protected void AdjustWorkHours()
        {
            if (DTR.Shift.AmbreakIn.HasValue && DTR.Shift.AmbreakOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakIn.Value.TimeOfDay))
                {
                    workHours -= Convert.ToDecimal((DTR.Shift.AmbreakIn.Value.RemoveSeconds() - DTR.Shift.AmbreakOut.Value.RemoveSeconds()).TotalMinutes);
                }
            }
            if (DTR.Shift.PmbreakIn.HasValue && DTR.Shift.PmbreakOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakIn.Value.TimeOfDay))
                {
                    workHours -= Convert.ToDecimal((DTR.Shift.PmbreakIn.Value.RemoveSeconds() - DTR.Shift.PmbreakOut.Value.RemoveSeconds()).TotalMinutes);
                }
            }
            if (DTR.Shift.LunchIn.HasValue && DTR.Shift.LunchOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchIn.Value.TimeOfDay))
                {
                    workHours -= Convert.ToDecimal((DTR.Shift.LunchIn.Value.RemoveSeconds() - DTR.Shift.LunchOut.Value.RemoveSeconds()).TotalMinutes);
                }
            }
            if (DTR.Shift.DinnerIn.HasValue && DTR.Shift.DinnerOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerIn.Value.TimeOfDay))
                {
                    workHours -= Convert.ToDecimal((DTR.Shift.DinnerIn.Value.RemoveSeconds() - DTR.Shift.DinnerOut.Value.RemoveSeconds()).TotalMinutes);
                }
            }
        }

        protected decimal AdjustWorkHours(DateTime timeIn, DateTime timeOut, decimal workHours) //for removing break hours of split DTRs
        {
            decimal adjustedWorkHours = workHours;
            if (DTR.Shift.AmbreakIn.HasValue && DTR.Shift.AmbreakOut.HasValue)
            {
                if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((DTR.Shift.AmbreakIn.Value.RemoveSeconds() - DTR.Shift.AmbreakOut.Value.RemoveSeconds()).TotalMinutes);
                }
                //else if (timeIn > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakIn.Value.TimeOfDay))
                //{
                //    adjustedWorkHours -= Convert.ToDecimal((new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakIn.Value.TimeOfDay) - timeIn).TotalMinutes);
                //}
                else if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakOut.Value.TimeOfDay) && timeOut < new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((timeOut - new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakOut.Value.TimeOfDay)).TotalMinutes);
                }
            }
            if (DTR.Shift.PmbreakIn.HasValue && DTR.Shift.PmbreakOut.HasValue)
            {
                if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((DTR.Shift.PmbreakIn.Value.RemoveSeconds() - DTR.Shift.PmbreakOut.Value.RemoveSeconds()).TotalMinutes);
                }
                //else if (timeIn > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakIn.Value.TimeOfDay))
                //{
                //    adjustedWorkHours -= Convert.ToDecimal((new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakIn.Value.TimeOfDay) - timeIn).TotalMinutes);
                //}
                else if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakOut.Value.TimeOfDay) && timeOut < new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((timeOut - new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakOut.Value.TimeOfDay)).TotalMinutes);
                }
            }
            if (DTR.Shift.LunchIn.HasValue && DTR.Shift.LunchOut.HasValue)
            {
                if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((DTR.Shift.LunchIn.Value.RemoveSeconds() - DTR.Shift.LunchOut.Value.RemoveSeconds()).TotalMinutes);
                }
                //else if (timeIn > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchIn.Value.TimeOfDay))
                //{
                //    adjustedWorkHours -= Convert.ToDecimal((new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchIn.Value.TimeOfDay) - timeIn).TotalMinutes);
                //}
                else if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchOut.Value.TimeOfDay) && timeOut < new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((timeOut - new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchOut.Value.TimeOfDay)).TotalMinutes);
                }
            }
            if (DTR.Shift.DinnerIn.HasValue && DTR.Shift.DinnerOut.HasValue)
            {
                if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((DTR.Shift.DinnerIn.Value.RemoveSeconds() - DTR.Shift.DinnerOut.Value.RemoveSeconds()).TotalMinutes);
                }
                //else if (timeIn > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerOut.Value.TimeOfDay) && timeOut >= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerIn.Value.TimeOfDay))
                //{
                //    adjustedWorkHours -= Convert.ToDecimal((new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerIn.Value.TimeOfDay) - timeIn).TotalMinutes);
                //}
                else if (timeIn <= new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerOut.Value.TimeOfDay) && timeOut < new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerIn.Value.TimeOfDay))
                {
                    adjustedWorkHours -= Convert.ToDecimal((timeOut - new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerOut.Value.TimeOfDay)).TotalMinutes);
                }
            }

            return adjustedWorkHours;
        }

        protected void MapFieldsToDTR()
        {
            DTR.RegularWorkHours = Math.Round(regularWorkHours / 60, 2) + 0.00m;
            DTR.WorkHours = Math.Round(workHours / 60, 2) + 0.00m;
            DTR.ApprovedLate = DTR.ActualLate = Math.Round(late / 60, 2) + 0.00m;
            DTR.ActualOvertime = Math.Round(totalOvertime / 60, 2) + 0.00m;
            DTR.ApprovedOvertime = Math.Round(approvedOvertime / 60, 2) + 0.00m;
            DTR.AbsentHours = Math.Round(absentHours / 60, 2) + 0.00m;

            DTR.ApprovedUndertime = DTR.ActualUndertime = Math.Round(undertime / 60, 2) + 0.00m;
            DTR.ApprovedPreOvertime = DTR.ActualPreOvertime = Math.Round(preShiftOvertime / 60, 2) + 0.00m;
            DTR.ApprovedPostOvertime = DTR.ActualPostOvertime = Math.Round(postShiftOvertime / 60, 2) + 0.00m;

            DTR.NightDifferential = Math.Round(nightDifferential / 60, 2) + 0.00m;
            DTR.NightDifferentialOt = Math.Round(nightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedLegHol = DTR.ActualLegHol = Math.Round(legalHoliday / 60, 2) + 0.00m;
            DTR.ActualLegHolOt = Math.Round(legalHolidayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDLegHol = DTR.ActualNDLegHol = Math.Round(legalHolidayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDLegHolOt = DTR.ActualNDLegHolOt = Math.Round(legalHolidayNightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedSpeHol = DTR.ActualSpeHol = Math.Round(specialHoliday / 60, 2) + 0.00m;
            DTR.ApprovedSpeHolOt = DTR.ActualSpeHolOt = Math.Round(specialHolidayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDSpeHol = DTR.ActualNDSpeHol = Math.Round(specialHolidayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDSpeHolOt = DTR.ActualNDSpeHolOt = Math.Round(specialHolidayNightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedRestDay = DTR.ActualRestDay = Math.Round(restDay / 60, 2) + 0.00m;
            DTR.ApprovedRestDayOt = DTR.ActualRestDayOt = Math.Round(restDayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDRD = DTR.ActualNDRD = Math.Round(restDayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDRDot = DTR.ActualNDRDot = Math.Round(restDayNightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedLegSpeHol = DTR.ActualLegSpeHol = Math.Round(legalSpecialHoliday / 60, 2) + 0.00m;
            DTR.ApprovedLegSpeHolOt = DTR.ActualLegSpeHolOt = Math.Round(legalSpecialHolidayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDLegSpeHol = DTR.ActualNDLegSpeHol = Math.Round(legalSpecialHolidayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDLegSpeHolOt = DTR.ActualNDLegSpeHolOt = Math.Round(legalSpecialHolidayNightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedSpeHolRD = DTR.ActualSpeHolRD = Math.Round(specialHolidayRestDay / 60, 2) + 0.00m;
            DTR.ApprovedSpeHolRDot = DTR.ActualSpeHolRDot = Math.Round(specialHolidayRestDayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDSpeHolRD = DTR.ActualNDSpeHolRD = Math.Round(specialHolidayRestDayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDSpeHolRDot = DTR.ActualNDSpeHolRDot = Math.Round(specialHolidayRestDayNightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedLegHolRD = DTR.ActualLegHolRD = Math.Round(legalHolidayRestDay / 60, 2) + 0.00m;
            DTR.ApprovedLegHolRDot = DTR.ActualLegHolRDot = Math.Round(legalHolidayRestDayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDLegHolRD = DTR.ActualNDLegHolRD = Math.Round(legalHolidayRestDayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDLegHolRDot = DTR.ActualNDLegHolRDot = Math.Round(legalHolidayRestDayNightDifferentialOvertime / 60, 2) + 0.00m;

            DTR.ApprovedLegSpeHolRD = DTR.ActualLegSpeHolRD = Math.Round(legalSpecialHolidayRestDay / 60, 2) + 0.00m;
            DTR.ApprovedLegSpeHolRDot = DTR.ActualLegSpeHolRDot = Math.Round(legalSpecialHolidayRestDayOvertime / 60, 2) + 0.00m;
            DTR.ApprovedNDLegSpeHolRD = DTR.ActualNDLegSpeHolRD = Math.Round(legalSpecialHolidayRestDayNightDifferential / 60, 2) + 0.00m;
            DTR.ApprovedNDLegSpeHolRDot = DTR.ActualNDLegSpeHolRDot = Math.Round(legalSpecialHolidayRestDayNightDifferentialOvertime / 60, 2) + 0.00m;

        }

        protected void MapFields() //new map
        {
            DTR.WorkHours = Math.Round(workHours / 60, 2) + 0.00m;
            if (DTR.Shift.IsRestDay.HasValue && DTR.Shift.IsRestDay.Value)
            {
                DTR.ActualRestDay = Math.Round(regularWorkHours / 60, 2) + 0.00m;
                //DTR.ApprovedRestDay
                DTR.ActualRestDayOt = Math.Round(totalOvertime / 60, 2) + 0.00m;
                //DTR.ApprovedRestDayOt
                DTR.ActualNDRD = Math.Round(nightDifferential / 60, 2) + 0.00m;
                //DTR.ApprovedNDRD
                DTR.ActualNDRDot = Math.Round(nightDifferentialOvertime / 60, 2) + 0.00m;
                //DTR.ApprovedNDRDot
            }
            else
            {
                DTR.RegularWorkHours = Math.Round(regularWorkHours / 60, 2) + 0.00m;

                DTR.ActualLate = Math.Round(late / 60, 2) + 0.00m;
                DTR.ApprovedLate = Math.Round(approvedLate / 60, 2) + 0.00m;

                DTR.ActualUndertime = Math.Round(undertime / 60, 2) + 0.00m;
                DTR.ApprovedUndertime = Math.Round(approvedUndertime / 60, 2) + 0.00m;

                DTR.ActualOvertime = Math.Round(totalOvertime / 60, 2) + 0.00m;
                DTR.ApprovedOvertime = Math.Round(approvedOvertime / 60, 2) + 0.00m;

                DTR.NightDifferential = Math.Round(nightDifferential / 60, 2) + 0.00m;
                DTR.NightDifferentialOt = Math.Round(nightDifferentialOvertime / 60, 2) + 0.00m;
            }
        }

        protected void GetLeaveDuration()
        {
            if (Leaves != null && Leaves.Count() > 0)
            {
                leaveDuration = Leaves.OrderByDescending(e => e.LeaveHours).FirstOrDefault().LeaveHours;
            }
        }

        public Tuple<DailyTransactionRecord, DailyTransactionRecord> Split(DailyTransactionRecord DTR)
        {

            DailyTransactionRecord head = new DailyTransactionRecord()
            {
                WorkHours = Math.Round(splitHeadWorkHours / 60, 2),
                ActualLate = Math.Round(splitHeadLate / 60, 2),
                ActualUndertime = Math.Round(splitHeadUndertime / 60, 2),
                AbsentHours = Math.Round(splitHeadAbsentHours / 60, 2),
                ActualPreOvertime = Math.Round(splitHeadPreShiftOvertime / 60, 2),
                ActualPostOvertime = Math.Round(splitHeadPostShiftOvertime / 60, 2)
            };

            DailyTransactionRecord tail = new DailyTransactionRecord()
            {
                WorkHours = Math.Round(splitTailWorkHours / 60, 2),
                ActualLate = Math.Round(splitTailLate / 60, 2),
                ActualUndertime = Math.Round(splitTailUndertime / 60, 2),
                AbsentHours = Math.Round(splitTailAbsentHours / 60, 2),
                ActualPreOvertime = Math.Round(splitTailPreShiftOvertime / 60, 2),
                ActualPostOvertime = Math.Round(splitTailPostShiftOvertime / 60, 2)
            };

            if (DTR.Shift.IsRestDay.HasValue && DTR.Shift.IsRestDay.Value)
            {
                head.ActualRestDay = Math.Round(splitHeadRegularWorkHours / 60, 2);
                head.ActualRestDayOt = Math.Round(splitHeadTotalOvertime / 60, 2);
                head.ActualNDRD = Math.Round(splitHeadNightDifferential / 60, 2);
                head.ActualNDRDot = Math.Round((splitHeadNightDifferentialPreShiftOvertime + splitHeadNightDifferentialPostShiftOvertime) / 60, 2);

                tail.ActualRestDay = Math.Round(splitTailRegularWorkHours / 60, 2);
                tail.ActualRestDayOt = Math.Round(splitTailTotalOvertime / 60, 2);
                tail.ActualNDRD = Math.Round(splitTailNightDifferential / 60, 2);
                tail.ActualNDRDot = Math.Round((splitTailNightDifferentialPreShiftOvertime + splitTailNightDifferentialPostShiftOvertime) / 60, 2);
            }
            else
            {
                head.RegularWorkHours = Math.Round(splitHeadRegularWorkHours / 60, 2);
                head.ActualOvertime = Math.Round(splitHeadTotalOvertime / 60, 2);
                head.NightDifferential = Math.Round(splitHeadNightDifferential / 60, 2);
                head.NightDifferentialOt = Math.Round((splitHeadNightDifferentialPreShiftOvertime + splitHeadNightDifferentialPostShiftOvertime) / 60, 2);

                tail.RegularWorkHours = Math.Round(splitTailRegularWorkHours / 60, 2);
                tail.ActualOvertime = Math.Round(splitTailTotalOvertime / 60, 2);
                tail.NightDifferential = Math.Round(splitTailNightDifferential / 60, 2);
                tail.NightDifferentialOt = Math.Round((splitTailNightDifferentialPreShiftOvertime + splitTailNightDifferentialPostShiftOvertime) / 60, 2);
            }
            return Tuple.Create(head, tail);
        }
    }
}
