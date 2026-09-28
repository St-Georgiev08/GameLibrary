using Game_Library.ViewModels.Game;
using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Game_Library.Controllers
{
    public class GameController : Controller
    {
        private readonly GameLibraryDbContext context;

        public GameController(GameLibraryDbContext Context)
        {
            this.context = Context;
        }
        public async Task<IActionResult> Index()
        {
            var games = await context.Games.Include
                (d => d.Developer)
                .ToListAsync();

            var model = new List<GameIndexViewModel>();

            foreach (var gm in games)
            {
                model.Add(new GameIndexViewModel
                {
                   GameId = gm.Game_id,
                   Title = gm.Title,
                   ReleaseYear = gm.ReleaseYear,
                   Genre = gm.Genre,
                   Price = gm.Price,
                   Rating = gm.Reating
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var game = await context.Games
                .Include(d => d.Developer)
                .FirstOrDefaultAsync(d => d.DeveloperId == id);

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

        public async Task<IActionResult> Create()
        {
            ViewBag.Hospitals = 
            new SelectList(await context.Developers.Where(h => !h.IsDeleted).ToListAsync(),"Id","Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateViewModel model)
        {
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

                context.Games.Add(game);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hospitals = new SelectList(
            await context.Developers.Where(h => !h.IsDeleted).ToListAsync(),"Id","Name");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var model = new GameEditViewModel
            {
                Title = game.Title,
                ReleaseYear = game.ReleaseYear,
                Genre = game.Genre,
                Price = game.Price,
                Rating = game.Reating,
                DeveloperId = game.DeveloperId
            };

            ViewBag.Hospitals = new SelectList(
            await context.Developers.Where(h => !h.IsDeleted).ToListAsync(),"Id","Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GameEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var gm = await context.Games.FindAsync(id);

                if (gm == null)
                {
                    return NotFound();
                }
                
                gm.Title = model.Title;
                gm.Reating = model.Rating;
                gm.Genre = model.Genre;
                gm.ReleaseYear = model.ReleaseYear;
                gm.DeveloperId = model.DeveloperId;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hospitals = new SelectList(
            await context.Developers.Where(h => !h.IsDeleted).ToListAsync(),
                  "Id",
                  "Name");

            return View(model);
        }

       
    }
}
