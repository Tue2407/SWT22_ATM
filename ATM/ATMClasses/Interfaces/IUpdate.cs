using System;
using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        ICalcVelocity Calculator { get; set; }
        void TrackCalculated(ICalcVelocity calc, List<ITracks> list);
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}