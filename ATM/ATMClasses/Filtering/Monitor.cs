using System;
using System.Collections.Generic;
using ATMClasses.Data;
using ATMClasses.Interfaces;

namespace ATMClasses.Filtering
{
    public class Monitor : IMonitors
    {
        public List<ITracks> Track { get; set; }
        //public bool InView { get; set; }
        //public Monitor()
        //{
        //    InView = false;
        //    //InView = MonitorFlight(Track);
        //}

        public bool MonitorFlight(ITracks track)
        {
            //De rette coords
            return track.X <= 90000
                   && track.X >= 10000
                   && track.Y <= 90000
                   && track.Y >= 10000
                   && track.Altitude >= 500
                   && track.Altitude <= 20000;
        }
    }
}