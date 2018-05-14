using ATMClasses.Interfaces;
using ATMClasses.Proximity_Detection;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class _SeparationTest
    {
        private SeparationEvent _uut;
        private ICalcDistance _calcDistance;
        private ITracks _track1;
        private ITracks _track2;

        [SetUp]
        public void Setup()
        {
            _uut = new SeparationEvent();
            _calcDistance = Substitute.For<ICalcDistance>();
            _track1 = Substitute.For<ITracks>();
            _track2 = Substitute.For<ITracks>();

        }

        public void Action(int x1, int x2, int alt1, int alt2)
        {
            _track1.X = x1;
            _track1.Altitude = alt1;
            _track2.X = x2;
            _track2.Altitude = alt2;

            _calcDistance.CalculateDistance1D(x1, x2).Returns(300);
            _calcDistance.CalculateDistance2D(x1, x2, alt1, alt2).Returns(400);
        }

        [TestCase(100,100,100,100, false)]
        [TestCase(5000, 5000, 1000, 1300, true)]
        public void SeparationEvent_Bool_Checker(int x1, int x2, int alt1, int alt2, bool r)
        {
            Action(x1, x2, alt1,alt2);
           
            Assert.That(_uut.CollisionDetection(_calcDistance, _track1, _track2),Is.EqualTo(r));
             
        }
    }
}