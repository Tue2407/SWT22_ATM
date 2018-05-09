using System;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;

namespace ATMClasses.TrackUpdate
{
    public class Update : IUpdate
    {
         
        public Update(ITrackDecoding arg)
        {
            arg.TrackDataReadyForCalculation += ArgOnTrackDataReadyForCalculation;
        }

        private void ArgOnTrackDataReadyForCalculation(object sender, TrackDataEventArgs trackDataEventArgs)
        {
            Console.WriteLine("Two stuff is here now");
        }


        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}