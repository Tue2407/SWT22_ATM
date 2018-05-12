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
        private List<ITracks> CurrentList;
        public ICalcVelocity Calculator { get; set; }
       
        public List<ITracks> OldTracklist { get; set; }
        public Update(ITrackDecoding arg)
        {
            CurrentList = new List<ITracks>();
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
                            //Så skal den lave noget
                            
                            double timespan = Convert.ToDouble(track.Timestamp.Second - oldtrack.Timestamp.Second);
                            track.Velocity = Convert.ToInt32(Calculator.Velocity(oldtrack.X, track.X, oldtrack.Y, track.Y, timespan));
                            
                            CurrentList.Clear();
                            CurrentList.Add(track);

                            Console.WriteLine($"ArgOnTrackDataReadyForCalculation: {track.Tag}");
                            Console.WriteLine($"ArgOnTrackDataReadyForCalculation: X1: {oldtrack.X}, X2: {track.X}, Y1: {oldtrack.Y}, Y2: {track.Y}, Timespan: {timespan}, Vel: {track.Velocity}");
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

        }

        //Skal initialisere Calc udefra plus tilføje velocity til listen!
        public void TrackCalculated(ICalcVelocity calc, List<ITracks> list)
        {
            list.Clear();
            foreach (var track in CurrentList)
            {
                 list.Add(track);
            }
            Calculator = calc;
        }
        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}