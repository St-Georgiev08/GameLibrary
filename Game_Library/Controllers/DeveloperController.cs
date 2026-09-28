using Game_Library.ViewModels.Developer;
using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game_Library.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly GameLibraryDbContext context;

        public DeveloperController(GameLibraryDbContext Context)
        {
            this.context = Context;
        }


        public async Task<IActionResult> Index()
        {
            var developers = await context.Developers
                .Where(h => !h.IsDeleted).ToListAsync();

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
        public async Task<IActionResult> Create(DeveloperCreateViewModel create)
        {
            if (ModelState.IsValid)//Проверява валидацията
            {
                var dev = new Developer
                {
                    DeveloperName = create.DeveloperName,
                    Country = create.Country,
                    Email = create.Email,
                    FoundedYear = create.FoundedYear,
                    IsDeleted = false
                };

                await context.Developers.AddAsync(dev);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(create);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dev = await context.Developers.FindAsync(id);

            if (dev == null)
            {
                return NotFound();
            }

            var model = new DeveloperEditViewModel
            {
                Id = id,
                DeveloperName = dev.DeveloperName,
                Country = dev.Country,
                FoundedYear = dev.FoundedYear,
                Email = dev.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DeveloperEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dev = await context.Developers.FindAsync(id);

                if (dev == null)
                {
                    return NotFound();
                }

                dev.DeveloperName = model.DeveloperName;
                dev.FoundedYear = model.FoundedYear;
                dev.Country = model.Country;
                dev.Email = model.Email;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var dev = await context.Developers.FindAsync(id);

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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dev = await context.Developers.FindAsync(id);

            if (dev != null)
            {
                dev.IsDeleted = true;

                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
                var dev = await context.Developers.FindAsync(id);
    
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
