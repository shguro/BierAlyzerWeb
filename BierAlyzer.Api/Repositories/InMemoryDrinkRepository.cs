using BierAlyzer.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Repositories
{
    public class InMemoryDrinkRepository : IDrinkRepository
    {
        private readonly List<Drink> _drinks = new()
        {
            new Drink { Id = 1, Name = "Pilsner", Abv = 5.0 },
            new Drink { Id = 2, Name = "Weizen", Abv = 5.4 }
        };

        public Task<IEnumerable<Drink>> GetAllAsync() => Task.FromResult(_drinks.AsEnumerable());
        public Task<Drink?> GetByIdAsync(int id) => Task.FromResult(_drinks.FirstOrDefault(d => d.Id == id));
        public Task<Drink> AddAsync(Drink drink)
        {
            drink.Id = _drinks.Max(d => d.Id) + 1;
            _drinks.Add(drink);
            return Task.FromResult(drink);
        }
        public Task<bool> UpdateAsync(Drink drink)
        {
            var existing = _drinks.FirstOrDefault(d => d.Id == drink.Id);
            if (existing == null) return Task.FromResult(false);
            existing.Name = drink.Name;
            existing.Abv = drink.Abv;
            return Task.FromResult(true);
        }
        public Task<bool> DeleteAsync(int id)
        {
            var drink = _drinks.FirstOrDefault(d => d.Id == id);
            if (drink == null) return Task.FromResult(false);
            _drinks.Remove(drink);
            return Task.FromResult(true);
        }
    }
}
