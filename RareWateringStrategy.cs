namespace PlantGuardian.Strategies
{
    public class RareWateringStrategy : IWateringStrategy
    {
        public WateringStrategyType GetWateringFrequency()
        {
            return WateringStrategyType.Rare;
        }
    }
}
