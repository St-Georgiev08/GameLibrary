
using Game_Library.ViewModels.Game;
using Game_Library_Core.Interfeces;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Game_Library.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        // ==============================
        // HELPER METHODS
        // ==============================

        private async Task<List<SelectListItem>> GetAvailablePlatformsAsync()
        {
            var platforms = await gameService
                .GetAvailablePlatformsAsync();


            return platforms.Select(p => new SelectListItem
            {
                Value = p.PlatformId.ToString(),
                Text = p.Name
            }).ToList();
        }

        private async Task LoadDevelopersAsync(
            int? selectedId = null)
        {
            var developers = await gameService
                .GetAvailableDevelopersAsync();

            ViewBag.Developers = new SelectList(
                developers,
                "Developer_id",
                "DeveloperName",
                selectedId
            );
        }

        private async Task ValidateRelationshipsAsync(
            int developerId,
            ICollection<int>? selectedPlatformIds)
        {
            bool developerExists = await gameService
                .DeveloperExistsAsync(developerId);

            if (!developerExists)
            {
                ModelState.AddModelError(
                    "DeveloperId",
                    "Please select a valid developer.");
            }

            bool platformsValid = await gameService
                .ArePlatformsValidAsync(selectedPlatformIds);

            if (!platformsValid)
            {
                ModelState.AddModelError(
                    "SelectedPlatformIds",
                    "One or more selected platforms are invalid.");
            }
        }

        // ==============================
        // INDEX
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Index(int? price, string? gameSort, string? searchName)
        {
            var games = await gameService.GetAllAsync();


            var model = games.Select(g =>
                new GameIndexViewModel
                {
                    GameId = g.Game_id,
                    Title = g.Title,
                    ReleaseYear = g.ReleaseYear,
                    Genre = g.Genre,
                    Price = g.Price,
                    Rating = g.Reating
                }).ToList();

            var game= await gameService.GetAllAsync(price,gameSort,searchName);

            ViewBag.Price = price;
            ViewBag.GameSort = gameSort;
            ViewBag.SearchName = searchName;
            return View(model);
        }

        // ==============================
        // DETAILS
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var game = await gameService.GetDetailsAsync(id);

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
                Rating = game.Reating,
                DeveloperName = game.Developer?.DeveloperName ?? "Unknown",
                Platforms = game.GamePlatforms.Select(gp => gp.Platform.Name).ToList()
            };

            return View(model);
        }

        // ==============================
        // CREATE GET
        // ==============================

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

        // ==============================
        // CREATE POST
        // ==============================

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

                try
                {
                    await gameService.CreateAsync(
                        game,
                        model.SelectedPlatformIds);

                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(
                        string.Empty, ex.Message);
                }
            }

            model.AvailablePlatforms =
                await GetAvailablePlatformsAsync();

            await LoadDevelopersAsync(model.DeveloperId);

            return View(model);
        }

        // ==============================
        // EDIT GET
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await gameService.GetByIdAsync(id);

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

        // ==============================
        // EDIT POST
        // ==============================

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
                var game = new Game
                {
                    Game_id = model.Id,
                    Title = model.Title,
                    ReleaseYear = model.ReleaseYear,
                    Genre = model.Genre,
                    Price = model.Price,
                    Reating = model.Rating,
                    DeveloperId = model.DeveloperId
                };

                try
                {
                    bool updated = await gameService.UpdateAsync(
                        game,
                        model.SelectedPlatformIds);

                    if (!updated)
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(
                        string.Empty, ex.Message);
                }
            }

            model.AvailablePlatforms =
                await GetAvailablePlatformsAsync();

            await LoadDevelopersAsync(model.DeveloperId);

            return View(model);
        }

        // ==============================
        // DELETE GET
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await gameService.GetByIdAsync(id);

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

        // ==============================
        // DELETE POST
        // ==============================

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await gameService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
