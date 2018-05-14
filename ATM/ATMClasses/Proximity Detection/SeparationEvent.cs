using System;
using ATMClasses.Interfaces;
using ATMClasses.TrackUpdate;

namespace ATMClasses.Proximity_Detection
{
    public class SeparationEvent : ISeparation
    {
        
        public ICalcDistance Distance { get; set; }
        private int horizontalConflict = 5000;
        private int verticalConflict = 300;

        public bool CollisionDetection(ICalcDistance Distance ,ITracks track1, ITracks track2)
        {
            this.Distance = Distance;
            double horizontalDistance = this.Distance.CalculateDistance2D(track1.X, track2.X, track1.Y, track2.Y);
            double verticalDistance = this.Distance.CalculateDistance1D(track1.Altitude, track2.Altitude);

            return horizontalDistance < horizontalConflict && verticalDistance < verticalConflict;
        }
    }
}