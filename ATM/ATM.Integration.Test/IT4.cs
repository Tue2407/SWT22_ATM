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

//Vi starter med systemets Monitor beliggenhed er i midt og på fjerde række af dependency diagrammet
//Monitor har 2 dependencies, Integrationsmæssigt så bliver den brugt til at se om Dataen som bliver parset overhovedet er det værd at gøre noget ved, derfor den bliver tillagt navnet Filtering.
//Indholdsmæssigt har Monitor Dependencies med Update hvor den bliver brugt, derefter TrackData hvor den tester den om det fungere.
//Selve Testen forgår fra Monitor om den virker med de 1 integrationsdele TrackData, fordi IT3 allerede tester Monitor med Update delen.

namespace ATM.Integration.Test
{
    [TestFixture]
    public class IT4
    {
        private Monitor _uut;
        private ITracks _Track1;
        private IUpdate _Update;

        [SetUp]
        public void Setup()
        {

            //Rigtige Klasser
            _uut = new Monitor();
            _Track1 = new TrackData() { Tag = "TAG1", X = 10000, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };
            
            _Update = Substitute.For<IUpdate>();
        }

        public void Action()
        {
            _uut.Track = new List<ITracks>(){ _Track1 };
        }

        [Test]
        public void Monitor_Gets_True_If_Track_Is_Not_Breaking_Boundaries()
        {
            Action();
            Assert.AreEqual(_uut.MonitorFlight(_Track1),true);
            Assert.That(_uut.Track[0].Tag,Is.EqualTo(_Track1.Tag));
        }

        [Test]
        public void Monitor_Gets_False_If_Track_Is_Breaking_Boundaries()
        {
            Action();
            _Track1.Altitude = 100;
            Assert.AreEqual(_uut.MonitorFlight(_Track1), false);
            Assert.That(_uut.Track[0].Altitude, Is.EqualTo(_Track1.Altitude));
        }

        [Test]
        public void Monitor_Receives_Right_Tracks()
        {
            _Update.Monitor.MonitorFlight(_Track1);
            _Update.Received().Monitor.MonitorFlight(_Track1);
        }
    }
    
}