namespace ATMClasses.Interfaces
{
    public interface ISeparation
    {
       bool CollisionDetection(ITracks track1, ITracks track2);
    }
}