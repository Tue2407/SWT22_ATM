using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;

namespace ATMClasses.Output
{
    public class Print : IPrints
    {
        private IOutput _myOutput;

        public ICalcDistance CalcDist{ get; set; }
        public ICalcVelocity CalcVel { get; set; }
        public ICalcCourse CalcCourse { get; set; }
        public ILog Logger { get; set; }
        public ISeparation Separation { get; set; }
        
        private IUpdate _Update;
        //public Print(IOutput output)
        //{
        //    _myOutput = output;
        //}
        public Print(IUpdate update,ICalcDistance calcDist , ICalcCourse calcCourse, ICalcVelocity calcVel, ILog logger, ISeparation separation, IMonitors monitor, IOutput output,List<ITracks> tracks)
        {
            _Update = update;
            CalcDist = calcDist;
            CalcVel = calcVel;
            CalcCourse = calcCourse;
            Logger = logger;
            Separation = separation;
            _myOutput = output;
            Printing(tracks, monitor);
            
        }
        public void Printing(List<ITracks> tracks, IMonitors monitor)
        {
            monitor.Track = tracks;
            //Her bliver mest af updates klasser initialiseret
            if (tracks.Count == 1)
            {
                _Update.TrackCalculated(monitor,CalcDist, CalcCourse, CalcVel, Logger, Separation, tracks);   
            }

            foreach (var track in monitor.Track)
            {
                monitor.Track = tracks;
                //Tilsat filtering
                if (monitor.MonitorFlight(track))
                {
                    _myOutput.OutputLine($"Tag: {track.Tag}");
                    _myOutput.OutputLine($"XCoord: {track.X}");
                    _myOutput.OutputLine($"YCoord: {track.Y}");
                    _myOutput.OutputLine($"Altitude: {track.Altitude}");
                    _myOutput.OutputLine($"Velocity: {track.Velocity}");
                    _myOutput.OutputLine($"Course: {track.Course}");
                    _myOutput.OutputLine($"");
                    //System.Console.WriteLine(track);
                }
                //else
                {

                }
            }
        }
    }
}