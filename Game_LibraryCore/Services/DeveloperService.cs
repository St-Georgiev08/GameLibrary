using Game_Library_Core.Interfeces;
using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary_Core.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly GameLibraryDbContext context;

        public DeveloperService(GameLibraryDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Developer>> GetAllAsync()
        {
            return await context.Developers
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<Developer?> GetByIdAsync(int id)
        {
            return await context.Developers
                .AsNoTracking()
                .FirstOrDefaultAsync(d =>
                    d.Developer_id == id && !d.IsDeleted);
        }

        public async Task CreateAsync(Developer developer)
        {
            developer.IsDeleted = false;

            await context.Developers.AddAsync(developer);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Developer developer)
        {
            var existingDeveloper = await context.Developers
                .FirstOrDefaultAsync(d =>
                    d.Developer_id == developer.Developer_id
                    && !d.IsDeleted);

            if (existingDeveloper == null)
            {
                return false;
            }

            existingDeveloper.DeveloperName = developer.DeveloperName;
            existingDeveloper.Country = developer.Country;
            existingDeveloper.Email = developer.Email;
            existingDeveloper.FoundedYear = developer.FoundedYear;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var developer = await context.Developers
                .FirstOrDefaultAsync(d =>
                    d.Developer_id == id && !d.IsDeleted);

            if (developer == null)
            {
                return false;
            }

            developer.IsDeleted = true;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Developer>> GetAllFilteredAsync(string? search, string? country, string? FoundedYear)
        {
            var query = context.Games
                .Include(d => d.Developer)
                .Where(d => !d.Developer.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Developer.DeveloperName.Contains(search));
            }

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(x => x.Developer.Country.Contains(country));
            }

            switch (FoundedYear)
            {
                case "founded-asc":
                    query = query.OrderBy(x => x.Developer.FoundedYear);
                    break;
                case "founded-desc":
                    query = query.OrderByDescending(x => x.Developer.FoundedYear);
                    break;
                default:
                    query = query.OrderBy(x => x.Developer.FoundedYear);
                    break;
            }

            return await query
                .Select(x => x.Developer)
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
