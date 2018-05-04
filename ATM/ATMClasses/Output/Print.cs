using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;

namespace ATMClasses.Output
{
    public class Print : IPrints
    {
        public Print(List<TrackData> tracks)
        {
            Printing(tracks);
        }
        public void Printing(List<TrackData> tracks)
        {
            foreach (var track in tracks)
            {
                //Tilsat filtering
                IOutput output = new Output();
                IMonitors monitor = new Monitor(track);
                if (monitor.InView == true)
                {
                    output.OutputLine($"Tag: {track.Tag}");
                    output.OutputLine($"XCoord: {track.X}");
                    output.OutputLine($"YCoord: {track.Y}");
                    output.OutputLine($"Altitude: {track.Altitude}");
                    output.OutputLine($"Velocity: {track.Velocity}");
                    output.OutputLine($"Course: {track.Course}");
                    //System.Console.WriteLine(track);
                }
                else
                {

                }
            }
        }
    }
}