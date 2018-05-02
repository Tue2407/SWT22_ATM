using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using TransponderReceiver;

namespace ATM.Unit.Test
{
    [TestFixture]
    class DecodingWithMethodTest
    {
        private DecodingWithMethod _uut;
        private ITransponderReceiver _receiver;
        private RawTransponderDataEventArgs _fakeTransponderData;
        private ITrackReceiver _trackReceiver;


        [SetUp]
        public void Setup()
        {
            _receiver = Substitute.For<ITransponderReceiver>();
            _trackReceiver = Substitute.For<ITrackReceiver>();
            _uut = new DecodingWithMethod(_receiver, _trackReceiver);

            _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>()
                { "TRK001;12345;67890;12000;20151014123456789" }
            );
        }

        private void RaiseFakeEvent()
        {
            // Raise an event on the transponder receiver to spark decoding
            _receiver.TransponderDataReady += Raise.EventWith(_fakeTransponderData);
        }

        [Test]
        public void Decoding_OneTrackDataInList_CountIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x.Count == 1));
        }

        [Test]
        public void Decoding_OneTrackDataInList_TagIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Tag == "TRK001"));
        }

        [Test]
        public void Decoding_OneTrackDataInList_XIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].X == 12345));
        }

        [Test]
        public void Decoding_OneTrackDataInList_YIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Y == 67890));
        }

        [Test]
        public void Decoding_OneTrackDataInList_AltitudeIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Altitude == 12000));
        }

        [Test]
        public void Decoding_OneTrackDataInList_TimestampYearIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Year == 2015));
        }

        [Test]
        public void Decoding_OneTrackDataInList_TimestampMonthIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Month == 10));
        }


        [Test]
        public void Decoding_OneTrackDataInList_TimestampDayIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Day == 14));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampHourIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Hour == 12));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampMinuteIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Minute == 34));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampSecondIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Second == 56));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampMSIsCorrect()
        {
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[0].Timestamp.Millisecond == 789));
        }


        [Test]
        public void Decoding_ThreeTrackDatasInList_CountIsCorrect()
        {
            _fakeTransponderData.TransponderData.Add("TRK002;12345;67890;12000;20151014123456789");
            _fakeTransponderData.TransponderData.Add("TRK003;12345;67890;12000;20151014123456789");
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x.Count == 3));
        }

        [Test]
        public void Decoding_ThreeTrackDatasInList_TagOfThirdTrackDataIsCorrect()
        {
            _fakeTransponderData.TransponderData.Add("TRK002;12345;67890;12000;20151014123456789");
            _fakeTransponderData.TransponderData.Add("TRK003;12345;67890;12000;20151014123456789");
            RaiseFakeEvent();
            _trackReceiver.Received().ReceiveTracks(Arg.Is<List<TrackData>>(x => x[2].Tag == "TRK003"));
        }


    }
}
