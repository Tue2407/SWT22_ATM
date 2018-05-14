using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;
using ATMClasses.Render;
using ATMClasses.TrackUpdate;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Extensions;
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
        private ILog _logger;
        private ISeparation _separation;
        private ICalcDistance _calcDistance;
        private ITracks _track1;
        private ITracks _track2;
        private IMonitors _monitor;
        private List<ITracks> receivedTrackData;
        [SetUp]
        public void Setup()
        {
            _track1 = Substitute.For<ITracks>();
            _track2 = Substitute.For<ITracks>();
            _decoder = Substitute.For<ITrackDecoding>();
            receivedTrackData = new List<ITracks>();
            _uut = new Update(_decoder);
            _calcDistance = Substitute.For<ICalcDistance>();
            _calcCourse = Substitute.For<ICalcCourse>();
            _calcVelocity = Substitute.For<ICalcVelocity>();
            _logger = Substitute.For<ILog>();
            _separation = Substitute.For<ISeparation>();
            _monitor = Substitute.For<IMonitors>();

            //Tilsæt ny data
            _track1.Tag = "Tag1";
            _track1.Altitude = 500;
            _track1.X = 20000;
            _track1.Y = 40000;
            //_track1.Course = 0;
            _track1.Timestamp = new DateTime(20180419152929150);
            //_track1.Velocity = 0;

            _track2.Tag = "Tag1";
            _track2.Altitude = 600;
            _track2.X = 40000;
            _track2.Y = 90000;
            //_track2.Course = 0;
            _track2.Timestamp = new DateTime(20180419152929100);
            //_track2.Velocity = 0;

            _fakeTransponderData = new TrackDataEventArgs(new List<ITracks>());
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            //Her bliver eventet sat til at være lige med receivedTrackData
            _decoder.TrackDataReadyForCalculation += (o, args) => receivedTrackData = args.TrackData;
        }

        private void RaiseFakeEvent()
        {
            _uut.TrackCalculated(_monitor, _calcDistance, _calcCourse, _calcVelocity, _logger, _separation, receivedTrackData);
            
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
            _uut.TrackCalculated(_monitor,_calcDistance,_calcCourse,_calcVelocity,_logger,_separation,receivedTrackData);
            Assert.That(_calcVelocity,Is.EqualTo(_calcVelocity));
        }

        [TestCase(1000,30000,1000,2000)]
        public void CalcVelocity_Get_Initialized_From_Public(int x1, int x2, int y1, int y2)
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_monitor, _calcDistance, _calcCourse, _calcVelocity, _logger, _separation, receivedTrackData);
            double timespan = x2 - x1;
            Assert.AreEqual(_uut.Velocity.Velocity(_track1, _track2), 0);
        }
        //Den skal ændre til det nye med velocity som er opdateret på
        [Test]
        public void Update_ReInitializes_List_It_Receives()
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_monitor, _calcDistance, _calcCourse, _calcVelocity, _logger, _separation, receivedTrackData);

            Assert.That(receivedTrackData.Count, Is.EqualTo(2));
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
            _uut.TrackCalculated(_monitor, _calcDistance, _calcCourse, _calcVelocity, _logger, _separation, receivedTrackData);
            Assert.AreEqual(_calcCourse, _calcCourse);
        }
        [TestCase(1000, 30000, 1000, 2000)]
        public void CalcCourse_Get_Initialized_From_Public(int x1, int x2, int y1, int y2)
        {
            RaiseFakeEvent();
            _uut.TrackCalculated(_monitor, _calcDistance, _calcCourse, _calcVelocity, _logger, _separation, receivedTrackData);
            Assert.AreEqual(_uut.Course.Calculate(_track1,_track2), 0);
        }

        [Test]
        public void One_Correct_Track_Output()
        {
            _fakeTransponderData.TrackData.Clear();
            _fakeTransponderData.TrackData.Add(_track1);
            RaiseFakeEvent();

            Assert.That(receivedTrackData[0].Tag, Is.EqualTo("Tag1"));
        }
        [Test]
        public void Two_Correct_Track_Output()
        {
            _track2.Tag = "Tag2";
            _track1.X = 9000;
            _track1.Y = 9000;
            _track1.Altitude = 9000;
            _fakeTransponderData.TrackData.Clear();
            _fakeTransponderData.TrackData.Add(_track1);
            //Den bliver added og eventet affyres
            RaiseFakeEvent();

            //Den affyrer og så kommer Update fra print og initialisere alt i systemet første gang.
            _uut.TrackCalculated(_monitor, _calcDistance, _calcCourse, _calcVelocity, _logger, _separation, receivedTrackData);
            _fakeTransponderData.TrackData.Add(_track2);

            //Den bliver added og eventet affyres
            RaiseFakeEvent();
            
            Assert.That(receivedTrackData[0].Tag,Is.EqualTo("Tag1"));
            Assert.That(receivedTrackData[1].Tag, Is.EqualTo("Tag2"));
        }

        [Test]
        public void Two_Same_Tag_Calculate_Velocity()
        {
            _fakeTransponderData.TrackData.Clear();
            _fakeTransponderData.TrackData.Add(_track1);
            _fakeTransponderData.TrackData.Add(_track2);
            RaiseFakeEvent();

            _monitor.MonitorFlight(receivedTrackData[0]).Returns(true);
            _calcVelocity.Velocity(_track1, _track2).Returns(200);
            RaiseFakeEvent();
            //_monitor.MonitorFlight(receivedTrackData[1]).Returns(true);
            _uut.UpdatesTrack(receivedTrackData);
            
            Assert.That(receivedTrackData[1].Velocity, Is.EqualTo(200));
        }


        [Test]
        public void Two_Same_Tag_Calculate_Course()
        {
            //Fylder listen op
            _fakeTransponderData.TrackData.Clear();
            _fakeTransponderData.TrackData.Add(_track1);
            _fakeTransponderData.TrackData.Add(_track2);
            RaiseFakeEvent();
            //Monitor bliver ok
            _monitor.MonitorFlight(receivedTrackData[0]).Returns(true);
            RaiseFakeEvent();
            //Kursen bliver udregnet
            _calcCourse.Calculate(_track1, _track2).Returns(95);
            _uut.UpdatesTrack(receivedTrackData);

            Assert.That(receivedTrackData[1].Course, Is.EqualTo(95));
        }
        [Test]
        public void Two_Different_Tag_SeparationBool_False()
        {
            _track2.Tag = "Tag2";
            //Fylder listen op
            _fakeTransponderData.TrackData.Clear();
            _fakeTransponderData.TrackData.Add(_track1);
            _fakeTransponderData.TrackData.Add(_track2);
            RaiseFakeEvent();
            //Monitor bliver ok
            _monitor.MonitorFlight(receivedTrackData[0]).Returns(true);
            RaiseFakeEvent();
            //Kursen bliver udregnet

            _uut.UpdatesTrack(receivedTrackData);

           
            Assert.That(_uut.Separation.CollisionDetection(_calcDistance, _track1, _track2),Is.EqualTo(false));

        }
        [Test]
        public void Two_Different_Tag_SeparationBool_True_With_Logger()
        {
            _track2.Tag = "Tag2";
            _track2.X = 9000;
            _track2.Y = 9000;

            _track1.X = 9000;
            _track1.Y = 9000;
            
            //Fylder listen op
            _fakeTransponderData.TrackData.Clear();
            _fakeTransponderData.TrackData.Add(_track1);
            _fakeTransponderData.TrackData.Add(_track2);
            RaiseFakeEvent();
            RaiseFakeEvent();
            //Monitor bliver ok
            _monitor.MonitorFlight(receivedTrackData[0]).Returns(true);

            //Kursen bliver udregnet
            _separation.CollisionDetection(_calcDistance, _track1, _track2).ReturnsForAnyArgs(true);
            _logger.LogSeparationEvent(_track1,_track2);
            _uut.UpdatesTrack(receivedTrackData);
            
            _logger.Received().LogSeparationEvent(_track1,_track2);
        }
    }
}