﻿using System;
using System.Collections.Generic;
using TKProcessor.Models.TK;

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

        #region Initialization
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
        protected decimal legalHolidayNightDifferential = 0;
        protected decimal legalHolidayNightDifferentialOvertime = 0;

        protected decimal specialHoliday = 0;
        protected decimal specialHolidayOvertime = 0;
        protected decimal specialHolidayNightDifferential = 0;
        protected decimal specialHolidayNightDifferentialOvertime = 0;

        protected decimal restDay = 0;
        protected decimal restDayOvertime = 0;
        protected decimal restDayNightDifferential = 0;
        protected decimal restDayNightDifferentialOvertime = 0;

        protected decimal legalSpecialHoliday = 0;
        protected decimal legalSpecialHolidayOvertime = 0;
        protected decimal legalSpecialHolidayNightDifferential = 0;
        protected decimal legalSpecialHolidayNightDifferentialOvertime = 0;

        protected decimal specialHolidayRestDay = 0;
        protected decimal specialHolidayRestDayOvertime = 0;
        protected decimal specialHolidayRestDayNightDifferential = 0;
        protected decimal specialHolidayRestDayNightDifferentialOvertime = 0;

        protected decimal legalHolidayRestDay = 0;
        protected decimal legalHolidayRestDayOvertime = 0;
        protected decimal legalHolidayRestDayNightDifferential = 0;
        protected decimal legalHolidayRestDayNightDifferentialOvertime = 0;

        protected decimal legalSpecialHolidayRestDay = 0;
        protected decimal legalSpecialHolidayRestDayOvertime = 0;
        protected decimal legalSpecialHolidayRestDayNightDifferential = 0;
        protected decimal legalSpecialHolidayRestDayNightDifferentialOvertime = 0;
        #endregion

        public DTRProcessorBase()
        {

        }

        public decimal GetTotalBreakDuration()
        {
            decimal sum = 0;

            if (DTR.Shift.AmbreakIn.HasValue && DTR.Shift.AmbreakOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.AmbreakIn.Value - DTR.Shift.AmbreakOut.Value).TotalMinutes);
            }
            if (DTR.Shift.PmbreakIn.HasValue && DTR.Shift.PmbreakOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.PmbreakIn.Value - DTR.Shift.PmbreakOut.Value).TotalMinutes);
            }
            if (DTR.Shift.LunchIn.HasValue && DTR.Shift.LunchOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.LunchIn.Value - DTR.Shift.LunchOut.Value).TotalMinutes);
            }
            if (DTR.Shift.DinnerIn.HasValue && DTR.Shift.DinnerOut.HasValue)
            {
                sum += Convert.ToDecimal((DTR.Shift.DinnerIn.Value - DTR.Shift.DinnerOut.Value).TotalMinutes);
            }
            return sum;
        }

        public void AdjustWorkHours()
        {
            if (DTR.Shift.AmbreakIn.HasValue && DTR.Shift.AmbreakOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.AmbreakIn.Value.TimeOfDay))
                { 
                    workHours -= Convert.ToDecimal((DTR.Shift.AmbreakIn.Value - DTR.Shift.AmbreakOut.Value).TotalMinutes);
                }
            }
            if (DTR.Shift.PmbreakIn.HasValue && DTR.Shift.PmbreakOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.PmbreakIn.Value.TimeOfDay))
                { 
                    workHours -= Convert.ToDecimal((DTR.Shift.PmbreakIn.Value - DTR.Shift.PmbreakOut.Value).TotalMinutes);
                }
            }
            if (DTR.Shift.LunchIn.HasValue && DTR.Shift.LunchOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.LunchIn.Value.TimeOfDay))
                {
                    workHours -= Convert.ToDecimal((DTR.Shift.LunchIn.Value - DTR.Shift.LunchOut.Value).TotalMinutes);
                }
            }
            if (DTR.Shift.DinnerIn.HasValue && DTR.Shift.DinnerOut.HasValue)
            {
                if (DTR.TimeOut.Value > new DateTime(DTR.TransactionDate.Value.Year, DTR.TransactionDate.Value.Month, DTR.TransactionDate.Value.Day).Add(DTR.Shift.DinnerIn.Value.TimeOfDay))
                {
                    workHours -= Convert.ToDecimal((DTR.Shift.DinnerIn.Value - DTR.Shift.DinnerOut.Value).TotalMinutes);
                }

            }
        }

        protected void MapFieldsToDTR()
        {
            DTR.WorkHours = Math.Round(workHours / 60, 2);
            DTR.ApprovedLate = DTR.ActualLate = Math.Round(late / 60, 2);
            DTR.ApprovedOvertime = DTR.ActualOvertime = Math.Round(totalOvertime / 60, 2);
            DTR.AbsentHours = Math.Round(absentHours / 60, 2);

            DTR.ApprovedUndertime = DTR.ActualUndertime = Math.Round(undertime / 60, 2);
            DTR.ApprovedPreOvertime = DTR.ActualPreOvertime = Math.Round(preShiftOvertime / 60, 2);
            DTR.ApprovedPostOvertime = DTR.ActualPostOvertime = Math.Round(postShiftOvertime / 60, 2);

            DTR.NightDifferential = Math.Round(nightDifferential / 60, 2);
            DTR.NightDifferentialOt = Math.Round(nightDifferentialOvertime / 60, 2);

            DTR.ApprovedLegHol = DTR.ActualLegHol = Math.Round(legalHoliday / 60, 2);
            DTR.ApprovedLegHolOt = DTR.ActualLegHolOt = Math.Round(legalHolidayOvertime / 60, 2);
            DTR.ApprovedNDLegHol = DTR.ActualNDLegHol = Math.Round(legalHolidayNightDifferential / 60, 2);
            DTR.ApprovedNDLegHolOt = DTR.ActualNDLegHolOt = Math.Round(legalHolidayNightDifferentialOvertime / 60, 2);

            DTR.ApprovedSpeHol = DTR.ActualSpeHol = Math.Round(specialHoliday / 60, 2);
            DTR.ApprovedSpeHolOt = DTR.ActualSpeHolOt = Math.Round(specialHolidayOvertime / 60, 2);
            DTR.ApprovedNDSpeHol = DTR.ActualNDSpeHol = Math.Round(specialHolidayNightDifferential / 60, 2);
            DTR.ApprovedNDSpeHolOt = DTR.ActualNDSpeHolOt = Math.Round(specialHolidayNightDifferentialOvertime / 60, 2);

            DTR.ApprovedRestDay = DTR.ActualRestDay = Math.Round(restDay / 60, 2);
            DTR.ApprovedRestDayOt = DTR.ActualRestDayOt = Math.Round(restDayOvertime / 60, 2);
            DTR.ApprovedNDRD = DTR.ActualNDRD = Math.Round(restDayNightDifferential / 60, 2);
            DTR.ApprovedNDRDot = DTR.ActualNDRDot = Math.Round(restDayNightDifferentialOvertime / 60, 2);

            DTR.ApprovedLegSpeHol = DTR.ActualLegSpeHol = Math.Round(legalSpecialHoliday / 60, 2);
            DTR.ApprovedLegSpeHolOt = DTR.ActualLegSpeHolOt = Math.Round(legalSpecialHolidayOvertime / 60, 2);
            DTR.ApprovedNDLegSpeHol = DTR.ActualNDLegSpeHol = Math.Round(legalSpecialHolidayNightDifferential / 60, 2);
            DTR.ApprovedNDLegSpeHolOt = DTR.ActualNDLegSpeHolOt = Math.Round(legalSpecialHolidayNightDifferentialOvertime / 60, 2);

            DTR.ApprovedSpeHolRD = DTR.ActualSpeHolRD = Math.Round(specialHolidayRestDay / 60, 2);
            DTR.ApprovedSpeHolRDot = DTR.ActualSpeHolRDot = Math.Round(specialHolidayRestDayOvertime / 60, 2);
            DTR.ApprovedNDSpeHolRD = DTR.ActualNDSpeHolRD = Math.Round(specialHolidayRestDayNightDifferential / 60, 2);
            DTR.ApprovedNDSpeHolRDot = DTR.ActualNDSpeHolRDot = Math.Round(specialHolidayRestDayNightDifferentialOvertime / 60, 2);

            DTR.ApprovedLegHolRD = DTR.ActualLegHolRD = Math.Round(legalHolidayRestDay / 60, 2);
            DTR.ApprovedLegHolRDot = DTR.ActualLegHolRDot = Math.Round(legalHolidayRestDayOvertime / 60, 2);
            DTR.ApprovedNDLegHolRD = DTR.ActualNDLegHolRD = Math.Round(legalHolidayRestDayNightDifferential / 60, 2);
            DTR.ApprovedNDLegHolRDot = DTR.ActualNDLegHolRDot = Math.Round(legalHolidayRestDayNightDifferentialOvertime / 60, 2);

            DTR.ApprovedLegSpeHolRD = DTR.ActualLegSpeHolRD = Math.Round(legalSpecialHolidayRestDay / 60, 2);
            DTR.ApprovedLegSpeHolRDot = DTR.ActualLegSpeHolRDot = Math.Round(legalSpecialHolidayRestDayOvertime / 60, 2);
            DTR.ApprovedNDLegSpeHolRD = DTR.ActualNDLegSpeHolRD = Math.Round(legalSpecialHolidayRestDayNightDifferential / 60, 2);
            DTR.ApprovedNDLegSpeHolRDot = DTR.ActualNDLegSpeHolRDot = Math.Round(legalSpecialHolidayRestDayNightDifferentialOvertime / 60, 2);
        }
    }
}
