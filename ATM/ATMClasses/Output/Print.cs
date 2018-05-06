using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;

namespace ATMClasses.Output
{
    public class Print : IPrints
    {
        private IOutput _myOutput;

        //public Print(IOutput output)
        //{
        //    _myOutput = output;
        //}
        public Print(IMonitors monitor, IOutput output,List<ITracks> tracks)
        {
            _myOutput = output;
            Printing(tracks, monitor);
        }
        public void Printing(List<ITracks> tracks, IMonitors monitor)
        {
            monitor.Track = tracks;

            foreach (var track in monitor.Track)
            {
                //Tilsat filtering

                if (monitor.MonitorFlight(track))
                {
                    _myOutput.OutputLine($"Tag: {track.Tag}");
                    _myOutput.OutputLine($"XCoord: {track.X}");
                    _myOutput.OutputLine($"YCoord: {track.Y}");
                    _myOutput.OutputLine($"Altitude: {track.Altitude}");
                    _myOutput.OutputLine($"Velocity: {track.Velocity}");
                    _myOutput.OutputLine($"Course: {track.Course}");
                    //System.Console.WriteLine(track);
                }
                else
                {

                }
            }
        }
    }
}