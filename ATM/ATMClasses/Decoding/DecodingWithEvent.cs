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
    public class DecodingWithEvent : ITrackDecoding
    {
        private List<ITracks> trackList;
        public event EventHandler<TrackDataEventArgs> TrackDataReady; //Eventet

        //Subscribe til handleren
        public DecodingWithEvent(ITransponderReceiver rawReceiver)
        {
            rawReceiver.TransponderDataReady += OnRawData;

            trackList = new List<ITracks>();
        }

        public void OnRawData(object o, RawTransponderDataEventArgs args)
        {
            trackList.Clear();
            //Deep copy
            foreach (var data in args.TransponderData)
            {
                trackList.Add(Convert(data));
            }

            //Hvis tracklisten har 1 track, skal nok laves om til 2 tracks
            if (trackList.Count != 0)
            {
                var handler = TrackDataReady;
                //Hvis at handler eventet har hævet flaget
                handler?.Invoke(this, new TrackDataEventArgs(trackList));
            }
        }

        public TrackData Convert(string data)
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
