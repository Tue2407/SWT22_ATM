using System;
using System.Collections.Generic;
using System.Linq;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using ATMClasses.Proximity_Detection;

namespace ATMClasses.TrackUpdate
{
    public class Update : IUpdate
    {
       
        private List<ITracks> CurrentList;
        public ILog Logger { get; set; }
        public ISeparation Separation { get; set; }
        public ICalcVelocity Velocity { get; set; }
        public ICalcCourse Course { get; set; }
       
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
                            
                            //Hastighed
                            double timespan = Convert.ToDouble(track.Timestamp.Second - oldtrack.Timestamp.Second);
                            track.Velocity = Convert.ToInt32(Velocity.Velocity(oldtrack.X, track.X, oldtrack.Y, track.Y, timespan));

                            //Kursen
                            track.Course = Convert.ToInt32(Course.Calculate(oldtrack.X, track.X, oldtrack.Y, track.Y));

                            CurrentList.Clear();
                            CurrentList.Add(track);

                            //Console.WriteLine($"ArgOnTrackDataReadyForCalculation: {track.Tag}");
                            //Console.WriteLine($"ArgOnTrackDataReadyForCalculation: X1: {oldtrack.X}, X2: {track.X}, Y1: {oldtrack.Y}, Y2: {track.Y}, Timespan: {timespan}, Vel: {track.Velocity}, {track.Course}");
                        }
                        //Separationseventet
                        else
                        {
                            if (Separation.CollisionDetection(track,oldtrack))
                            {
                                Logger.LogSeparationEvent(track, oldtrack);
                            }
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
        public void TrackCalculated(ICalcCourse course , ICalcVelocity vel, ILog logger, ISeparation separation, List<ITracks> list)
        {
            list.Clear();
            foreach (var track in CurrentList)
            {
                 list.Add(track);
            }
            Velocity = vel;
            Course = course;
            Logger = logger;
            Separation = separation;
        }
        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}