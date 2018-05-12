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

        public ICalculation Calculator { get; set; }

        private IUpdate _Update;
        //public Print(IOutput output)
        //{
        //    _myOutput = output;
        //}
        public Print(IUpdate update , ICalculation calculator ,IMonitors monitor, IOutput output,List<ITracks> tracks)
        {
            _Update = update;
            Calculator = calculator;
            _myOutput = output;
            Printing(tracks, monitor);
        }
        public void Printing(List<ITracks> tracks, IMonitors monitor)
        {
            monitor.Track = tracks;
            _Update.TrackCalculated(Calculator, tracks);
            foreach (var track in monitor.Track)
            {
                //_Update.TrackCalculated(Calculator, tracks);
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
                else
                {

                }
            }
        }
    }
}