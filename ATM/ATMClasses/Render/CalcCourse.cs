using System;
using ATMClasses.Interfaces;

namespace ATMClasses.Render
{
    public class CalcCourse : ICalcCourse
    {
        public double _Angle { get; set; }
        public double _dx { get; set; }
        public double _dy { get; set; }


        public double Calculate(ITracks track1, ITracks track2)
        {
            double Rad2Deg = 180.0 / Math.PI;
            double dx = track2.X - track1.X;
            double dy = track2.Y - track1.Y;
            double angle = 90 - Math.Atan2(dy, dx) * Rad2Deg;

            if (angle <= 0)
            {
                angle = angle + 360;
            }

            _Angle = angle;
            return _Angle;
        }
    }
}