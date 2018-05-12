using System;
using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        ILog Logger { get; set; }
        ISeparation Separation { get; set; }
        ICalcVelocity Velocity { get; set; }
        ICalcCourse Course { get; set; }
        ICalcDistance Distance { get; set; }

        void TrackCalculated(ICalcDistance distance, ICalcCourse course, ICalcVelocity vel, ILog logger,
            ISeparation separation, List<ITracks> list);
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}