
using Game_Library_Core.Interfeces;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Game_Library.Controllers
{
    public class PlatformController : Controller
    {
        private readonly IPlatformService platformService;

        public PlatformController(IPlatformService platformService)
        {
            this.platformService = platformService;
        }

        public async Task<IActionResult> Index()
        {
            var platforms = await platformService.GetAllAsync();

            return View(platforms);
        }

        public async Task<IActionResult> Details(int id)
        {
            var platform = await platformService.GetDetailsAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            return View(platform);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Platform platform)
        {
            ModelState.Remove(nameof(Platform.GamePlatforms));

            if (!ModelState.IsValid)
            {
                return View(platform);
            }

            bool exists = await platformService.ExistsAsync(platform.Name);

            if (exists)
            {
                ModelState.AddModelError(
                    nameof(Platform.Name),
                    "A platform with this name already exists.");

                return View(platform);
            }

            await platformService.CreateAsync(platform);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var platform = await platformService.GetByIdAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            return View(platform);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Platform model)
        {
            if (id != model.PlatformId)
            {
                return BadRequest();
            }

            ModelState.Remove(nameof(Platform.GamePlatforms));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var platform = await platformService.GetByIdAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            bool exists = await platformService.ExistsAsync(
                model.Name, id);

            if (exists)
            {
                ModelState.AddModelError(
                    nameof(Platform.Name),
                    "A platform with this name already exists.");

                return View(model);
            }

            bool updated = await platformService.UpdateAsync(model);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var platform = await platformService.GetByIdAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            return View(platform);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await platformService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
