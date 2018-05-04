using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using ATMClasses.Output;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class PrintTest
    {
        private Print _uut;
        private List<TrackData> _tracks;
        private TrackData _track;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _tracks = new List<TrackData>();
            _track = Substitute.For<TrackData>();
            _uut = new Print(_tracks);
        }
        public void Action(string tag ,int x, int y, int z)
        {
            _track.Tag = tag;
            _track.X = x;
            _track.Y = y;
            _track.Altitude = z;

            _tracks.Add(_track);

            //_uut.Printing(_tracks);
           
        }

        //[TestCase("TRK001", 10000, 10000, 500)]
        //[TestCase("TRK001", 90000, 90000, 500)]
        //[TestCase("TRK001", 10000, 10000, 20000)]
        //[TestCase("TRK001", 90000, 90000, 20000)]
        [Test]
        public void Print_Contains()
        {
            string tag = "TRK";
            int x = 10;
            int y = 200;
            int alt = 50;
            Action(tag, x, y, alt);
            _uut = new Print(_tracks);
            _uut.Printing(_tracks);
            Console.WriteLine(_track.Tag);
            _output.Received().OutputLine(Arg.Is<string>(track => _track.Tag.Contains("TRK")));
        }
    }
}