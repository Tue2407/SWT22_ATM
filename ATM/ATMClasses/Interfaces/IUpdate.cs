using System;
using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        void TrackCalculated(ICalculation calc, List<ITracks> list);
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}