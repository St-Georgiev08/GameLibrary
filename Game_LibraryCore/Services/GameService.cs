
using Game_Library_Core.Interfeces;
using GameLibrary_Data;
using GameLibrary_Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary_Core.Services
{
    public class GameService : IGameService
    {
        private readonly GameLibraryDbContext context;

        public GameService(GameLibraryDbContext context)
        {
            this.context = context;
        }

        // ==============================
        // GET METHODS
        // ==============================

        public async Task<List<Game>> GetAllAsync(int? price , string? gameSort , string? searchName)
        {
            var games = await context.Games .AsNoTracking() .OrderBy(g => g.Title) .ToListAsync();

            if(!string.IsNullOrEmpty(searchName))
            {
                games = games.Where(g => g.Title.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (price.HasValue)
            {
                games = games.Where(g => g.Price <= price.Value).ToList();
            }

            if (!string.IsNullOrEmpty(gameSort))
            {
                switch (gameSort.ToLower())
                {
                    case "title_asc":
                        games = games.OrderBy(g => g.Title).ToList();
                        break;
                    case "title_desc":
                        games = games.OrderByDescending(g => g.Title).ToList();
                        break;
                    case "price_asc":
                        games = games.OrderBy(g => g.Price).ToList();
                        break;
                        case "price_desc":
                            games = games.OrderByDescending(g => g.Price).ToList();
                        break;
                        case "reating_asc":
                            games = games.OrderBy(g => g.Reating).ToList();
                        break;
                    default:
                        games = games.OrderBy(g => g.Title).ToList();
                        break;
                }
            }

            return games;
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await context.Games
                .AsNoTracking()
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g => g.Game_id == id);
        }

        public async Task<Game?> GetDetailsAsync(int id)
        {
            return await context.Games
                .AsNoTracking()
                .Include(g => g.Developer)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Game_id == id);
        }

        // ==============================
        // DROPDOWN DATA
        // ==============================

        public async Task<List<Developer>>
            GetAvailableDevelopersAsync()
        {
            return await context.Developers
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .OrderBy(d => d.DeveloperName)
                .ToListAsync();
        }

        public async Task<List<Platform>>
            GetAvailablePlatformsAsync()
        {
            return await context.Platforms
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        // ==============================
        // VALIDATION
        // ==============================

        public async Task<bool> DeveloperExistsAsync(
            int developerId)
        {
            return await context.Developers
                .AnyAsync(d =>
                    d.Developer_id == developerId &&
                    !d.IsDeleted);
        }

        public async Task<bool> ArePlatformsValidAsync(
            IEnumerable<int>? platformIds)
        {
            var selectedIds = platformIds?
                .Distinct()
                .ToList() ?? new List<int>();

            int validCount = await context.Platforms
                .CountAsync(p =>
                    selectedIds.Contains(p.PlatformId));

            return validCount == selectedIds.Count;
        }

        // ==============================
        // CREATE
        // ==============================

        public async Task CreateAsync(
            Game game,
            IEnumerable<int>? platformIds)
        {
            var selectedIds = platformIds?
                .Distinct()
                .ToList() ?? new List<int>();

            if (!await DeveloperExistsAsync(game.DeveloperId))
            {
                throw new InvalidOperationException(
                    "The selected developer is invalid.");
            }

            if (!await ArePlatformsValidAsync(selectedIds))
            {
                throw new InvalidOperationException(
                    "One or more selected platforms are invalid.");
            }

            context.Games.Add(game);

            foreach (var platformId in selectedIds)
            {
                context.GamePlatforms.Add(
                    new GamePlatform
                    {
                        Game = game,
                        PlatformId = platformId
                    });
            }

            await context.SaveChangesAsync();
        }

        // ==============================
        // UPDATE
        // ==============================

        public async Task<bool> UpdateAsync(
            Game model,
            IEnumerable<int>? platformIds)
        {
            var game = await context.Games
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g =>
                    g.Game_id == model.Game_id);

            if (game == null)
            {
                return false;
            }

            var selectedIds = platformIds?
                .Distinct()
                .ToHashSet() ?? new HashSet<int>();

            if (!await DeveloperExistsAsync(model.DeveloperId))
            {
                throw new InvalidOperationException(
                    "The selected developer is invalid.");
            }

            if (!await ArePlatformsValidAsync(selectedIds))
            {
                throw new InvalidOperationException(
                    "One or more selected platforms are invalid.");
            }

            // Update game properties
            game.Title = model.Title;
            game.ReleaseYear = model.ReleaseYear;
            game.Genre = model.Genre;
            game.Price = model.Price;
            game.Reating = model.Reating;
            game.DeveloperId = model.DeveloperId;

            // Existing platform IDs
            var existingIds = game.GamePlatforms
                .Select(gp => gp.PlatformId)
                .ToHashSet();

            // Remove unchecked platforms
            var toRemove = game.GamePlatforms
                .Where(gp =>
                    !selectedIds.Contains(gp.PlatformId))
                .ToList();

            context.GamePlatforms.RemoveRange(toRemove);

            // Add newly selected platforms
            foreach (var platformId in selectedIds)
            {
                if (!existingIds.Contains(platformId))
                {
                    context.GamePlatforms.Add(
                        new GamePlatform
                        {
                            Game = game,
                            PlatformId = platformId
                        });
                }
            }

            await context.SaveChangesAsync();

            return true;
        }

        // ==============================
        // DELETE
        // ==============================

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await context.Games
                .Include(g => g.GamePlatforms)
                .FirstOrDefaultAsync(g => g.Game_id == id);

            if (game == null)
            {
                return false;
            }

            context.GamePlatforms.RemoveRange(
                game.GamePlatforms);

            context.Games.Remove(game);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
