using System;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;

namespace ATMClasses.Proximity_Detection
{
    public class SeparationEvent : ISeparation
    {
        private ITracks _track;
        private ICalcDistance dist;
        int horizontalConflict = 5000;
        int verticalConflict = 300;

        public bool CollisionDetection(ITracks track1, ITracks track2)
        {
            double horizontalDistance = dist.CalculateDistance2D(track1.X, track2.X, track1.Y, track2.Y);
            double verticalDistance = dist.CalculateDistance1D(track1.Altitude, track2.Altitude);

            return horizontalDistance < horizontalConflict && verticalDistance < verticalConflict;
        }
    }
}