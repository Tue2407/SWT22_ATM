using System;
using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        ICalcVelocity Velocity { get; set; }
        ICalcCourse Course { get; set; }
        void TrackCalculated(ICalcCourse course, ICalcVelocity calc, List<ITracks> list);
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}