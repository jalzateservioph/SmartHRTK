using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TKProcessor.Models.TK;
using TKProcessor.Services;
namespace TKProcessor.Test
{
    [TestClass]
    public class StandardProcessorTest
    {
        DailyTransactionRecord DTR;
        StandardDTRProcessor standard;


        #region Regular
        [TestMethod]
        public void StandardRegularWorkHoursTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); 
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);

            //shift.AmbreakIn = new DateTime(2019, 10, 14, 9, 0, 0);
            //shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            //shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            //shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            //shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            //shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);

            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal workHours = DTR.WorkHours;
            Assert.AreEqual(9, workHours);
        }

        #region Late
        [TestMethod]
        public void StandardRegularLateTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 15, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsLateIn = true;

            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal late = DTR.ActualLate;
            Assert.AreEqual(0.25M, late);
        }


        [TestMethod]
        public void StandardRegularLateWithGracePeriodTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 14, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsLateIn = true;
            shift.GracePeriodLateIn = 15;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal late = DTR.ActualLate;
            Assert.AreEqual(0, late);
        }
        #endregion

        #region Undertime
        [TestMethod]
        public void StandardRegularUndertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 15, 45, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsEarlyOut = true;

            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal undertime = DTR.ActualUndertime;
            Assert.AreEqual(0.25M, undertime);
        }


        [TestMethod]
        public void StandardRegularUndertimeWithGracePeriodTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 15, 45, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsEarlyOut = true;
            shift.GracePeriodEarlyOut = 15;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal undertime = DTR.ActualUndertime;
            Assert.AreEqual(0, undertime);
        }
        #endregion


        #region Overtime

        #region Pre-shift Overtime
        [TestMethod]
        public void StandardRegularPreShiftOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPreShiftOt = true;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal preShiftOvertime = DTR.ActualPreOvertime;
            Assert.AreEqual(1M, preShiftOvertime);
        }

        [TestMethod]
        public void StandardRegularPreShiftOvertimeWithMinimumTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 30, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPreShiftOt = true;
            shift.MinimumPreShiftOt = 1;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal preShiftOvertime = DTR.ActualPreOvertime;
            Assert.AreEqual(0M, preShiftOvertime);
        }

        [TestMethod]
        public void StandardRegularPreShiftOvertimeWithMaximumTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 4, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPreShiftOt = true;
            shift.MaximumPreShiftOt = 2;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal preShiftOvertime = DTR.ActualPreOvertime;
            Assert.AreEqual(2M, preShiftOvertime);
        }
        #endregion

        #region Post-shift Overtime
        [TestMethod]
        public void StandardRegularPostShiftOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 18, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal postShiftOvertime = DTR.ActualPostOvertime;
            Assert.AreEqual(2M, postShiftOvertime);
        }

        [TestMethod]
        public void StandardRegularPostShiftOvertimeWithMinimumTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 45, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.MinimumPostShiftOt = 1;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal postShiftOvertime = DTR.ActualPostOvertime;
            Assert.AreEqual(0M, postShiftOvertime);
        }

        [TestMethod]
        public void StandardRegularPostShiftOvertimeWithMaximumTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 19, 30, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.MaximumPostShiftOt = 2;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal postShiftOvertime = DTR.ActualPostOvertime;
            Assert.AreEqual(2M, postShiftOvertime);
        }
        #endregion

        #endregion

        #region Night Diffferential
        [TestMethod]
        public void StandardRegularNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 13, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 22, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 22, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 19, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 23, 0, 0);

            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal nightDifferential = DTR.NightDifferential;
            Assert.AreEqual(3M, nightDifferential);
        }

        #region Night Differential Overtime

        [TestMethod]
        public void StandardRegularNightDifferentialPreShiftOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 10, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 9, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.IsPreShiftOt = true;


            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal nightDifferentialOvertime = DTR.NightDifferentialOt;
            Assert.AreEqual(1M, nightDifferentialOvertime);
        }

        [TestMethod]
        public void StandardRegularNightDifferentialPostShiftOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 19, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 10, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 20, 0, 0);
            shift.IsPostShiftOt = true;


            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeRegular();

            decimal nightDifferentialOvertime = DTR.NightDifferentialOt;
            Assert.AreEqual(1M, nightDifferentialOvertime);
        }
        #endregion
        #endregion
        #endregion


        #region Holiday/Rest Day

        #region Legal Holiday
        [TestMethod]
        public void StandardLegalHolidayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday {
                Type =  (int) HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
                });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHoliday = standard.DTR.ActualLegHol;
            Assert.AreEqual(9M, legalHoliday);
        }

        [TestMethod]
        public void StandardLegalHolidayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayOvertime = standard.DTR.ActualLegHolOt;
            Assert.AreEqual(2M, legalHolidayOvertime);
        }

        [TestMethod]
        public void StandardLegalHolidayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayNightDifferential = standard.DTR.ActualNDLegHol;
            Assert.AreEqual(2M, legalHolidayNightDifferential);
        }

        [TestMethod]
        public void StandardLegalHolidayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsPostShiftOt = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayNightDifferentialOvertime = standard.DTR.ActualNDLegHolOt;
            Assert.AreEqual(1M, legalHolidayNightDifferentialOvertime);
        }
        #endregion

        #region Special Holiday
        [TestMethod]
        public void StandardSpecialHolidayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHoliday = standard.DTR.ActualSpeHol;
            Assert.AreEqual(9M, specialHoliday);
        }
        [TestMethod]
        public void StandardSpecialHolidayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayOvertime = standard.DTR.ActualSpeHolOt;
            Assert.AreEqual(2M, specialHolidayOvertime);
        }
        [TestMethod]
        public void StandardSpecialHolidayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayNightDifferential = standard.DTR.ActualNDSpeHol;
            Assert.AreEqual(2M, specialHolidayNightDifferential);
        }

        [TestMethod]
        public void StandardSpecialHolidayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsPostShiftOt = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayNightDifferentialOvertime = standard.DTR.ActualNDSpeHolOt;
            Assert.AreEqual(1M, specialHolidayNightDifferentialOvertime);
        }
        #endregion

        #region Rest Day
        [TestMethod]
        public void StandardRestDayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeHolidayAndRestDay();

            decimal restDay = standard.DTR.ActualRestDay;
            Assert.AreEqual(9M, restDay);
        }

        [TestMethod]
        public void StandardRestDayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;
            shift.IsRestDay = true;

            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeHolidayAndRestDay();

            decimal restDayOvertime = standard.DTR.ActualRestDayOt;
            Assert.AreEqual(2M, restDayOvertime);
        }

        [TestMethod]
        public void StandardRestDayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            standard = new StandardDTRProcessor(DTR);

            standard.ComputeHolidayAndRestDay();

            decimal restDayNightDifferential = standard.DTR.ActualNDRD;
            Assert.AreEqual(2M, restDayNightDifferential);
        }

        [TestMethod]
        public void StandardRestDayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsRestDay = true;
            shift.IsPostShiftOt = true;
            DTR.Shift = shift;
            standard = new StandardDTRProcessor(DTR);

            standard.ComputeHolidayAndRestDay();

            decimal restDayNightDifferentialOvertime = standard.DTR.ActualNDRDot;
            Assert.AreEqual(1M, restDayNightDifferentialOvertime);
        }
        #endregion

        #region Legal and Special Holiday
        [TestMethod]
        public void StandardLegalSpecialHolidayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHoliday = standard.DTR.ActualLegSpeHol;
            Assert.AreEqual(9M, legalSpecialHoliday);
        }
        [TestMethod]
        public void StandardLegalSpecialHolidayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayOvertime = standard.DTR.ActualLegSpeHolOt;
            Assert.AreEqual(2M, legalSpecialHolidayOvertime);
        }

        [TestMethod]
        public void StandardLegalSpecialHolidayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayNightDifferential = standard.DTR.ActualNDLegSpeHol;
            Assert.AreEqual(2M, legalSpecialHolidayNightDifferential);
        }

        [TestMethod]
        public void StandardLegalSpecialHolidayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsPostShiftOt = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });
            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayNightDifferentialOvertime = standard.DTR.ActualNDLegSpeHolOt;
            Assert.AreEqual(1M, legalSpecialHolidayNightDifferentialOvertime);
        }
        #endregion

        #region Legal Holiday and Rest Day
        [TestMethod]
        public void StandardLegalHolidayRestDayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayRestDay = standard.DTR.ActualLegHolRD;
            Assert.AreEqual(9M, legalHolidayRestDay);
        }

        [TestMethod]
        public void StandardLegalHolidayRestDayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayRestDayOvertime = standard.DTR.ActualLegHolRDot;
            Assert.AreEqual(2M, legalHolidayRestDayOvertime);
        }

        [TestMethod]
        public void StandardLegalHolidayRestDayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayRestDayNightDifferential = standard.DTR.ActualNDLegHolRD;
            Assert.AreEqual(2M, legalHolidayRestDayNightDifferential);
        }

        [TestMethod]
        public void StandardLegalHolidayRestDayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalHolidayNightDifferentialOvertime = standard.DTR.ActualLegHolRDot;
            Assert.AreEqual(1M, legalHolidayNightDifferentialOvertime);
        }
        #endregion

        #region Special Holiday and Rest Day
        [TestMethod]
        public void StandardSpecialHolidayRestDayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayRestDay = standard.DTR.ActualSpeHolRD;
            Assert.AreEqual(9M, specialHolidayRestDay);
        }
        [TestMethod]
        public void StandardSpecialHolidayRestDayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;
            shift.IsRestDay = true;

            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayRestDayOvertime = standard.DTR.ActualSpeHolRDot;
            Assert.AreEqual(2M, specialHolidayRestDayOvertime);
        }
        [TestMethod]
        public void StandardSpecialHolidayRestDayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayRestDayNightDifferential = standard.DTR.ActualNDSpeHolRD;
            Assert.AreEqual(2M, specialHolidayRestDayNightDifferential);
        }

        [TestMethod]
        public void StandardSpecialHolidayRestDayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal specialHolidayRestDayNightDifferentialOvertime = standard.DTR.ActualNDSpeHolRDot;
            Assert.AreEqual(1M, specialHolidayRestDayNightDifferentialOvertime);
        }
        #endregion

        #region Legal, Special Holiday and Rest Day
        [TestMethod]
        public void StandardLegalSpecialRestDayHolidayTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayRestDay = standard.DTR.ActualLegSpeHolRD;
            Assert.AreEqual(9M, legalSpecialHolidayRestDay);
        }
        [TestMethod]
        public void StandardLegalSpecialHolidayRestDayOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsPreShiftOt = true;
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayRestDayOvertime = standard.DTR.ActualLegSpeHolRDot;
            Assert.AreEqual(2M, legalSpecialHolidayRestDayOvertime);
        }

        [TestMethod]
        public void StandardLegalSpecialHolidayRestDayNightDifferentialTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });

            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayRestDayNightDifferential = standard.DTR.ActualNDLegSpeHolRD;
            Assert.AreEqual(2M, legalSpecialHolidayRestDayNightDifferential);
        }

        [TestMethod]
        public void StandardLegalSpecialHolidayRestDayNightDifferentialOvertimeTest()
        {
            DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 17, 0, 0);  //October 14 2019 4:00pm


            Shift shift = new Shift();

            shift.ScheduleIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.ScheduleOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.NightDiffStart = new DateTime(2019, 10, 14, 15, 0, 0);
            shift.NightDiffEnd = new DateTime(2019, 10, 14, 18, 0, 0);
            shift.IsPostShiftOt = true;
            shift.IsRestDay = true;
            DTR.Shift = shift;

            List<Holiday> holidays = new List<Holiday>();
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Legal,
                Date = new DateTime(2019, 10, 14)
            });
            holidays.Add(new Holiday
            {
                Type = (int)HolidayType.Special,
                Date = new DateTime(2019, 10, 14)
            });
            standard = new StandardDTRProcessor(DTR, holidays);

            standard.ComputeHolidayAndRestDay();

            decimal legalSpecialHolidayRestDayNightDifferentialOvertime = standard.DTR.ActualNDLegSpeHolRDot;
            Assert.AreEqual(1M, legalSpecialHolidayRestDayNightDifferentialOvertime);
        }
        #endregion
        #endregion
    }
}
