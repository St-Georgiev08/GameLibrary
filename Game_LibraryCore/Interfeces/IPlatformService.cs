using GameLibrary_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Library_Core.Interfeces
{
    public interface IPlatformService
    {
        Task<List<Platform>> GetAllAsync();

        Task<Platform?> GetByIdAsync(int id);

        Task<Platform?> GetDetailsAsync(int id);

        Task<bool> ExistsAsync(string name, int? excludeId = null);

        Task CreateAsync(Platform platform);

        Task<bool> UpdateAsync(Platform platform);

        Task<bool> DeleteAsync(int id);
    }
}
