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
        #endregion

        public DTRProcessorBase()
        {

        }

        protected void GetActualTimeInAndOut()
        {
            actualTimeIn = DateTimeHelpers.RemoveSeconds(DTR.TimeIn.Value);
            actualTimeOut = DateTimeHelpers.RemoveSeconds(DTR.TimeOut.Value);
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

        protected void GetLeaveDuration()
        {
            if (Leaves.Count() > 0)
            {
                leaveDuration = Leaves.OrderByDescending(e => e.LeaveHours).FirstOrDefault().LeaveHours;
            }
        }
        protected void GetRequiredWorkHours()
        {
            if (DTR.Shift.RequiredWorkHours.HasValue)
            {
                requiredWorkHours = DTR.Shift.RequiredWorkHours.Value;
            }
            else
            {
                requiredWorkHours = Math.Round(((decimal)(DTR.Shift.ScheduleOut.Value.RemoveSeconds() - DTR.Shift.ScheduleIn.Value.RemoveSeconds()).TotalMinutes - totalBreak) / 60, 2);
            }
        }
    }
}
