using ATMClasses.Data;
using ATMClasses.Filtering;
using NUnit.Framework;
using NSubstitute;
//Start med TDD

namespace ATM.Unit.Test
{
    [TestFixture]
    public class MonitorTest
    {
        private TrackData _track;
        private Monitor _uut;

        [SetUp]
        public void Setup()
        {
            _track = Substitute.For<TrackData>();
            _uut = new Monitor();
        }

        

        [Test]
        public void Track_In_Range()
        {
            
        }
        
    }
}