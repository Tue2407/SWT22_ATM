using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using TransponderReceiver;

namespace ATMClasses.Decoding
{
    public class DecodingWithMethod
    {
        private List<TrackData> trackList;
        private ITrackReceiver outputTrackReceiver;

        public DecodingWithMethod(ITransponderReceiver rawReceiver, ITrackReceiver trackReceiver)
        {
            rawReceiver.TransponderDataReady += OnRawData;

            outputTrackReceiver = trackReceiver;

            trackList = new List<TrackData>();
        }

        public void OnRawData(object o, RawTransponderDataEventArgs args)
        {
            trackList.Clear();

            foreach (var data in args.TransponderData)
            {
                trackList.Add(Convert(data));
            }

            outputTrackReceiver.ReceiveTracks(trackList);
        }

        private TrackData Convert(string data)
        {
            TrackData track = new TrackData();
            var words = data.Split(';');
            track.Tag = words[0];
            track.X = int.Parse(words[1]);
            track.Y = int.Parse(words[2]);
            track.Altitude = int.Parse(words[3]);
            track.Timestamp = DateTime.ParseExact(words[4], "yyyyMMddHHmmssfff",
                System.Globalization.CultureInfo.InvariantCulture);
            track.Course = 0;
            track.Velocity = 0;

            return track;
        }
    }
}
