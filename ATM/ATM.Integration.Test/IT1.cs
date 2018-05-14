using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ATMClasses;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using ATMClasses.Proximity_Detection;
using ATMClasses.TrackUpdate;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;

// Bottom-up. 

//Vi starter med systemets logger på venstre bund af dependency diagrammet
//Logger har 2 dependencies, Integrationsmæssigt med SeparationsEvent.Collision, og indholdsmæssigt med TrackData.
//Selve Testen forgår på Loggeren om den virker med de 2 integrationsdele.

namespace ATM.Integration.Test
{
    [TestFixture]
    public class IT1
    {

        private Logger _uut;
        private ISeparation _Event;
        private TrackData _Track1;
        private TrackData _Track2;
        private ICalcDistance _CalcDistance;
        private IUpdate _Update;
        private ITrackDecoding _decoder;
        private ICalcVelocity _calcVelocity;
        private ICalcCourse _calcCourse;
        private IMonitors _monitor;
        private TrackDataEventArgs _fakeTransponderData;

        private List<ITracks> _Tracklist;

        [SetUp]
        public void SetUp()
        {
            _decoder = Substitute.For<ITrackDecoding>();
            //Rigtige Klasser
            _uut = new Logger();
            _Event = new SeparationEvent();
            _Track1 = new TrackData(){Tag = "TAG1", X = 1000, Y = 2000, Altitude = 3000 , Course = 30 , Velocity = 23, FormattedTimestamp = "14-05-2018 17:18 53 1111" };
            _Track2 = new TrackData(){ Tag = "TAG2", X = 1000, Y = 2000, Altitude = 3000, Course = 30, Velocity = 23, FormattedTimestamp = "14-05-2018 17:18 53 1111" };

            _Update = new Update(_decoder);
            //Lister

            _Tracklist = new List<ITracks>(){_Track1, _Track2};
            _fakeTransponderData = new TrackDataEventArgs(_Tracklist);

            //Substitueret
            _CalcDistance = Substitute.For<ICalcDistance>();
            _calcCourse = Substitute.For<ICalcCourse>();
            _calcVelocity = Substitute.For<ICalcVelocity>();
            _monitor = Substitute.For<IMonitors>();

            _decoder.TrackDataReadyForCalculation += (o, args) => _Tracklist = args.TrackData;
        }

        public void Action()
        {
            //Går ind i update løkken der finder ud af om de har kollision

            _Update.TrackCalculated(_monitor, _CalcDistance, _calcCourse, _calcVelocity, _uut, _Event, _Tracklist);
        }
        private void RaiseFakeEvent()
        {
            // Hæv eventet hvis _decoder har fået hævet flaget, indsæt den falske liste
            _decoder.TrackDataReadyForCalculation += Raise.EventWith(_fakeTransponderData);

        }

        
        [Test]
        public void If_SeparationEvent_Is_True_Logger_Starts_And_Show_That_TrackData_Is_Used()
        {
            //Man hæver eventet og logger bliver gjort her
            Action(); //Logger og Tracks bliver initialiseret og sat her
            RaiseFakeEvent(); //Eventet sker
            RaiseFakeEvent(); //Eventet sker

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                string[] lines = System.IO.File.ReadAllLines(@"SeparatationEventLog.txt");

                //("Timestamp: 20151006213456789\tTag1 and Tag2 are breaking separation rules", lines[0]); //Jenkins Virker ellers ikke
                //Assert.AreEqual("Timestamp: 14-05-2018 17:18 53 1111\tTAG1 and TAG1 are breaking separation rules", lines[0]);
                Assert.AreEqual("Timestamp: 20151006213456789\tTag1 and Tag2 are breaking separation rules", lines[0]);
            }
        }
       
    }
}
