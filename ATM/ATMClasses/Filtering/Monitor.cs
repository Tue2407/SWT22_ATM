using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Interfaces;

namespace ATMClasses.Filtering
{
    public class Monitor : IMonitors
    {
        public bool InView { get; set; }
        public Monitor(ITracks track)
        {
            InView = false;
            InView = MonitorFlight(track);
        }

        public bool MonitorFlight(ITracks track)
        {
            //De rette koords
            return track.X <= 90000
                   && track.X >= 10000
                   && track.Y <= 90000
                   && track.Y >= 10000
                   && track.Altitude >= 500
                   && track.Altitude <= 20000;
        }
    }
}