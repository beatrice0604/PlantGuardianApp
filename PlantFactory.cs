using PlantGuardian.Models;
using PlantGuardian.Strategies;

namespace PlantGuardian.Factories
{
    public static class PlantFactory
    {
        public static Plant CreatePlant(string name, string type, DateTime lastWatered,
            bool needsLight, int preferredHumidity, string owner)
        {
            var plant = new Plant
            {
                Name = name,
                Type = type,
                LastWatered = lastWatered,
                NeedsLight = needsLight,
                PreferredHumidity = preferredHumidity,
                Owner = owner
            };

            plant.WateringStrategy = GetStrategyForType(type);
            plant.ApplyWateringStrategy();

            return plant;
        }

        private static IWateringStrategy GetStrategyForType(string type)
        {
            return type.ToLower() switch
            {
                "orchid" => new WeeklyWateringStrategy(),
                "cactus" => new RareWateringStrategy(),
                "daisy" => new DailyWateringStrategy(),
                _ => new WeeklyWateringStrategy()
            };
        }
    }
}
