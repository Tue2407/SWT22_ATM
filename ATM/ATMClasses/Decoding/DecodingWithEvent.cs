using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Data;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;
using TransponderReceiver;

namespace ATMClasses.Decoding
{
    public class DecodingWithEvent : ITrackDecoding
    {
        private List<ITracks> trackList;
        //public event EventHandler<TrackDataEventArgs> TrackDataReady; //Eventet
        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation; //Eventet til to

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
                
                var handler = TrackDataReadyForCalculation;
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
            track.FormattedTimestamp = FormatTimestamp(words[4]); 
            track.Timestamp = DateTime.ParseExact(words[4], "yyyyMMddHHmmssfff",
                System.Globalization.CultureInfo.InvariantCulture);
            track.Course = 0;
            track.Velocity = 0;

            return track;
        }
        public string FormatTimestamp(string timestamp)
        {
            string format = "yyyyMMddHHmmssfff";    //Format
            DateTime date = DateTime.ParseExact(timestamp, format, CultureInfo.CreateSpecificCulture("en-US"));
            string dateformat = String.Format(new CultureInfo("en-US"), "{0:MMMM d}{1}{0:, yyyy, 'at' HH:mm:ss 'and' fff 'miliseconds'}", date, GetDaySuffix(date));   //Format date correctly

            return dateformat;
        }
        public string GetDaySuffix(DateTime timeStamp)
        {
            //returns "st", "nd", "rd" or "th"
            return (timeStamp.Day % 10 == 1 && timeStamp.Day != 11) ? "st"
                : (timeStamp.Day % 10 == 2 && timeStamp.Day != 12) ? "nd"
                : (timeStamp.Day % 10 == 3 && timeStamp.Day != 13) ? "rd"
                : "th";
        }
    }
}
