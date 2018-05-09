using System;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}