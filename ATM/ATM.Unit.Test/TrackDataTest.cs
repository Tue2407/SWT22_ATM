using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class TrackDataTest
    {
        private ITracks _uut;
        private string _tag;
        private int _xcoord;
        private int _ycoord;
        private int _altitude;
        private int _course { get; set; }
        private int _velocity { get; set; }
       

        [SetUp]
        public void Setup()
        {
            //Initialisere
            _tag = "TRK001";
            _xcoord = 12345;
            _ycoord = 67890;
            _altitude = 12000;
            _course = 0;
            _velocity = 0;
            //Test
            _uut = new TrackData();
        }

        private void Action()
        {
            _uut.Tag = _tag;
            _uut.X = _xcoord;
            _uut.Y = _ycoord;
            _uut.Altitude = _altitude;
            _uut.Velocity = _velocity;
            _uut.Course = _course;
        }

        [Test]
        public void Track_Tag()
        {
            Action();
            Assert.That(_uut.Tag,Is.EqualTo("TRK001"));
        }
        [Test]
        public void Track_XCoord()
        {
            Action();
            Assert.That(_uut.X, Is.EqualTo(12345));
        }
        [Test]
        public void Track_YCoord()
        {
            Action();
            Assert.That(_uut.Y, Is.EqualTo(67890));
        }
        [Test]
        public void Track_Altitude()
        {
            Action();
            Assert.That(_uut.Altitude, Is.EqualTo(12000));
        }
        [Test]
        public void Track_Velocity()
        {
            Action();
            Assert.That(_uut.Velocity, Is.EqualTo(0));
        }
        [Test]
        public void Track_Course()
        {
            Action();
            Assert.That(_uut.Course, Is.EqualTo(0));
        }

    }
}