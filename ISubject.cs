namespace PlantGuardian.Models
{
    public interface ISubject
    {
        void NotifyObserver(IObserver observer);
    }
}
