using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game_Library.Controllers
{
    public class PlatformController : Controller
    {
        private readonly GameLibraryDbContext context;

        public PlatformController(GameLibraryDbContext Context)
        {
            this.context = Context;
        }

        public async Task<IActionResult> Index()
        {
            var platforms = await context.Platforms
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(platforms);
        }

        public async Task<IActionResult> Details(int id)
        {
            var platform = await context.Platforms
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlatformId == id);

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

            bool exists = await context.Platforms
                .AnyAsync(p => p.Name == platform.Name);

            if (exists)
            {
                ModelState.AddModelError(
                    nameof(Platform.Name),
                    "A platform with this name already exists.");

                return View(platform);
            }

            context.Platforms.Add(platform);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var platform = await context.Platforms.FindAsync(id);

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

            var platform = await context.Platforms.FindAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            bool exists = await context.Platforms
                .AnyAsync(p => p.Name == model.Name
                            && p.PlatformId != id);

            if (exists)
            {
                ModelState.AddModelError(
                    nameof(Platform.Name),
                    "A platform with this name already exists.");

                return View(model);
            }

            platform.Name = model.Name;
            platform.Manufacturer = model.Manufacturer;
            platform.ReleaseYear = model.ReleaseYear;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlatformId == id);

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
            var platform = await context.Platforms.FindAsync(id);

            if (platform == null)
            {
                return NotFound();
            }

            context.Platforms.Remove(platform);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
