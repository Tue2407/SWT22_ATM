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
        private double timespan;
        public ILog Logger { get; set; }
        public ISeparation Separation { get; set; }
        public ICalcVelocity Velocity { get; set; }
        public ICalcCourse Course { get; set; }
        public ICalcDistance Distance { get; set; }
        public List<ITracks> OldTracklist { get; set; }
        public List<ITracks> CurrentList { get; set; }
        public List<ITracks> CompareList { get; set; }

        private int count = 0;
        
        public Update(ITrackDecoding arg)
        {
            CompareList = new List<ITracks>();
            CurrentList = new List<ITracks>();
            OldTracklist = new List<ITracks>();
            arg.TrackDataReadyForCalculation += ArgOnTrackDataReadyForCalculation;
        }

        //Vores update ligger her
        private void ArgOnTrackDataReadyForCalculation(object sender, TrackDataEventArgs trackDataEventArgs)
        {
            //Kan gøres til en metode
            if (OldTracklist.Count != 0)
            {
                foreach (var track in trackDataEventArgs.TrackData)
                {
                    foreach (var oldtrack in OldTracklist)
                    {

                        if (track.Tag == oldtrack.Tag)
                        {
                            //Hastighed
                            track.Velocity = Convert.ToInt32(Velocity.Velocity(oldtrack, track));

                            //Kursen
                            track.Course = Convert.ToInt32(Course.Calculate(oldtrack, track));
                            count++;
                            ////CurrentList.Add(track);
                            Console.WriteLine($"ArgOnTrackDataReadyForCalculation: {track.Tag}");
                            Console.WriteLine($"ArgOnTrackDataReadyForCalculation: X1: {oldtrack.X}, X2: {track.X}, Y1: {oldtrack.Y}, Y2: {track.Y}, Vel: {track.Velocity}, {track.Course}");
                        }

                        if (track.Tag != oldtrack.Tag)
                        {
                            if (Separation.CollisionDetection(Distance, track, oldtrack))
                            {
                                Logger.LogSeparationEvent(track, oldtrack);
                            }
                        }
                        
                    }
                    ListRenew(trackDataEventArgs.TrackData);

                    //OldTracklist.Clear();

                    //foreach (var newtrack in trackDataEventArgs.TrackData)
                    //{
                    //    OldTracklist.Add(newtrack);
                    //}
                    
                }
            }
            else
            {
                ListRenew(trackDataEventArgs.TrackData);
                //foreach (var track in trackDataEventArgs.TrackData)
                //{
                //    OldTracklist.Clear();
                //    OldTracklist.Add(track);
                //}
            }

        }
            

        //Skal initialisere Calc udefra plus tilføje alt til listen!
        public void TrackCalculated(ICalcDistance distance ,ICalcCourse course, ICalcVelocity vel, ILog logger, ISeparation separation, List<ITracks> list)
        {
            //Initialisering af klasserne
            Distance = distance;
            Velocity = vel;
            Course = course;
            Logger = logger;
            Separation = separation;
        }
        
        public void ListRenew(List<ITracks> list)
        {
            OldTracklist.Clear();
            foreach (var Hans in list)
            {
                OldTracklist.Add(Hans);
            }
        }

        public bool TrackMatchingTag(List<ITracks> list)
        {
            return true;
        }

        public void TrackVelocity(List<ITracks> list)
        {

        }

        public event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}