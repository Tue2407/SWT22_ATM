using System;
using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        ICalculation Calculator { get; set; }
        void TrackCalculated(ICalculation calc, List<ITracks> list);
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}