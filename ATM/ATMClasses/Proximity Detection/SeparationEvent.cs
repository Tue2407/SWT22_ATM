using System;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;

namespace ATMClasses.Proximity_Detection
{
    public class SeparationEvent : ISeparation
    {
        
        public ICalcDistance dist { get; set; }
        int horizontalConflict = 5000;
        int verticalConflict = 300;

        public bool CollisionDetection(ICalcDistance Distance ,ITracks track1, ITracks track2)
        {
            dist = Distance;
            double horizontalDistance = dist.CalculateDistance2D(track1.X, track2.X, track1.Y, track2.Y);
            double verticalDistance = dist.CalculateDistance1D(track1.Altitude, track2.Altitude);

            return horizontalDistance < horizontalConflict && verticalDistance < verticalConflict;
        }
    }
}