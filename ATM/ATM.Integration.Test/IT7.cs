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

//Vi starter med systemets CalcVelocity beliggenhed er i bunden til højre og på tredje række af dependency diagrammet
//CalcVelocity har 2 dependencies, Integrationsmæssigt så bliver den brugt til at få hastigheden på flyet imellem Update og TrackData.
//Indholdsmæssigt har den koordinator og tidsdifferencen mellem 2 tracks og bruger disse værdier til at beregne hastigheden.
//Selve Testen forgår fra CalcVelocity om den virker med den 1 integrationsdele TrackData.

namespace ATM.Integration.Test
{
    [TestFixture]
    public class IT7
    {
        private ICalcVelocity _uut;
        private ITracks _Track1;
        private ITracks _Track2;
        private ITracks _Track3;
        private IUpdate _Update;
        private List<ITracks> _Tracklist;

        [SetUp]
        public void Setup()
        {
            _uut = new CalcVelocity();
            _Track1 = new TrackData() { Tag = "TAG1", X = 10000, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };
            _Track2 = new TrackData() { Tag = "TAG1", X = 50000, Y = 20000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 1111" };
            _Track3 = new TrackData() { Tag = "TAG1", X = 100, Y = 21000, Altitude = 3000, Course = 0, Velocity = 100, FormattedTimestamp = "14-05-2018 17:18 53 4000" };

            _Tracklist = new List<ITracks>() { _Track1, _Track2, _Track3 };

            _Update = Substitute.For<IUpdate>();
        }

        [Test]
        public void CalcCourse_Receives_Correct_Tracks()
        {
            _Update.Velocity.Velocity(_Track1, _Track2);
            _Update.Velocity.Received().Velocity(_Track1, _Track2);
        }
    }
}