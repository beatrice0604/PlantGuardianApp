using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantGuardian.Factories;
using PlantGuardian.Models;
using PlantGuardian.Services;
using System.Threading.Tasks;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace PlantGuardian.Controllers
{
    public class PlantsController : Controller
    {
        private readonly FirestoreService _firestoreService;
        public PlantsController(FirestoreService firestoreService)
        {
            _firestoreService = firestoreService;
        }

        // GET: PlantsController
        public async Task<ActionResult> Index()
        {
            var plants = await _firestoreService.GetPlantsAsync();
            return View(plants);
        }

        // GET: PlantsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlantsController/Create
        public ActionResult Create()
        {
            Plant plant = new Plant();
            ViewBag.Types = new List<String> { "Orchid", "Cactus", "Daisy" };
            return View(plant);
        }

        // POST: PlantsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Plant plant)
        {
            string owner = "beatrice@gmail.com"; 
            try
            {
                var newPlant = PlantFactory.CreatePlant(
                    plant.Name,
                    plant.Type,
                    DateTime.SpecifyKind(plant.LastWatered, DateTimeKind.Utc),
                    plant.NeedsLight,
                    plant.PreferredHumidity,
                    owner
                    );

                await _firestoreService.AddPlantAsync(newPlant);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: PlantsController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var plant = await _firestoreService.GetPlantByIdAsync(id);
            if (plant == null)
            {
                return NotFound();
            }

            return View(plant); 
        }

        // POST: PlantsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Plant updatedPlant)
        {
            try
            {
                var existingPlant = await _firestoreService.GetPlantByIdAsync(id);
                if (existingPlant == null)
                {
                    return NotFound("Plant not found");
                }
 
                var newDate = DateTime.SpecifyKind(updatedPlant.LastWatered,DateTimeKind.Utc);  

                if (existingPlant.CheckWateringNeed(newDate))
                {
                    await _firestoreService.UpdatePlantAsync(id, updatedPlant);

                    var userObserver = new UserObserver(updatedPlant.Owner);
                    updatedPlant.NotifyObserver(userObserver);
                    TempData["Notification"] = userObserver.AlertMessage;

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PlantsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlantsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
