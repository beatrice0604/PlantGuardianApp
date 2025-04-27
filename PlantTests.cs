using PlantGuardian.Models;
using PlantGuardian.Strategies;

namespace PlantGuardianTests.Tests
{
    public class PlantTests
    {
        [Fact]
        public void CheckWateringNeed_ReturnsTrue_WhenDaysExceeded()
        {
            var newDate = DateTime.UtcNow.AddDays(7);
            // Simulation of time passing - plant was watered seven days ago and we want to check if the plant should be watered today
            // based on plant's watering strategy
            var plant = new Plant
            {
                Name = "Orhidee de test",
                Type = "Orchid",
                WateringFrequencyDays = (int)WateringStrategyType.Weekly,
                LastWatered = DateTime.UtcNow

            };

            var result = plant.CheckWateringNeed(newDate);    

            Assert.True(result);    
        }

        [Fact]

        public void NotifyObserver_SendsTheRightMessage()
        {
            var observer = new UserObserver("user_test@gmail.com");
            var plant = new Plant
            {
                Name = "Cactus de test",
                Type = "Cactus",
                Owner = observer.GetUserEmail()
            };

            plant.NotifyObserver(observer);

            Assert.Equal($"Alert for user_test@gmail.com: Cactus de test needs watering!", observer.AlertMessage);
        }
    }
}
