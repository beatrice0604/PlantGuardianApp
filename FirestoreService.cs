using Google.Cloud.Firestore;
using PlantGuardian.Models;
using System.Diagnostics;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace PlantGuardian.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _db;

        public FirestoreService(IWebHostEnvironment env)
        {
            string path = Path.Combine(env.ContentRootPath, "AppData", "myplantguardian-firebase-adminsdk-fbsvc-5e65812139.json");

            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(path);
            FirestoreDbBuilder builder = new FirestoreDbBuilder
            {
                ProjectId = "myplantguardian",
                Credential = credential
            };
            _db = builder.Build();
        }
        public async Task<List<Plant>> GetPlantsAsync()
        {
            QuerySnapshot snapshot = await _db.Collection("plants").GetSnapshotAsync();
            var plantList = snapshot.Documents.Select(doc => doc.ConvertTo<Plant>()).ToList();

            return plantList;
        }

        public async Task AddPlantAsync(Plant plant)
        {
            var docRef = await _db.Collection("plants").AddAsync(plant);
            plant.PlantId = docRef.Id;
            await docRef.UpdateAsync(new Dictionary<string, object>
            {
                {"PlantId", docRef.Id},
            });
        }

        public async Task<Plant> GetPlantByIdAsync(string id)
        {
            try
            {
                DocumentSnapshot document = await _db.Collection("plants").Document(id).GetSnapshotAsync();

                if (document.Exists)
                {
                    var plant = document.ConvertTo<Plant>();
                    return plant;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting plant by id: {ex.Message}");
                return null;
            }
        }

        public async Task UpdatePlantAsync(string docId, Plant updatedPlant)
        {
            try
            {

                var plantRef = _db.Collection("plants").Document(docId);
                var updates = new Dictionary<string, object>
                {
                    {"Name" , updatedPlant.Name },
                    { "Type" , updatedPlant.Type },
                    { "LastWatered" , DateTime.SpecifyKind(updatedPlant.LastWatered, DateTimeKind.Utc) },
                    { "NeedsLight" , updatedPlant.NeedsLight },
                    { "PreferredHumidity" , updatedPlant.PreferredHumidity },
                    { "Owner" , updatedPlant.Owner },
                };

                await plantRef.UpdateAsync(updates);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating plant: {ex.Message}");
                throw;
            }
        }
    }
}
