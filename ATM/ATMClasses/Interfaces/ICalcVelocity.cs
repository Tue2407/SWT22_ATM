namespace ATMClasses.Interfaces
{
    public interface ICalcVelocity
    {
        double Velocity(ITracks track1, ITracks track2);
        double timespan { get; set; }

    }
}