using System;
using ATMClasses.Interfaces;

namespace ATMClasses.TrackUpdate
{
    public class CalcCourse : ICalcCourse
    {
        public double _Angle { get; set; }
        public double _x1 { get; set; }
        public double _x2 { get; set; }
        public double _y1 { get; set; }
        public double _y2 { get; set; }
        public double _dx { get; set; }
        public double _dy { get; set; }

        public CalcCourse(double x1, double x2, double y1, double y2)
        {
            _x1 = x1;
            _x2 = x2;
            _y1 = y1;
            _y2 = y2;

            _Angle = Calculate(_x1, _x2, _y1, _y2);
        }

        public double Calculate(double x1, double x2, double y1, double y2)
        {
            double Rad2Deg = 180.0 / Math.PI;
            double dx = x2 - x1;
            double dy = y2 - y1;
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