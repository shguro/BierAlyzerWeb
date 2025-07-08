using BierAlyzer.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Repositories
{
    public interface IDrinkRepository
    {
        Task<IEnumerable<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(int id);
        Task<Drink> AddAsync(Drink drink);
        Task<bool> UpdateAsync(Drink drink);
        Task<bool> DeleteAsync(int id);
    }
}
