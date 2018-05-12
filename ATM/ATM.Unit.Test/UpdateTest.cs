using System.Collections.Generic;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class UpdateTest
    {
        private ITrackDecoding _decoder;
        private TrackDataEventArgs _fakeTransponderData;
        private IUpdate _uut;
        private ICalcVelocity _calcVelocity;
        private ICalcCourse _calcCourse;

        private List<ITracks> receivedTrackData;
        [SetUp]
        public void Setup()
        {
            _decoder = Substitute.For<ITrackDecoding>();
            receivedTrackData = new List<ITracks>();
            _uut = new Update(_decoder);
            _calcCourse = Substitute.For<ICalcCourse>();
            _calcVelocity = Substitute.For<ICalcVelocity>();
            //Tilsæt ny data
            _fakeTransponderData = new TrackDataEventArgs(new List<ITracks>());
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            //Her bliver eventet sat til at være lige med receivedTrackData
            _decoder.TrackDataReadyForCalculation += (o, args) => receivedTrackData = args.TrackData;
        }

        private void RaiseFakeEvent()
        {
           
            // Hæv eventet hvis _decoder har fået hævet flaget, indsæt den falske liste
            _decoder.TrackDataReadyForCalculation += Raise.EventWith(_fakeTransponderData);
           
        }

        [Test]
        public void Raised_Event_If_Two_Tracks_Appeared()
        {
            RaiseFakeEvent();
            
            Assert.That(receivedTrackData.Count, Is.EqualTo(2));
        }
        [Test]
        public void Raised_Event_If_Three_Tracks_Appeared()
        {
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            RaiseFakeEvent();

            Assert.That(receivedTrackData.Count, Is.EqualTo(3));
        }

        [Test]
        public void Calculator_Gets_Initialized()
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_calcCourse,_calcVelocity,receivedTrackData);
            Assert.That(_calcVelocity,Is.EqualTo(_calcVelocity));
        }

        [TestCase(1000,30000,1000,2000)]
        public void CalcVelocity_Get_Initialized_From_Public(int x1, int x2, int y1, int y2)
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_calcCourse, _calcVelocity, receivedTrackData);
            double timespan = x2 - x1;
            Assert.AreEqual(_uut.Velocity.Velocity(x1, x2, y1, y2, timespan), 0);
        }
        //Den skal ændre til det nye med velocity som er opdateret på
        [Test]
        public void Update_ReInitializes_List_It_Receives()
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_calcCourse, _calcVelocity, receivedTrackData);

            Assert.That(receivedTrackData.Count, Is.EqualTo(0));
        }

        [Test]
        public void CalcVelocity_Set()
        {
            _calcVelocity = new CalcVelocity();
            Assert.AreEqual(_calcVelocity,_calcVelocity);
        }
        [Test]
        public void CalcCourse_Set()
        {
            _calcCourse= new CalcCourse();
            Assert.AreEqual(_calcCourse, _calcCourse);
        }

        [Test]
        public void CalcCourse_Get()
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_calcCourse,_calcVelocity,receivedTrackData);
            Assert.AreEqual(_calcCourse, _calcCourse);
        }
        [TestCase(1000, 30000, 1000, 2000)]
        public void CalcCourse_Get_Initialized_From_Public(int x1, int x2, int y1, int y2)
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_calcCourse, _calcVelocity, receivedTrackData);
            Assert.AreEqual(_uut.Course.Calculate(x1, x2, y1, y2), 0);
        }
    }
}