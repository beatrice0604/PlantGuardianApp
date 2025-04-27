namespace PlantGuardian.Strategies
{
    public enum WateringStrategyType
    {
        Daily = 1,
        Weekly = 7,
        Rare = 14
    }
    public interface IWateringStrategy
    {
        WateringStrategyType GetWateringFrequency();
    }
}
