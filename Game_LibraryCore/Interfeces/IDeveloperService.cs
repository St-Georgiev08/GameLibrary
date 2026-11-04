using GameLibrary_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Library_Core.Interfeces
{
    public interface IDeveloperService
    {
        Task<List<Developer>> GetAllAsync();

        Task<Developer?> GetByIdAsync(int id);

        Task CreateAsync(Developer developer);

        Task<bool> UpdateAsync(Developer developer);

        Task<bool> DeleteAsync(int id);
    }
}
