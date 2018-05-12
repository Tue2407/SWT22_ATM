using ATMClasses.TrackUpdate;
using NUnit.Framework;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class CourseTest
    {
        private CalcCourse _uut;
        private double _x1, _x2, _y1, _y2;

        [SetUp]
        public void Setup()
        {

            _uut = new CalcCourse(_x1, _x2, _y1, _y2);
        }

        [Test]
        public void Set_DX()
        {
            _uut._dx = _x1;
            Assert.AreEqual(_uut._dx, _x1);
        }

        [Test]
        public void Get_DX()
        {
            _uut._dx = _x1;
            Assert.AreEqual(_uut._dx, _x1);
        }
        [Test]
        public void Set_DY()
        {
            _uut._dy = _y1;
            Assert.AreEqual(_uut._dy, _y1);
        }

        [Test]
        public void Get_DY()
        {
            _uut._dy = _y1;
            Assert.AreEqual(_uut._dy, _y1);

        }

        [Test]
        public void HeadingEast()
        {
            _x1 = 2;
            _x2 = 2;
            _y1 = 0;
            _y2 = 0;
            _uut = new CalcCourse(_x1, _x2, _y1, _y2);
            Assert.AreEqual(_uut._Angle, 90);

        }

        [Test]
        public void HeadingWest()
        {
            _x1 = 2;
            _x2 = 1;
            _y1 = 2;
            _y2 = 2;
            _uut = new CalcCourse(_x1, _x2, _y1, _y2);
            Assert.AreEqual(_uut._Angle, 270);
        }
        [Test]
        public void HeadingSouth()
        {
            _x1 = 2;
            _x2 = 2;
            _y1 = 2;
            _y2 = 0;
            _uut = new CalcCourse(_x1, _x2, _y1, _y2);
            Assert.AreEqual(_uut._Angle, 180);
        }
        [Test]
        public void HeadingNorth()
        {
            _x1 = 2;
            _x2 = 2;
            _y1 = 0;
            _y2 = 2;
            _uut = new CalcCourse(_x1, _x2, _y1, _y2);
            Assert.AreEqual(_uut._Angle, 360);
        }
    }
}