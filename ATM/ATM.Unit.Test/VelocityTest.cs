using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using ATMClasses.Render;
using ATMClasses.TrackUpdate;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class VelocityTest
    {
        private ICalcVelocity _uut;
        private List<ITracks> tracklist;

        [SetUp]
        public void Setup()
        {
            tracklist = new List<ITracks>();
            _uut = new CalcVelocity();
            tracklist.Add(Substitute.For<TrackData>());
            tracklist.Add(Substitute.For<TrackData>());
        }

        private void Action(double x1, double x2, double y1, double y2)
        {
            tracklist[0].X = Convert.ToInt32(x1);
            tracklist[1].X = Convert.ToInt32(x2);
            tracklist[0].Y = Convert.ToInt32(y1);
            tracklist[1].Y = Convert.ToInt32(y2);
            tracklist[0].Timestamp = new DateTime(20180419152929100);
            tracklist[1].Timestamp = new DateTime(20180419152929106);
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(-1, -2, -1, -2, 2357023.0d)]
        [TestCase(-1.500, -2.50, 20.1000, 10.223, 16666667)]
        [TestCase(10, 90, -10, -900, 1489313787)]
        [TestCase(10, 90, 10, 900, 1489313787)]
        [TestCase(90, 10, 90, 100, 134370962)]
        public void Calculations_Of_Velocity_Is_Right_With_Track(double x1, double x2, double y1, double y2, double r)
        {
            Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0],tracklist[1]));
            Assert.AreEqual(result, r);
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(-1, -2, -1, -2, 2357023.0d)]
        [TestCase(-1.500, -2.50, 20.1000, 10.223, 16666667)]
        [TestCase(10, 90, -10, -900, 1489313787)]
        [TestCase(10, 90, 10, 900, 1489313787)]
        [TestCase(90, 10, 90, 100, 134370962)]
        public void Calculations_Of_Velocity_Is_Right(double x1, double x2, double y1, double y2, double r)
        {
            Action(x1,x2,y1,y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0], tracklist[1]));
            Assert.AreEqual(result, r);
        }
        [TestCase(0, 0, 0, 0, 1)]
        [TestCase(-1, -2, -1, -2, 2)]
        //[TestCase(-1, -2, 20, 10, 3)]
        [TestCase(10, 90, -10, -90, 10)]
        [TestCase(10, 90, 10, 90, 20)]
        public void Calculations_Of_Velocity_Is_Wrong(double x1, double x2, double y1, double y2, double r)
        {
            Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0], tracklist[1]));
            Assert.AreNotEqual(result, r);
        }
        [TestCase(0, 0, 0, 0, "0")]
        [TestCase(-1, -2, -1, -2, "2357023")]
        [TestCase(-1.500, -2.50, 20.1000, 10.223, "16666667")]
        [TestCase(10, 90, -10, -900, "1489313787")]
        [TestCase(10, 90, 10, 900, "1489313787")]
        [TestCase(90, 10, 90, 100, "134370962")]
        public void Calculations_Of_Velocity_Output_One_ToString(double x1, double x2, double y1, double y2, string r)
        {
            Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0], tracklist[1]));
            Assert.AreEqual(result.ToString(), r );
        }
        
        [TestCase(-1, -2, -1, -2, 1)]
        [TestCase(-1.500, -2.50, 20.1000, 10.223, 1)]
        [TestCase(10, 90, 10, 900, 1)]
        [TestCase(90, 10, 90, 100, 1)]
        public void Calculations_Of_Velocity_Output_Is_Greater(double x1, double x2, double y1, double y2, double r)
        {
            Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0], tracklist[1]));
            Assert.AreEqual(result.CompareTo(r), r);
        }
        [TestCase(0, 0, 0, 0, "10")]
        [TestCase(-1, -2, -1, -2, "2")]
        //[TestCase(-1, -2, 20, 10, "0")]
        [TestCase(10, 90, -10, -90, "8856")]
        [TestCase(10, 90, 10, 90, "8856")]
        public void Calculations_Of_Velocity_Output_One_WrongString(double x1, double x2, double y1, double y2, string r)
        {
            Action(x1, x2, y1, y2);
            double result = Convert.ToInt32(_uut.Velocity(tracklist[0], tracklist[1]));
            Assert.AreNotEqual(result.ToString(), r);
        }
    }
}