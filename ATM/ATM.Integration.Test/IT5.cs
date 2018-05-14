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
using NSubstitute.Extensions;
using NUnit.Framework;

// Bottom-up. 

//Vi starter med systemets CalcDistance beliggenhed er i bunden til venstre og på tredje række af dependency diagrammet
//CalcDistance har 3 dependencies, Integrationsmæssigt så bliver den brugt til at få distancen mellem SeparationsTesten og Tracks er korrekt til et separations event.
//Indholdsmæssigt har den koordinator fra 2 tracks og bruger den værdi til at se om der er en separations event.
//Selve Testen forgår fra CalcDistance om den virker med de 2 integrationsdele TrackData og SeparationsEventet.

namespace ATM.Integration.Test
{
    [TestFixture]
    public class IT5
    {
        private ICalcDistance _uut;
        private ITracks _Track1;
        private ITracks _Track2;
        private ITracks _Track3;
        private ISeparation _Event;


        [SetUp]
        public void Setup()
        {
            _uut = new CalcDistance();
            _Track1 = new TrackData() { Tag = "TAG1", X = 10000, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };
            _Track2 = new TrackData() { Tag = "TAG1", X = 50000, Y = 20000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 1111" };
            _Track3 = new TrackData() { Tag = "TAG1", X = 100, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };

            _Event = Substitute.For<ISeparation>();

        }

        [Test]
        public void Received_Right_Tracks()
        {
            _Event.CollisionDetection(_uut, _Track1, _Track2);
            _Event.Received().CollisionDetection(_uut, _Track1, _Track2);
        }
    }
}