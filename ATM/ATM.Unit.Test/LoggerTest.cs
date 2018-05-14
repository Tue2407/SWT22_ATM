using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ATMClasses.Interfaces;
using ATMClasses.Proximity_Detection;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class LoggerTest
    {
        private ITracks _track1;
        private ITracks _track2;
        private ILog _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new Logger();
            _track1 = Substitute.For<ITracks>();
            _track2 = Substitute.For<ITracks>();
            _track1.Tag = "Tag1";
            _track1.FormattedTimestamp = "20151006213456789";
            _track2.Tag = "Tag2";
            _track2.FormattedTimestamp = "20151006213456789";

        }

        [Test]
        public void Logger_Event_Happens()
        {
            _uut.LogSeparationEvent(_track1,_track2);
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
            string[] lines = System.IO.File.ReadAllLines(@"SeparatationEventLog.txt");

                //"Timestamp: 20151006213456789\tTag1 and Tag2 are breaking separation rules", lines[0] //Giver jenkins rigtigt
                //Assert.AreEqual("Timestamp: 14-05-2018 17:18 53 1111\tTAG1 and TAG1 are breaking separation rules", lines[0]);
                Assert.AreEqual("Timestamp: 20151006213456789\tTag1 and Tag2 are breaking separation rules", lines[0]);
            }
        }
    }
}