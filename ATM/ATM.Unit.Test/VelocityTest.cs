using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class VelocityTest
    {
        private ICalculation _uut;
        private double TO1, TO2, timespan;
        private List<ITracks> tracklist;

        [SetUp]
        public void Setup()
        {
            tracklist = new List<ITracks>();
            _uut = new CalcVelocity();
            TO1 = 30;
            TO2 = 24;
            tracklist.Add(Substitute.For<TrackData>());
            tracklist.Add(Substitute.For<TrackData>());
        }

        private double Action(double x1, double x2, double y1, double y2)
        {
            tracklist[0].X = Convert.ToInt32(x1);
            tracklist[1].X = Convert.ToInt32(x2);
            tracklist[0].Y = Convert.ToInt32(y1);
            tracklist[1].Y = Convert.ToInt32(y2);
            return timespan = TO2 - TO1;
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(-1, -2, -1, -2, 0)]
        //[TestCase(-1, -2, 20, 10, 2)]
        [TestCase(-1.500, -2.50, 20.1000, 10.223, 2)]
        [TestCase(10000, 90000, -10000, -90000, 18856)]
        [TestCase(10000, 90000, 10000, 90000, 18856)]
        [TestCase(90000, 10000, 90000, 10000, 18856)]
        public void Calculations_Of_Velocity_Is_Right_With_Track(double x1, double x2, double y1, double y2, double r)
        {
            timespan = Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0].X, tracklist[1].X, tracklist[0].Y, tracklist[1].Y, timespan));
            Assert.AreEqual(result, r);
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(-1, -2, -1, -2, 0)]
        [TestCase(-1.500, -2.50, 20.1000, 10.223, 2)]
        [TestCase(10000, 90000, -10000, -90000, 18856)]
        [TestCase(10000, 90000, 10000, 90000, 18856)]
        public void Calculations_Of_Velocity_Is_Right(double x1, double x2, double y1, double y2, double r)
        {
            timespan = Action(x1,x2,y1,y2);
            double result = Convert.ToInt32(_uut.Velocity(x1, x2, y1, y2, timespan));
            Assert.AreEqual(result, r);
        }
        [TestCase(0, 0, 0, 0, 1)]
        [TestCase(-1, -2, -1, -2, 2)]
        //[TestCase(-1, -2, 20, 10, 3)]
        [TestCase(10000, 90000, -10000, -90000, 100)]
        [TestCase(10000, 90000, 10000, 90000, 200)]
        public void Calculations_Of_Velocity_Is_Wrong(double x1, double x2, double y1, double y2, double r)
        {
            timespan = Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(x1, x2, y1, y2, timespan));
            Assert.AreNotEqual(result, r);
        }
        [TestCase(0, 0, 0, 0, "0")]
        [TestCase(-1, -2, -1, -2, "0")]
        //[TestCase(-1, -2, 20, 10, "2")]
        [TestCase(10000, 90000, -10000, -90000, "18856")]
        [TestCase(10000, 90000, 10000, 90000, "18856")]
        public void Calculations_Of_Velocity_Output_One_ToString(double x1, double x2, double y1, double y2, string r)
        {
            timespan = Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(x1, x2, y1, y2, timespan));
            Assert.AreEqual(result.ToString(), r );
        }
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(-1, -2, -1, -2, 0)]
        //[TestCase(-1, -2, 20, 10, 1)]
        [TestCase(10000, 90000, -10000, -90000, 1)]
        [TestCase(10000, 90000, 10000, 90000, 1)]
        public void Calculations_Of_Velocity_Output_Is_Greater(double x1, double x2, double y1, double y2, double r)
        {
            timespan = Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(x1, x2, y1, y2, timespan));
            Assert.AreEqual(result.CompareTo(r), r);
        }
        [TestCase(0, 0, 0, 0, "10")]
        [TestCase(-1, -2, -1, -2, "2")]
        //[TestCase(-1, -2, 20, 10, "0")]
        [TestCase(10000, 90000, -10000, -90000, "8856")]
        [TestCase(10000, 90000, 10000, 90000, "8856")]
        public void Calculations_Of_Velocity_Output_One_WrongString(double x1, double x2, double y1, double y2, string r)
        {
            timespan = Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(x1, x2, y1, y2, timespan));
            Assert.AreNotEqual(result.ToString(), r);
        }
    }
}