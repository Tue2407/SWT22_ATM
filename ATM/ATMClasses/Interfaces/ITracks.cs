namespace ATMClasses.Interfaces
{
    public interface ITracks
    {
        string Tag { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Altitude { get; set; }
        int Velocity { get; set; }
        int Course { get; set; }
    }
}