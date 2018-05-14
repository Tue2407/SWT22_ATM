using ATMClasses.Interfaces;
using ATMClasses.Render;
using ATMClasses.TrackUpdate;
using NUnit.Framework;
using NSubstitute;
namespace ATM.Unit.Test
{
    [TestFixture]
    public class CourseTest
    {
        private CalcCourse _uut;
        private double _x1, _x2, _y1, _y2;
        private ITracks _track1;
        private ITracks _track2;

        [SetUp]
        public void Setup()
        {
            _track1 = Substitute.For<ITracks>();
            _track2 = Substitute.For<ITracks>();
            _uut = new CalcCourse();
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
            _track1.X = 20000;
            _track2.X = 20000;
            _track1.Y = 10000;
            _track2.Y = 10000;

            _uut.Calculate(_track1, _track2);
            Assert.AreEqual(_uut._Angle, 90);

        }

        [Test]
        public void HeadingWest()
        {
            _track1.X = 200000;
            _track2.X = 100000;
            _track1.Y = 200000;
            _track2.Y = 200000;
            _uut.Calculate(_track1, _track2);
            Assert.AreEqual(_uut._Angle, 270);
        }
        [Test]
        public void HeadingSouth()
        {
            _track1.X = 2;
            _track2.X = 2;
            _track1.Y = 2;
            _track2.Y = 0;
            _uut.Calculate(_track1, _track2);
            Assert.AreEqual(_uut._Angle, 180);
        }
        [Test]
        public void HeadingNorth()
        {
            _track1.X = 2;
            _track2.X = 2;
            _track1.Y = 0;
            _track2.Y = 2;
            _uut.Calculate(_track1, _track2);
            Assert.AreEqual(_uut._Angle, 360);
        }
    }
}