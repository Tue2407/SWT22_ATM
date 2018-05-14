using System;
using System.Collections.Generic;

namespace ATMClasses.Interfaces
{
    public interface IUpdate
    {
        IMonitors Monitor { get; set; }
       
        ILog Logger { get; set; }
        ISeparation Separation { get; set; }
        ICalcVelocity Velocity { get; set; }
        ICalcCourse Course { get; set; }
        ICalcDistance Distance { get; set; }

        List<ITracks> OldTracklist { get; }

        void UpdatesTrack(List<ITracks> list);
        void ListRenew(List<ITracks> list);
        void ListInit(List<ITracks> list);

        void TrackCalculated(IMonitors monitor, ICalcDistance distance, ICalcCourse course, ICalcVelocity vel,
            ILog logger, ISeparation separation, List<ITracks> list);
        event EventHandler<TrackDataEventArgs> TrackDataReadyForCalculation;
    }
}