
using Game_Library.ViewModels.Platform;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var platforms = await platformService.GetAllAsync();

            var model = platforms.Select(platform => new IndexPlatformViewModel
            {
                PlatformId = platform.PlatformId,
                Name = platform.Name,
                Manufacturer = platform.Manufacturer,
                ReleaseYear = platform.ReleaseYear,
                GamesCount = platform.GamePlatforms.Count
            }).ToList();

            return View(model);
        }

       [HttpGet]
public async Task<IActionResult> Details(int id)
{
    var platform = await platformService.GetDetailsAsync(id);

    if (platform == null)
    {
        return NotFound();
    }

    var model = new DetailsPlatformViewModel
    {
        PlatformId = platform.PlatformId,
        Name = platform.Name,
        Manufacturer = platform.Manufacturer,
        ReleaseYear = platform.ReleaseYear,
        Games = platform.GamePlatforms
            .Select(link => new DetailsPlatformViewModel.PlatformGameViewModel
            {
                GameId = link.Game.Game_id,
                Title = link.Game.Title,
                ReleaseYear = link.Game.ReleaseYear
            })
            .ToList()
    };

    return View(model);
}

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePlatformViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePlatformViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await platformService.ExistsAsync(model.Name))
            {
                ModelState.AddModelError(
                    nameof(CreatePlatformViewModel.Name),
                    "A platform with this name already exists.");

                return View(model);
            }

            var platform = new Platform
            {
                Name = model.Name,
                Manufacturer = model.Manufacturer,
                ReleaseYear = model.ReleaseYear
            };

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

            var model = new EditPlatformViewModel
            {
                PlatformId = platform.PlatformId,
                Name = platform.Name,
                Manufacturer = platform.Manufacturer,
                ReleaseYear = platform.ReleaseYear
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPlatformViewModel model)
        {
            if (id != model.PlatformId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var platform = await platformService.GetByIdAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            if (await platformService.ExistsAsync(model.Name, id))
            {
                ModelState.AddModelError(
                    nameof(EditPlatformViewModel.Name),
                    "A platform with this name already exists.");

                return View(model);
            }

            var updatedPlatform = new Platform
            {
                PlatformId = id,
                Name = model.Name,
                Manufacturer = model.Manufacturer,
                ReleaseYear = model.ReleaseYear
            };

            if (!await platformService.UpdateAsync(updatedPlatform))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var platform = await platformService.GetDetailsAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            var model = new DeletePlatformViewModel
            {
                PlatformId = platform.PlatformId,
                Name = platform.Name,
                Manufacturer = platform.Manufacturer,
                ReleaseYear = platform.ReleaseYear,
                GamesCount = platform.GamePlatforms.Count
            };

            return View(model);
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
