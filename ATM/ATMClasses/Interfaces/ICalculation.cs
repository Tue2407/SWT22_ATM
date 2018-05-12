namespace ATMClasses.Interfaces
{
    public interface ICalculation
    {
        double Velocity(double x1, double x2, double y1, double y2, double time);
        string Result(string result);
    }
}