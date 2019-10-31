using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TKProcessor.Services;
using TKProcessor.Models.TK;
namespace TKProcessor.Test
{
    [TestClass]
    public class FlextimeProcessorTest
    {
        [TestMethod]
        public void FullFlexComputeRegTest()
        {
            /*
             * Arrange: Shift (FlextimeType, Breaks, IsEarlyOut, IsLateIn, RequiredWorkHours)
             * DTR (TimeIn, TimeOut, BreakIn, BreakOut, Shift)
             */
            DailyTransactionRecord DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 15, 45, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();
            shift.FlextimeType = 0;
            shift.AmbreakIn = new DateTime(2019,10,14,9,0,0);
            shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);
            shift.IsEarlyOut = true;
            shift.IsLateIn = true;
            shift.RequiredWorkHours = (decimal) 7.5;
            //shift.GracePeriodEarlyOut = 15;
            DTR.Shift = shift;

            FlextimeDTRProcessor flex = new FlextimeDTRProcessor(DTR);

            /*
             * Act
             */
            //flex.ComputeReg();

            //Assert
            decimal undertime = flex.DTR.ActualUndertime;
            Assert.AreEqual((decimal) 0.25, undertime);
        }

        [TestMethod]
        public void SemiOnTheDotComputeRegTest()
        {
            /*
             * Arrange: Shift (FlextimeType, Breaks, IsEarlyOut, IsLateIn, RequiredWorkHours, EarliestIn, LatestIn, EarliestOut, LatestOut)
             * DTR (TimeIn, TimeOut, BreakIn, BreakOut, Shift)
             */
            DailyTransactionRecord DTR = new DailyTransactionRecord();

            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 30, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();
            shift.FlextimeType = 1;

            shift.AmbreakIn = new DateTime(2019, 10, 14, 9, 0, 0);
            shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);

            shift.IsEarlyOut = true;
            shift.EarliestTimeIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.LatestTimeIn = new DateTime(2019, 10, 14, 10, 0, 0);
            shift.EarliestTimeOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.LatestTimeOut = new DateTime(2019, 10, 14, 19, 0, 0);

            shift.IsLateIn = true;
            shift.RequiredWorkHours = (decimal)7.5;
            shift.GracePeriodLateIn = 15;
            shift.GracePeriodEarlyOut = 15;
            DTR.Shift = shift;

            FlextimeDTRProcessor flex = new FlextimeDTRProcessor(DTR);

            /*
             * Act
             */
            //flex.ComputeReg();


