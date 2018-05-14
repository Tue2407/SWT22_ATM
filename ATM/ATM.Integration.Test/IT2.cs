using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses;
using ATMClasses.Data;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;
using ATMClasses.Output;
using ATMClasses.Proximity_Detection;
using ATMClasses.Render;
using ATMClasses.TrackUpdate;
using NSubstitute;
using NUnit.Framework;

// Bottom-up

//Vi starter med systemets SeparationEvent på venstre bund anden række af dependency diagrammet
//SeparationEvent har 3 dependencies, Integrationsmæssigt med Update.UpdateTracks, såvel som Constructor Injection med CalcDistance og indholdsmæssigt med TrackData.
//Selve Testen forgår på SeparationsEventet om den virker med de 3 integrationsdele.
namespace ATM.Integration.Test
{
    [TestFixture]
    class IT2
    {
        private ILog _logger;
        private ISeparation _uut;
        private TrackData _Track1;
        private TrackData _Track2;
        private ICalcDistance _CalcDistance;
        private IUpdate _Update;
        private ITrackDecoding _decoder;
        private ICalcVelocity _calcVelocity;
        private ICalcCourse _calcCourse;
        private IMonitors _monitor;
        private TrackDataEventArgs _fakeTransponderData;
        private IOutput _output;
        private Print _print;

        private List<ITracks> _Tracklist;

        [SetUp]
        public void SetUp()
        {
            _decoder = Substitute.For<ITrackDecoding>();
            //Rigtige Klasser
            _uut = new SeparationEvent();
            _Track1 = new TrackData() { Tag = "TAG1", X = 1000, Y = 2000, Altitude = 3000, Course = 30, Velocity = 23, FormattedTimestamp = "14-05-2018 17:18 53 1111" };
            _Track2 = new TrackData() { Tag = "TAG2", X = 1000, Y = 2000, Altitude = 3000, Course = 30, Velocity = 23, FormattedTimestamp = "14-05-2018 17:18 53 1111" };
            _CalcDistance = new CalcDistance();
            

            _Update = new Update(_decoder);
            //Lister

            _Tracklist = new List<ITracks>() { _Track1, _Track2 };
            _fakeTransponderData = new TrackDataEventArgs(_Tracklist);

            //Substitueret
            _calcCourse = Substitute.For<ICalcCourse>();
            _calcVelocity = Substitute.For<ICalcVelocity>();
            _monitor = Substitute.For<IMonitors>();
            _logger = Substitute.For<ILog>();
            _output = Substitute.For<IOutput>();


            _print = new Print(_Update, _CalcDistance, _calcCourse, _calcVelocity, _logger, _uut, _monitor, _output, _Tracklist);
            _decoder.TrackDataReadyForCalculation += (o, args) => _Tracklist = args.TrackData;
        }

        public void Action()
        {
            //Går ind i update løkken der finder ud af om de har kollision
            _monitor.Track = _Tracklist;
            _Update.TrackCalculated(_monitor, _CalcDistance, _calcCourse, _calcVelocity, _logger, _uut, _Tracklist);
            
        }
        private void RaiseFakeEvent()
        {
            // Hæv eventet hvis _decoder har fået hævet flaget, indsæt den falske liste
            _decoder.TrackDataReadyForCalculation += Raise.EventWith(_fakeTransponderData);

        }

        [Test]
        public void When_Update_Gets_Two_Different_Tags_SeparationEvent_Checks_And_Gets_True_On_Collision()
        {
            //Hæv og init _uut, _CalcDistance, _Track1, _Track2
            Action();
            RaiseFakeEvent(); //Den går igennem Update.TrackUpdate, ser 2 forskellige tags og undersøger den her.
            RaiseFakeEvent(); //Den går igennem Update.TrackUpdate, ser 2 forskellige tags og undersøger den her.
            //Check om _uut faktisk bliver true med de tracks den "looper" igennem.
            Assert.That(_uut.CollisionDetection(_CalcDistance,_Track1,_Track2),Is.EqualTo(true));
            _monitor.MonitorFlight(_Track1).Returns(true);

            _print.Printing(_Tracklist, _monitor);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_Track1.Velocity.ToString())));
        }
        [Test]
        public void
        When_Update_Gets_Two_Different_Tags_SeparationEvent_Checks_And_Gets_True_On_Collision_Then_Still_Update_Track()
        {
            //Hæv og init _uut, _CalcDistance, _Track1, _Track2
            Action();
            RaiseFakeEvent(); //Den går igennem Update.TrackUpdate, ser 2 forskellige tags og undersøger den her.
            RaiseFakeEvent(); //Den går igennem Update.TrackUpdate, ser 2 forskellige tags og undersøger den her.
            Assert.That(_Track1.Velocity,Is.EqualTo(23));
            _monitor.MonitorFlight(_Track1).Returns(true);

            _print.Printing(_Tracklist, _monitor);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_Track1.Velocity.ToString())));
        }
        [Test]
        public void
            When_Update_Gets_Two_Different_Tags_SeparationEvent_Checks_And_Gets_False_On_Collision_Then_Still_Update_Track()
        {
            //Hæv og init _uut, _CalcDistance, _Track1, _Track2
            Action();
            _Track1.Altitude = 1000;
            _Track1.X = 45000;
            
            RaiseFakeEvent(); //Den går igennem Update.TrackUpdate, ser 2 forskellige tags og undersøger den her.
            RaiseFakeEvent(); //Den går igennem Update.TrackUpdate, ser 2 forskellige tags og undersøger den her.
            //Vi checker lige om det kan mon passe at den skal give false
            Assert.That(_uut.CollisionDetection(_CalcDistance,_Track1,_Track2),Is.EqualTo(false));
            //Den opdatere stadigvæk hastigheden selvom de ikke er de samme
            Assert.That(_Track1.Velocity, Is.EqualTo(23));
            _monitor.MonitorFlight(_Track1).Returns(true);

            _print.Printing(_Tracklist,_monitor);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_Track1.Velocity.ToString())));
        }
    }
}
