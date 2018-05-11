using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;

namespace ATMClasses.TrackUpdate
{
    public class Update : IUpdate
    {
        public List<ITracks> Tracklist { get; set; }
        public Update(ITrackDecoding arg)
        {
            arg.TrackDataReadyForCalculation += ArgOnTrackDataReadyForCalculation;
        }

        private void ArgOnTrackDataReadyForCalculation(object sender, TrackDataEventArgs trackDataEventArgs)
        {
            Tracklist = new List<ITracks>();
            Tracklist = trackDataEventArgs.TrackData;
            foreach (var track in Tracklist)
            {
                Console.WriteLine($"ArgOnTrackDataReadyForCalculation: {track.Tag}");
            }
            
        }


        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}