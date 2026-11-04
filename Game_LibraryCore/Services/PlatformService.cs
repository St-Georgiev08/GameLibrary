
using Game_Library_Core.Interfeces;
using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary_Core.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly GameLibraryDbContext context;

        public PlatformService(GameLibraryDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Platform>> GetAllAsync()
        {
            return await context.Platforms
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Platform?> GetByIdAsync(int id)
        {
            return await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlatformId == id);
        }

        public async Task<Platform?> GetDetailsAsync(int id)
        {
            return await context.Platforms
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlatformId == id);
        }

        public async Task<bool> ExistsAsync(
            string name, int? excludeId = null)
        {
            return await context.Platforms
                .AnyAsync(p =>
                    p.Name == name &&
                    (!excludeId.HasValue ||
                     p.PlatformId != excludeId.Value));
        }

        public async Task CreateAsync(Platform platform)
        {
            if (await ExistsAsync(platform.Name))
            {
                throw new InvalidOperationException(
                    "A platform with this name already exists.");
            }

            context.Platforms.Add(platform);

            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Platform model)
        {
            var platform = await context.Platforms
                .FirstOrDefaultAsync(p =>
                    p.PlatformId == model.PlatformId);

            if (platform == null)
            {
                return false;
            }

            if (await ExistsAsync(model.Name, model.PlatformId))
            {
                throw new InvalidOperationException(
                    "A platform with this name already exists.");
            }

            platform.Name = model.Name;
            platform.Manufacturer = model.Manufacturer;
            platform.ReleaseYear = model.ReleaseYear;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var platform = await context.Platforms
                .FirstOrDefaultAsync(p => p.PlatformId == id);

            if (platform == null)
            {
                return false;
            }

            context.Platforms.Remove(platform);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
