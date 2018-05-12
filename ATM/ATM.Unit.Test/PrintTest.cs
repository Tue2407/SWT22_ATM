using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using ATMClasses.Output;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    //Printer kun når flyet er inde for monitor (dvs. når den er true)
    [TestFixture]
    public class PrintTest
    {
        private Print _uut;
        private List<ITracks> _tracks;
        private ITracks _track;
        private IOutput _output;
        private IMonitors _monitor;
        private ICalcVelocity _calcvelocity;
        private ICalcCourse _calccourse;
        private IUpdate _update;
        private ILog _logger;
        private ISeparation _separation;

        [SetUp]
        public void Setup()
        {
            _calccourse = Substitute.For<ICalcCourse>();
            _calcvelocity = Substitute.For<ICalcVelocity>();
            _update = Substitute.For<IUpdate>();
            _output = Substitute.For<IOutput>();
            _tracks = new List<ITracks>();
            _track = Substitute.For<TrackData>();
            _monitor = Substitute.For<IMonitors>();
            _uut = new Print(_update,_calccourse,_calcvelocity, _logger, _separation,_monitor,_output, _tracks);

        }
        public void Action(string tag, int x, int y, int z)
        {
            
            _track.Tag = tag;
            _track.X = x;
            _track.Y = y;
            _track.Altitude = z;

            _tracks.Add(_track);
            //Den tilsætter monitor til true.
            _monitor.MonitorFlight(_track).Returns(true);
            //_uut.Printing(_tracks);

        }

        [TestCase("TRK001", 3000, 4000, 5000)]
        [TestCase("TRK002", 3000, 4000, 5000)]
        [TestCase("TRK003", 3000, 4000, 5000)]
        [TestCase("TRK004", 3000, 4000, 5000)]
        public void Print_Contains_Tag(string tag, int x, int y, int z)
        {

            Action(tag, x, y, z);
            _uut.Printing(_tracks,_monitor);
            
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_track.Tag)));
        }

        [TestCase("TRK001", 3000, 4000, 5000)]
        [TestCase("TRK002", 3000, 4000, 5000)]
        [TestCase("TRK003", 33000, 4000, 5000)]
        [TestCase("TRK004", 3000, 4000, 5000)]
        public void Print_Contains_XCoord(string tag, int x, int y, int z)
        {

            Action(tag, x, y, z);
            _uut.Printing(_tracks, _monitor);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_track.X.ToString())));
        }
        [TestCase("TRK001", 3000, 4000, 5000)]
        [TestCase("TRK002", 3000, 4000, 5000)]
        [TestCase("TRK003", 3000, 4000, 5000)]
        [TestCase("TRK004", 3000, 4000, 5000)]
        public void Print_Contains_YCoord(string tag, int x, int y, int z)
        {

            Action(tag, x, y, z);
            _uut.Printing(_tracks, _monitor);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_track.Y.ToString())));
        }
        [TestCase("TRK001", 3000, 4000, 5000)]
        [TestCase("TRK002", 3000, 4000, 5000)]
        [TestCase("TRK003", 3000, 4000, 5000)]
        [TestCase("TRK004", 3000, 4000, 5000)]
        public void Print_Contains_Velocity(string tag, int x, int y, int z)
        {

            Action(tag, x, y, z);
            _uut.Printing(_tracks, _monitor);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_track.Velocity.ToString())));
        }
        [TestCase("TRK001", 3000, 4000, 5000)]
        [TestCase("TRK002", 3000, 4000, 5000)]
        [TestCase("TRK003", 3000, 4000, 5000)]
        [TestCase("TRK004", 3000, 4000, 5000)]
        public void Print_Contains_Course(string tag, int x, int y, int z)
        {

            Action(tag, x, y, z);
            _uut.Printing(_tracks, _monitor);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(_track.Course.ToString())));
        }

        [TestCase("TRK004", 3000, 4000, 5000)]
        public void No_Call_From_Output(string tag, int x, int y, int z)
        {
            Action(tag, x, y , z);
            _monitor.MonitorFlight(_track).Returns(false);
            _uut.Printing(_tracks, _monitor);
            
            //Den skal ikke modtage noget received output pga. monitor er nu false
        }


    }
}