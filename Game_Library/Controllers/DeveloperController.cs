
using Game_Library.ViewModels.Developer;
using Game_Library_Core.Interfeces;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Game_Library.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly IDeveloperService developerService;

        public DeveloperController(IDeveloperService developerService)
        {
            this.developerService = developerService;
        }

        public async Task<IActionResult> Index()
        {
            var developers = await developerService.GetAllAsync();

            var model = new List<DeveloperIndexViewModel>();

            foreach (var dev in developers)
            {
                model.Add(new DeveloperIndexViewModel
                {
                    Developer_id = dev.Developer_id,
                    DeveloperName = dev.DeveloperName,
                    Country = dev.Country,
                    Email = dev.Email,
                    FoundedYear = dev.FoundedYear
                });
            }

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DeveloperCreateViewModel create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }

            var developer = new Developer
            {
                DeveloperName = create.DeveloperName,
                Country = create.Country,
                Email = create.Email,
                FoundedYear = create.FoundedYear
            };

            await developerService.CreateAsync(developer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dev = await developerService.GetByIdAsync(id);

            if (dev == null)
            {
                return NotFound();
            }

            var model = new DeveloperEditViewModel
            {
                Id = dev.Developer_id,
                DeveloperName = dev.DeveloperName,
                Country = dev.Country,
                FoundedYear = dev.FoundedYear,
                Email = dev.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, DeveloperEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var developer = new Developer
            {
                Developer_id = model.Id,
                DeveloperName = model.DeveloperName,
                Country = model.Country,
                FoundedYear = model.FoundedYear,
                Email = model.Email
            };

            var updated = await developerService.UpdateAsync(developer);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dev = await developerService.GetByIdAsync(id);

            if (dev == null)
            {
                return NotFound();
            }

            var model = new DeveloperDeleteViewModel
            {
                Id = dev.Developer_id,
                Name = dev.DeveloperName
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await developerService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var dev = await developerService.GetByIdAsync(id);

            if (dev == null)
            {
                return NotFound();
            }

            var model = new DeveloperDetailsViewModel
            {
                DeveloperName = dev.DeveloperName,
                Country = dev.Country,
                FoundedYear = dev.FoundedYear,
                Email = dev.Email
            };

            return View(model);
        }
    }
}
