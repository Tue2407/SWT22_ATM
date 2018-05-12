using System;
using System.Collections.Generic;
using System.Linq;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;

namespace ATMClasses.TrackUpdate
{
    public class Update : IUpdate
    {
        public List<ITracks> OldTracklist { get; set; }
        public Update(ITrackDecoding arg)
        {
            OldTracklist = new List<ITracks>();
            arg.TrackDataReadyForCalculation += ArgOnTrackDataReadyForCalculation;
        }

        private void ArgOnTrackDataReadyForCalculation(object sender, TrackDataEventArgs trackDataEventArgs)
        {
            if (OldTracklist.Count != 0)
            {
                foreach (var track in trackDataEventArgs.TrackData)
                {
                    foreach (var oldtrack in OldTracklist)
                    {
                        if (track.Tag == oldtrack.Tag)
                        {
                            Console.WriteLine($"ArgOnTrackDataReadyForCalculation: {track.Tag}");
                            Console.WriteLine($"ArgOnTrackDataReadyForCalculation: X1: {oldtrack.X}, X2: {track.X}, Y1: {oldtrack.Y}, Y2: {track.Y}");
                            //Så skal den lave noget
                        }
                    }
                    OldTracklist.Clear();
                    OldTracklist.Add(track);
                }
            }
            else
            {
                foreach (var track in trackDataEventArgs.TrackData)
                {
                    OldTracklist.Clear();
                    OldTracklist.Add(track);
                }
            }
            
            
            //foreach (var track in OldTracklist)
            //{
            //    Console.WriteLine($"ArgOnTrackDataReadyForCalculation: {track.Tag}");
            //}
            
        }


        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}