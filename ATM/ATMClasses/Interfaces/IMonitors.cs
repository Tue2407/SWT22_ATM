using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IMonitors
    {
        //ITracks Track { get; set; }
        //bool InView { get; set; }
        List<ITracks> Track { get; set; }
        bool MonitorFlight(ITracks track);
    }
}