            //Assert
            decimal workHours = flex.DTR.WorkHours;
            decimal undertime = flex.DTR.ActualUndertime;
            decimal late = flex.DTR.ActualLate;
            //Assert.AreEqual(9M, workHours, "Workhours not equal");
            Assert.AreEqual(0M, undertime, "Undertime not equal");
            //Assert.AreEqual(1M, late, "Late not equal");
        }

        [TestMethod]
        public void SemiFixedIncrementsComputeRegTest()
        {
            /*
             * Arrange: Shift (FlextimeType, Breaks, IsEarlyOut, IsLateIn, RequiredWorkHours, EarliestIn, LatestIn, Increment)
             * DTR (TimeIn, TimeOut, BreakIn, BreakOut, Shift)
             */
            DailyTransactionRecord DTR = new DailyTransactionRecord();

            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 16, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 16, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();
            shift.FlextimeType = 2;

            shift.AmbreakIn = new DateTime(2019, 10, 14, 9, 0, 0);
            shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);

            shift.IsEarlyOut = true;
            shift.EarliestTimeIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.LatestTimeIn = new DateTime(2019, 10, 14, 10, 0, 0);

            shift.Increment = 1;

            shift.IsLateIn = true;
            shift.RequiredWorkHours = (decimal)7.5;
            shift.GracePeriodLateIn = 15;
            shift.GracePeriodEarlyOut = 15;
            DTR.Shift = shift;

            FlextimeDTRProcessor flex = new FlextimeDTRProcessor(DTR);

            /*
             * Act
             */
            //flex.ComputeReg();

            //Assert
            decimal workHours = flex.DTR.WorkHours;
            decimal undertime = flex.DTR.ActualUndertime;
            decimal late = flex.DTR.ActualLate;
            //Assert.AreEqual(9M, workHours, "Workhours not equal");
            Assert.AreEqual(0M, undertime, "Undertime not equal");
            //Assert.AreEqual(1M, late, "Late not equal");
        }

        [TestMethod]
        public void FullFlexOvertimeTest()
        {
            /*
             * Arrange: Shift (FlextimeType, Breaks, IsEarlyOut, IsLateIn, RequiredWorkHours)
             * DTR (TimeIn, TimeOut, BreakIn, BreakOut, Shift)
             */
            DailyTransactionRecord DTR = new DailyTransactionRecord();
            DTR.TimeIn = new DateTime(2019, 10, 14, 7, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 21, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();
            shift.FlextimeType = 0;
            shift.AmbreakIn = new DateTime(2019, 10, 14, 9, 0, 0);
            shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);
            shift.IsEarlyOut = true;
            shift.IsLateIn = true;
            shift.RequiredWorkHours = (decimal)7.5;
            //shift.GracePeriodEarlyOut = 15;
            shift.IsPostShiftOt = true;
            shift.MinimumPostShiftOt = 1;
            shift.MaximumPostShiftOt = 4;
            DTR.Shift = shift;

            FlextimeDTRProcessor flex = new FlextimeDTRProcessor(DTR);


            //flex.ComputeReg();
            decimal postOvertime = flex.DTR.ActualPostOvertime;
            Assert.AreEqual(4M, postOvertime);


        }

        [TestMethod]
        public void SemiOnTheDotOvertimeTest()
        {
            DailyTransactionRecord DTR = new DailyTransactionRecord();

            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 19, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();
            shift.FlextimeType = 1;

            shift.AmbreakIn = new DateTime(2019, 10, 14, 9, 0, 0);
            shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);

            shift.IsEarlyOut = true;
            shift.EarliestTimeIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.LatestTimeIn = new DateTime(2019, 10, 14, 10, 0, 0);
            shift.EarliestTimeOut = new DateTime(2019, 10, 14, 16, 0, 0);
            shift.LatestTimeOut = new DateTime(2019, 10, 14, 19, 0, 0);

            shift.IsLateIn = true;
            shift.RequiredWorkHours = (decimal)7.5;
            shift.GracePeriodLateIn = 15;
            shift.GracePeriodEarlyOut = 15;

            shift.IsPreShiftOt = true;
            shift.MinimumPreShiftOt = 1;
            shift.MaximumPreShiftOt = 3;

            shift.IsPostShiftOt = true;
            shift.MinimumPostShiftOt = 1;
            shift.MaximumPostShiftOt = 4;

            DTR.Shift = shift;

            FlextimeDTRProcessor flex = new FlextimeDTRProcessor(DTR);

            /*
             * Act
             */


            //flex.ComputeReg();
            decimal postOvertime = flex.DTR.ActualPostOvertime;
            decimal preOvertime = flex.DTR.ActualPreOvertime;
            decimal totalOvertime = flex.DTR.ActualOvertime;
            Assert.AreEqual(1M, preOvertime);
            Assert.AreEqual(3M, postOvertime);
            Assert.AreEqual(4M, totalOvertime);
        }

        [TestMethod]
        public void SemiFixedIncrementsOvertimeTest()
        {
            /*
            * Arrange: Shift (FlextimeType, Breaks, IsEarlyOut, IsLateIn, RequiredWorkHours, EarliestIn, LatestIn, Increment)
            * DTR (TimeIn, TimeOut, BreakIn, BreakOut, Shift)
            */
            DailyTransactionRecord DTR = new DailyTransactionRecord();

            DTR.TimeIn = new DateTime(2019, 10, 14, 6, 0, 0); //October 14 2019 7:00am
            DTR.TimeOut = new DateTime(2019, 10, 14, 19, 0, 0);  //October 14 2019 4:00pm

            Shift shift = new Shift();
            shift.FlextimeType = 2;

            shift.AmbreakIn = new DateTime(2019, 10, 14, 9, 0, 0);
            shift.AmbreakOut = new DateTime(2019, 10, 14, 9, 15, 0);
            shift.LunchIn = new DateTime(2019, 10, 14, 12, 0, 0);
            shift.LunchOut = new DateTime(2019, 10, 14, 13, 0, 0);
            shift.PmbreakIn = new DateTime(2019, 10, 14, 14, 0, 0);
            shift.PmbreakOut = new DateTime(2019, 10, 14, 14, 15, 0);

            shift.IsEarlyOut = true;
            shift.EarliestTimeIn = new DateTime(2019, 10, 14, 7, 0, 0);
            shift.LatestTimeIn = new DateTime(2019, 10, 14, 10, 0, 0);

            shift.Increment = 1;

            shift.IsLateIn = true;
            shift.RequiredWorkHours = (decimal)7.5;
            shift.GracePeriodLateIn = 15;
            shift.GracePeriodEarlyOut = 15;
            DTR.Shift = shift;


            shift.IsPreShiftOt = true;
            shift.MinimumPreShiftOt = 1;
            shift.MaximumPreShiftOt = 3;

            shift.IsPostShiftOt = true;
            shift.MinimumPostShiftOt = 1;
            shift.MaximumPostShiftOt = 4;


            FlextimeDTRProcessor flex = new FlextimeDTRProcessor(DTR);

            /*
             * Act
             */
            //flex.ComputeReg();

            decimal postOvertime = flex.DTR.ActualPostOvertime;
            decimal preOvertime = flex.DTR.ActualPreOvertime;
            decimal totalOvertime = flex.DTR.ActualOvertime;
            Assert.AreEqual(1M, preOvertime);
            Assert.AreEqual(3M, postOvertime);
            Assert.AreEqual(4M, totalOvertime);
        }
    }
}
