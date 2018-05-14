using System;
using System.Collections.Generic;
using System.Linq;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using ATMClasses.Proximity_Detection;
using ATMClasses.Render;

namespace ATMClasses.TrackUpdate
{
    public class Update : IUpdate
    {
        public IMonitors Monitor { get; set; }
        public ILog Logger { get; set; }
        public ISeparation Separation { get; set; }
        public ICalcVelocity Velocity { get; set; }
        public ICalcCourse Course { get; set; }
        public ICalcDistance Distance { get; set; }

        public List<ITracks> OldTracklist { get; }
        
        public Update(ITrackDecoding arg)
        {
            OldTracklist = new List<ITracks>();
            arg.TrackDataReadyForCalculation += ArgOnTrackDataReadyForCalculation;
        }

        //Vores filter og update sidder her, med separationseventet hvis tags ikke er det samme
        private void ArgOnTrackDataReadyForCalculation(object sender, TrackDataEventArgs trackDataEventArgs)
        {
            
            if (OldTracklist.Count != 0)
            {
                UpdatesTrack(trackDataEventArgs.TrackData);
            }
            else
            {
                ListInit(trackDataEventArgs.TrackData);
            }
            ListRenew(trackDataEventArgs.TrackData);
        }
            

        //Skal initialisere Calc udefra plus tilføje alt til listen!
        public void TrackCalculated(IMonitors monitor,ICalcDistance distance ,ICalcCourse course, ICalcVelocity vel, ILog logger, ISeparation separation, List<ITracks> list)
        {
            //Initialisering af klasserne
            Monitor = monitor;
            Distance = distance;
            Velocity = vel;
            Course = course;
            Logger = logger;
            Separation = separation;
        }
        
        public void ListInit(List<ITracks> list)
        {
            if(!OldTracklist.Any())
            {
                foreach (var Hans in list)
                {
                    OldTracklist.Add(Hans);
                }
            }
            
        }

        public void ListRenew(List<ITracks> list)
        {
        OldTracklist.Clear();
            foreach (var Hans in list)
            {
                OldTracklist.Add(Hans);
            } 
        }

        public void UpdatesTrack(List<ITracks> list)
        {
            foreach (var newTrack in list)
            {
                foreach (var oldtrack in OldTracklist)
                {
                    if (Monitor.MonitorFlight(oldtrack))
                    {
                        if (oldtrack.Tag == newTrack.Tag)
                        {
                            //Console.WriteLine($"{newTrack.Tag}, X1: {oldtrack.X}, X2: {newTrack.X}");
                            newTrack.Velocity = (int)Velocity.Velocity(oldtrack, newTrack);
                            newTrack.Course = (int)Course.Calculate(oldtrack, newTrack);
                            break;
                        }

                        if (oldtrack.Tag != newTrack.Tag)
                        {

                            if (Separation.CollisionDetection(Distance, newTrack, oldtrack))
                            {
                                Logger.LogSeparationEvent(newTrack,oldtrack);
                            }
                        }
                    }
                }
            }
        }


        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}