namespace PlantGuardian.Strategies
{
    public class DailyWateringStrategy : IWateringStrategy
    {
        public WateringStrategyType GetWateringFrequency()
        {
            return WateringStrategyType.Daily;
        }
    }
}
