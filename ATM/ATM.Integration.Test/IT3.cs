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

// Bottom-up. 

//Vi starter med systemets Update beliggenhed i midt og på tredje række af dependency diagrammet
//Update har 6 dependencies, Integrationsmæssigt med så sker beslutningerne her i denne klasse, hvor alt som er filtreret kan føres igennem her.
//Indholdsmæssigt så har Update alle klasser som dependencies bortset fra print og decodingwithevents, hvilket er mere eller mindre er en parsing funktion.
//Selve Testen forgår på Update om den virker med de 4 integrationsdele.


namespace ATM.Integration.Test
{
    [TestFixture]
    public class IT3
    {
        private ILog _logger;
        private ISeparation _Separation;
        private ITracks _Track1;
        private ITracks _Track2;
        private ITracks _Track3;
        private ICalcDistance _CalcDistance;
        private IUpdate _uut;
        private ITrackDecoding _decoder;
        private ICalcVelocity _calcVelocity;
        private ICalcCourse _calcCourse;
        private IMonitors _monitor;
        private TrackDataEventArgs _fakeTransponderData;
        private IOutput _output;
        private IPrints _print;

        private List<ITracks> _Tracklist;

        [SetUp]
        public void SetUp()
        {
            _decoder = Substitute.For<ITrackDecoding>();
            //Rigtige Klasser
            _Track1 = new TrackData() { Tag = "TAG1", X = 10000, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };
            _Track2 = new TrackData() { Tag = "TAG1", X = 50000, Y = 20000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 1111" };
            _Track3 = new TrackData() { Tag = "TAG1", X = 100, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };
            _CalcDistance = new CalcDistance();
            _calcCourse = new CalcCourse();
            _calcVelocity = new CalcVelocity();
            _monitor = new Monitor();
            

            _uut = new Update(_decoder);
            //Lister

            _Tracklist = new List<ITracks>() { _Track1, _Track2 };
            _fakeTransponderData = new TrackDataEventArgs(_Tracklist);

            //Substitueret
            _logger = Substitute.For<ILog>();
            _output = Substitute.For<IOutput>();
            _print = Substitute.For<IPrints>();
            _Separation = Substitute.For<ISeparation>();


            _decoder.TrackDataReadyForCalculation += (o, args) => _Tracklist = args.TrackData;
        }

        public void Action()
        {
            //Går ind i update løkken der finder ud af om de har kollision
            _monitor.Track = _Tracklist;
            _uut.TrackCalculated(_monitor, _CalcDistance, _calcCourse, _calcVelocity, _logger, _Separation, _Tracklist);

        }
        private void RaiseFakeEvent()
        {
            // Hæv eventet hvis _decoder har fået hævet flaget, indsæt den falske liste
            _decoder.TrackDataReadyForCalculation += Raise.EventWith(_fakeTransponderData);

        }


        [Test]
        public void Monitor_Is_True_Through_Update()
        {
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();
            //Check om den får true
            Assert.That(_monitor.MonitorFlight(_Track1),Is.True);
            
        }
        [Test]
        public void Monitor_Is_False_Through_Update()
        {
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();
            //Check om den får false pga. X coord
            Assert.That(_monitor.MonitorFlight(_Track3), Is.False);

        }
        [Test]
        public void CalcVelocity_Calcs_If_Monitor_Is_True_Through_Update()
        {

            _Track1.X = 20000;
            _Track2.X = 10000;
            _Track1.Y = 20000;
            _Track2.Y = 20000;
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();

            //Check om den får true
            Assert.That(_Tracklist[1].Velocity, Is.EqualTo(0));
        }
        [Test]
        public void CalcVelocity_Receives_Call_If_Monitor_Is_True_Through_Update()
        {
            _calcVelocity = Substitute.For<ICalcVelocity>();
            _Track1.X = 20000;
            _Track2.X = 10000;
            _Track1.Y = 20000;
            _Track2.Y = 20000;
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();

            //Check om den får kaldet
            _calcVelocity.Received().Velocity(_Track1, _Track2);

        }
        [Test]
        public void CalcVelocity_Receives_Correct_Tracks()
        {
            _calcVelocity = Substitute.For<ICalcVelocity>();
            Action();
            RaiseFakeEvent();
            RaiseFakeEvent();
            _uut.UpdatesTrack(_Tracklist);

            _calcVelocity.Received().Velocity(_Track1, _Track2);
        }
        [Test]
        public void CalcCourse_Calcs_If_Monitor_Is_True_Through_Update()
        {

            _Track1.X = 20000;
            _Track2.X = 10000;
            _Track1.Y = 20000;
            _Track2.Y = 20000;
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();

            //Check om den får true
            Assert.That(_Tracklist[1].Course,Is.EqualTo(270));
        }

        [Test]
        public void CalcCourse_Receives_Correct_Tracks()
        {
            _calcCourse = Substitute.For<ICalcCourse>();
            Action();
            RaiseFakeEvent();
            RaiseFakeEvent();
            _uut.UpdatesTrack(_Tracklist);
            
            _calcCourse.Received().Calculate(_Track1, _Track2);
        }

        [Test]
        public void CalcCourse_Receives_Call_If_Monitor_Is_True_Through_Update()
        {
            _calcCourse = Substitute.For<ICalcCourse>();
            _Track1.X = 20000;
            _Track2.X = 10000;
            _Track1.Y = 20000;
            _Track2.Y = 20000;
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();

            //Check om den får kaldet
            _calcCourse.Received().Calculate(_Track1, _Track2);

        }

        [Test]
        public void Monitor_Receives_Monitor_Flight_Calls_From_Update()
        {
            _monitor = Substitute.For<IMonitors>();
            _Track1.X = 20000;
            _Track2.X = 20000;
            _Track1.Y = 10000;
            _Track2.Y = 10000;
            _Track1.Altitude = 100;
            Action();
            RaiseFakeEvent();
            //Tracks osv. Check
            RaiseFakeEvent();

            //Check om den får kaldet
            _monitor.Received().MonitorFlight(_Track1);
            _monitor.Received().MonitorFlight(_Track2);
        }
    }
}
