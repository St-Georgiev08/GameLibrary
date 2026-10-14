
using Game_Library.ViewModels.Game;
using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Game_Library.Controllers
{
    public class GameController : Controller
    {
        private readonly GameLibraryDbContext context;

        public GameController(GameLibraryDbContext context)
        {
            this.context = context;
        }

        // ==============================
        // HELPER METHODS
        // ==============================

        private async Task<List<SelectListItem>>
            GetAvailablePlatformsAsync()
        {
            return await context.Platforms
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new SelectListItem
                {
                    Value = p.PlatformId.ToString(),
                    Text = p.Name
                })
                .ToListAsync();
        }

        private async Task LoadDevelopersAsync(
            int? selectedId = null)
        {
            var developers = await context.Developers
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .OrderBy(d => d.DeveloperName)
                .ToListAsync();

            ViewBag.Developers = new SelectList(
                developers,
                "Developer_id",
                "DeveloperName",
                selectedId
            );
        }

        private async Task ValidateRelationshipsAsync(
            int developerId,
            ICollection<int> selectedPlatformIds)
        {
            bool developerExists = await context.Developers
                .AnyAsync(d =>
                    d.Developer_id == developerId &&
                    !d.IsDeleted);

            if (!developerExists)
            {
                ModelState.AddModelError(
                    "DeveloperId",
                    "Please select a valid developer.");
            }

            var selectedIds = selectedPlatformIds
                .Distinct()
                .ToList();

            int validPlatformCount = await context.Platforms
                .CountAsync(p =>
                    selectedIds.Contains(p.PlatformId));

            if (validPlatformCount != selectedIds.Count)
            {
                ModelState.AddModelError(
                    "SelectedPlatformIds",
                    "One or more selected platforms are invalid.");
            }
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await context.Games
                .AsNoTracking()
                .OrderBy(g => g.Title)
                .Select(g => new GameIndexViewModel
                {
                    GameId = g.Game_id,
                    Title = g.Title,
                    ReleaseYear = g.ReleaseYear,
                    Genre = g.Genre,
                    Price = g.Price,
                    Rating = g.Reating
                })
                .ToListAsync();

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .Include(g => g.Developer)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Game_id == id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameDetailsViewModel
            {
                GameId = game.Game_id,
                Title = game.Title,
                ReleaseYear = game.ReleaseYear,
                Genre = game.Genre,
                Price = game.Price,
                Rating = game.Reating
            };

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new GameCreateViewModel
            {
                AvailablePlatforms =
                    await GetAvailablePlatformsAsync()
            };

            await LoadDevelopersAsync();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            GameCreateViewModel model)
        {
            await ValidateRelationshipsAsync(
                model.DeveloperId,
                model.SelectedPlatformIds);

            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    Title = model.Title,
                    ReleaseYear = model.ReleaseYear,
                    Genre = model.Genre,
                    Price = model.Price,
                    Reating = model.Rating,
                    DeveloperId = model.DeveloperId
                };

                foreach (var platformId in
                    model.SelectedPlatformIds.Distinct())
                {
                    game.GamePlatforms.Add(
                        new GamePlatform
                        {
                            PlatformId = platformId
                        });
                }

                context.Games.Add(game);

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.AvailablePlatforms =
                await GetAvailablePlatformsAsync();

            await LoadDevelopersAsync(model.DeveloperId);

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g => g.Game_id == id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameEditViewModel
            {
                Id = game.Game_id,
                Title = game.Title,
                ReleaseYear = game.ReleaseYear,
                Genre = game.Genre,
                Price = game.Price,
                Rating = game.Reating,
                DeveloperId = game.DeveloperId,

                SelectedPlatformIds = game.GamePlatforms
                    .Select(gp => gp.PlatformId)
                    .ToList(),

                AvailablePlatforms =
                    await GetAvailablePlatformsAsync()
            };

            await LoadDevelopersAsync(model.DeveloperId);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            GameEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            await ValidateRelationshipsAsync(
                model.DeveloperId,
                model.SelectedPlatformIds);

            if (ModelState.IsValid)
            {
                var game = await context.Games
                    .Include(g => g.GamePlatforms)
                    .FirstOrDefaultAsync(g => g.Game_id == id);

                if (game == null)
                {
                    return NotFound();
                }

                // Update game information
                game.Title = model.Title;
                game.ReleaseYear = model.ReleaseYear;
                game.Genre = model.Genre;
                game.Price = model.Price;
                game.Reating = model.Rating;
                game.DeveloperId = model.DeveloperId;

                // Selected platform IDs
                var selectedIds = model.SelectedPlatformIds
                    .Distinct()
                    .ToHashSet();

                // Remove unchecked platforms
                var toRemove = game.GamePlatforms
                    .Where(gp =>
                        !selectedIds.Contains(gp.PlatformId))
                    .ToList();

                context.GamePlatforms.RemoveRange(toRemove);

                // Get existing platform IDs
                var existingIds = game.GamePlatforms
                    .Select(gp => gp.PlatformId)
                    .ToHashSet();

                // Add newly selected platforms
                foreach (var platformId in selectedIds)
                {
                    if (!existingIds.Contains(platformId))
                    {
                        game.GamePlatforms.Add(
                            new GamePlatform
                            {
                                PlatformId = platformId
                            });
                    }
                }

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.AvailablePlatforms =
                await GetAvailablePlatformsAsync();

            await LoadDevelopersAsync(model.DeveloperId);

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Game_id == id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameDeleteViewModel
            {
                GameId = game.Game_id,
                Title = game.Title
            };

            return View(model);
        }



        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await context.Games
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g => g.Game_id == id);

            if (game == null)
            {
                return NotFound();
            }

            // Remove junction table records first
            context.GamePlatforms.RemoveRange(
                game.GamePlatforms);

            context.Games.Remove(game);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
