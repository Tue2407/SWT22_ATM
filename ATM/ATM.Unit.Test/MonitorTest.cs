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
            _track = Substitute.For<ITracks>();
            _uut = new Monitor(_track);
        }

        public void Action()
        {
            _track.X = 10000;
            _track.Y = 90000;
        }
        

        [Test]
        public void Track_Is_In_Range()
        {
            Action();
            Assert.That(_uut,Is.EqualTo(true));
        }
        
    }
}