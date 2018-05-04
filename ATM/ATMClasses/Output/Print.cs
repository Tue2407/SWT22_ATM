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
        public Print(IOutput output,List<TrackData> tracks)
        {
            _myOutput = output;
            Printing(tracks);
        }
        public void Printing(List<TrackData> tracks)
        {
            foreach (var track in tracks)
            {
                //Tilsat filtering

                IMonitors monitor = new Monitor(track);
                if (monitor.InView == true)
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