namespace PlantGuardian.Strategies
{
    public class WeeklyWateringStrategy : IWateringStrategy
    {
        public WateringStrategyType GetWateringFrequency()
        {
            return WateringStrategyType.Weekly;
        }
    }
}
