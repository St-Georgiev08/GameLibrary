using GameLibrary_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Library_Core.Interfeces
{
    public interface IGameService
    {
        Task<List<Game>> GetAllAsync(int? price = null, string? gameSort = null, string? searchName = null);

        Task<Game?> GetByIdAsync(int id);

        Task<Game?> GetDetailsAsync(int id);

        Task<List<Developer>> GetAvailableDevelopersAsync();

        Task<List<Platform>> GetAvailablePlatformsAsync();

        Task<bool> DeveloperExistsAsync(int developerId);

        Task<bool> ArePlatformsValidAsync(
            IEnumerable<int>? platformIds);

        Task CreateAsync(
            Game game,
            IEnumerable<int>? platformIds);

        Task<bool> UpdateAsync(
            Game model,
            IEnumerable<int>? platformIds);

        Task<bool> DeleteAsync(int id);
    }
}
