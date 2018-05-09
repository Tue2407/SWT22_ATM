using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;

namespace ATMClasses.TrackUpdate
{
    public class Update : IUpdate
    {
        private List<ITracks> TrackData { get; } 
        public Update(ITrackDecoding arg)
        {
            arg.TrackDataReadyForCalculation += ArgOnTrackDataReadyForCalculation;
        }

        public Update(List<ITracks> trackData)
        {
            TrackData = trackData;
        }

        private void ArgOnTrackDataReadyForCalculation(object sender, TrackDataEventArgs trackDataEventArgs)
        {
            Console.WriteLine("Two stuff is here now");
        }


        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}