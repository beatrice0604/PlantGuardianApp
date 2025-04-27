using Google.Cloud.Firestore;
using PlantGuardian.Strategies;

namespace PlantGuardian.Models
{
    [FirestoreData]
    public class Plant : ISubject
    {
        [FirestoreProperty]
        public string PlantId { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Type { get; set; }

        [FirestoreProperty]
        public DateTime LastWatered { get; set; }

        [FirestoreProperty]
        public int WateringFrequencyDays { get; set; }

        [FirestoreProperty]
        public bool NeedsLight { get; set; } = false;

        [FirestoreProperty]
        public int PreferredHumidity { get; set; }

        [FirestoreProperty]
        public string Owner { get; set; }

        public IWateringStrategy WateringStrategy { get; set; }

        public void ApplyWateringStrategy()
        {
            if (WateringStrategy != null)
            {
                WateringFrequencyDays = (int)WateringStrategy.GetWateringFrequency();
            }
        }

        public void NotifyObserver(IObserver observer)
        {
            if (!string.IsNullOrEmpty(Owner))
            {
                observer.Update($"{Name} needs watering!");
            }
        }

        public bool CheckWateringNeed(DateTime newDate)
        {
            if (WateringFrequencyDays != null)
            {
                var daysSinceWatered = (newDate - LastWatered).TotalDays;
                return Math.Ceiling(daysSinceWatered) >= (int)WateringFrequencyDays;
            }
            return false;
        }

    }
}
