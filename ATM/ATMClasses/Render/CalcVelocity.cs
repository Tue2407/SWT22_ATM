using System;
using ATMClasses.Interfaces;

namespace ATMClasses.Render
{
    public class CalcVelocity : ICalcVelocity
    {
        public double timespan { get; set; }
        public double Velocity(ITracks track1, ITracks track2)
        {
            TimeSpan time = track2.Timestamp - track1.Timestamp;
            timespan = (double)time.TotalSeconds;

            double a = 0, b = 0, speed = 0;

            if (track1.X > track2.X)
            {
                a = track1.X - track2.X;
            }
            else
            {
                a = track2.X - track1.X;
            }

            if (track1.Y > track2.Y)
            {
                b = track1.Y - track2.Y;
            }
            else
            {
                b = track2.Y - track1.Y;
            }

            double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

            if (timespan < 0)
            {
                timespan = timespan * -1;
                speed = c / timespan;
            }
            else if (timespan > 0)
            { speed = c / timespan; }

            return speed;
        }
    }
}