using System.Collections.Generic;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class UpdateTest
    {
        private ITrackDecoding _decoder;
        private TrackDataEventArgs _fakeTransponderData;
        private IUpdate _uut;

        private List<ITracks> receivedTrackData;
        [SetUp]
        public void Setup()
        {
            _decoder = Substitute.For<ITrackDecoding>();
            receivedTrackData = new List<ITracks>();
            _uut = new Update(_decoder);

            //Tilsæt ny data
            _fakeTransponderData = new TrackDataEventArgs(new List<ITracks>());
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            _fakeTransponderData.TrackData.Add(Substitute.For<ITracks>());
            //Her bliver eventet sat til at være lige med receivedTrackData
            _decoder.TrackDataReadyForCalculation += (o, args) => receivedTrackData = args.TrackData;

        }

        private void RaiseFakeEvent()
        {
           
            // Hæv eventet hvis _decoder har fået hævet flaget, indsæt den falske liste
            _decoder.TrackDataReadyForCalculation += Raise.EventWith(_fakeTransponderData);
           
        }

        [Test]
        public void Raised_Event_If_Two_Tracks_Appeared()
        {
            RaiseFakeEvent();
            
            Assert.That(receivedTrackData.Count, Is.EqualTo(2));
        }
    }
}