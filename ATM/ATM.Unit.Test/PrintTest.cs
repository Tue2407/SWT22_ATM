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
            _uut = new Print(_output, _tracks);
        }
        public void Action(string tag)
        {
            _track.Tag = tag;
            //_track.X = x;
            //_track.Y = y;
            //_track.Altitude = z;

            _tracks.Add(_track);

            //_uut.Printing(_tracks);
           
        }

        [TestCase("TRK001")]
        [TestCase("TRK002")]
        [TestCase("TRK003")]
        [TestCase("TRK004")]
        public void Print_Contains(string tag)
        {
            //string tag = "TRK";
            //int x = 10;
            //int y = 200;
            //int alt = 50;
            Action(tag);
            _uut.Printing(_tracks);
            
            //_output.Received().OutputLine(_uut.ToString());
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_track.Tag)));
        }
    }
}