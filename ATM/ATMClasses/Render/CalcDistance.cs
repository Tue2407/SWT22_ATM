using System;
using ATMClasses.Interfaces;

namespace ATMClasses.Render
{
    public class CalcDistance
    {
        public class CalculateDistance : ICalcDistance
        {
            public double CalculateDistance1D(double x1, double x2)
            {
                return Math.Abs(x1 - x2);
            }

            public double CalculateDistance2D(double x1, double x2, double y1, double y2)
            {
                double xDist = CalculateDistance1D(x1, x2);
                double yDist = CalculateDistance1D(y1, y2);

                return Math.Sqrt((xDist * xDist) + (yDist * yDist));
            }
        }
    }
}