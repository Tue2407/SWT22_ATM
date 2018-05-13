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

    class DecodingWithEventTest
    {
        private ITrackDecoding _uut;
        private ITransponderReceiver _receiver;
        private RawTransponderDataEventArgs _fakeTransponderData;
        private RawTransponderDataEventArgs _fakeTransponderData2;

        private List<ITracks> receivedTrackData;
        [SetUp]
        public void Setup()
        {
            _receiver = Substitute.For<ITransponderReceiver>();
            _uut = new DecodingWithEvent(_receiver);

            _fakeTransponderData = new RawTransponderDataEventArgs(new List<string>()
                { "TRK001;12345;67890;12000;20151014123456789" }

            );

            // Make a fake subscriber to the event in UUT
            _uut.TrackDataReadyForCalculation += (o, args) => receivedTrackData = args.TrackData;

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
            Assert.That(receivedTrackData.Count, Is.EqualTo(1));
        }

        [Test]
        public void Decoding_OneTrackDataInList_TagIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Tag, Is.EqualTo("TRK001"));
        }

        [Test]
        public void Decoding_OneTrackDataInList_XIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].X, Is.EqualTo(12345));
        }

        [Test]
        public void Decoding_OneTrackDataInList_YIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Y, Is.EqualTo(67890));
        }

        [Test]
        public void Decoding_OneTrackDataInList_AltitudeIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Altitude, Is.EqualTo(12000));
        }

        [Test]
        public void Decoding_OneTrackDataInList_TimestampYearIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Year, Is.EqualTo(2015));
        }

        [Test]
        public void Decoding_OneTrackDataInList_TimestampMonthIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Month, Is.EqualTo(10));
        }


        [Test]
        public void Decoding_OneTrackDataInList_TimestampDayIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Day, Is.EqualTo(14));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampHourIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Hour, Is.EqualTo(12));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampMinuteIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Minute, Is.EqualTo(34));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampSecondIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Second, Is.EqualTo(56));
        }
        [Test]
        public void Decoding_OneTrackDataInList_TimestampMSIsCorrect()
        {
            RaiseFakeEvent();
            Assert.That(receivedTrackData[0].Timestamp.Millisecond, Is.EqualTo(789));
        }

        [Test]
        public void Decoding_TwoTrackDatasInList_CountIsCorrect()
        {
            _fakeTransponderData.TransponderData.Add("TRK002;12345;67890;12000;20151014123456789");
            RaiseFakeEvent();
            Assert.That(receivedTrackData.Count, Is.EqualTo(2));
        }

        [Test]
        public void Decoding_ThreeTrackDatasInList_CountIsCorrect()
        {
            _fakeTransponderData.TransponderData.Add("TRK002;12345;67890;12000;20151014123456789");
            _fakeTransponderData.TransponderData.Add("TRK003;12345;67890;12000;20151014123456789");
            RaiseFakeEvent();
            Assert.That(receivedTrackData.Count, Is.EqualTo(3));
        }

        [Test]
        public void Decoding_ThreeTrackDatasInList_TagOfThirdTrackDataIsCorrect()
        {
            _fakeTransponderData.TransponderData.Add("TRK002;12345;67890;12000;20151014123456789");
            _fakeTransponderData.TransponderData.Add("TRK003;12345;67890;12000;20151014123456789");
            RaiseFakeEvent();
            Assert.That(receivedTrackData[2].Tag, Is.EqualTo("TRK003"));
        }



    }
}
