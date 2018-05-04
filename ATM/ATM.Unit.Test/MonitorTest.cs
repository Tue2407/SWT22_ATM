using System;
using ATMClasses.Data;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;
//Start med TDD

namespace ATM.Unit.Test
{
    [TestFixture]
    public class MonitorTest
    {
        private ITracks _track;
        private Monitor _uut;
        
        [SetUp]
        public void Setup()
        {
            _track = new TrackData();

            _uut = new Monitor(_track);
        }

        public void Action(int x, int y, int z)
        {
            _track.X = x;
            _track.Y = y;
            _track.Altitude = z;
            
            _uut.InView = _uut.MonitorFlight(_track);
        }

        [TestCase(10000, 10000, 500, true)]
        [TestCase(90000, 90000, 500, true)]
        [TestCase(10000, 10000, 20000, true)]
        [TestCase(90000, 90000, 20000, true)]
        public void Track_Is_In_Range(int x, int y, int alt, bool view)
        {
            Action(x, y , alt);
            Assert.AreEqual(_uut.InView, view);
        }

        [TestCase(10000, 10000, 499, false)]
        [TestCase(90000, 90000, 499, false)]
        [TestCase(10000, 10000, 20001, false)]
        [TestCase(90000, 90000, 20001, false)]
        [TestCase(10000, 9999, 500, false)]
        [TestCase(90000, 90001, 500, false)]
        [TestCase(9999, 10000, 2000, false)]
        [TestCase(90001, 90000, 2000, false)]
        public void Track_Is_Out_Of_Range(int x, int y, int alt, bool view)
        {
            Action(x, y, alt);
            Assert.AreEqual(_uut.InView, view);
        }


    }
}