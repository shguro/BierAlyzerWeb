using BierAlyzer.Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Services
{
    public interface IDrinkService
    {
        Task<IEnumerable<DrinkDto>> GetAllAsync();
        Task<DrinkDto?> GetByIdAsync(int id);
        Task<DrinkDto> AddAsync(DrinkDto drinkDto);
        Task<bool> UpdateAsync(DrinkDto drinkDto);
        Task<bool> DeleteAsync(int id);
    }
}
