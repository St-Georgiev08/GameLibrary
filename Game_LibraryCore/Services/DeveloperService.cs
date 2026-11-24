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
                .FirstOrDefaultAsync(d => d.Developer_id == id && !d.IsDeleted);

            if (developer == null)
            {
                return false;
            }

            var hasGames = await context.Entry(developer)
                .Collection(d => d.Games)
                .Query()
                .AnyAsync();

            if (hasGames)
            {
                return false;
            }

            developer.IsDeleted = true;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Developer>> GetAllFilteredAsync(
    string? search, string? country, int? foundedYear, string? sortOrder)
        {
            var query = context.Developers
                .AsNoTracking()
                .Where(d => !d.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var name = search.Trim();
                query = query.Where(d => d.DeveloperName.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                var countryName = country.Trim();
                query = query.Where(d => d.Country.Contains(countryName));
            }

            if (foundedYear.HasValue)
            {
                query = query.Where(d => d.FoundedYear == foundedYear.Value);
            }

            query = sortOrder switch
            {
                "name-desc" => query.OrderByDescending(d => d.DeveloperName),
                "foundedYear" => query.OrderBy(d => d.FoundedYear)
                                      .ThenBy(d => d.DeveloperName),
                _ => query.OrderBy(d => d.DeveloperName)
            };

            return await query.ToListAsync();
        }
    }
}
