namespace ATMClasses.Interfaces
{
    public interface ISeparation
    {
       bool CollisionDetection(ICalcDistance Distance, ITracks track1, ITracks track2);
       ICalcDistance Distance { get; set; }
    }
}