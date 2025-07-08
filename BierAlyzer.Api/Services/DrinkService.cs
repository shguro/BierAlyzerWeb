using AutoMapper;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Models;
using BierAlyzer.Api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly IDrinkRepository _repo;
        private readonly IMapper _mapper;
        public DrinkService(IDrinkRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DrinkDto>> GetAllAsync()
        {
            var drinks = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<DrinkDto>>(drinks);
        }
        public async Task<DrinkDto?> GetByIdAsync(int id)
        {
            var drink = await _repo.GetByIdAsync(id);
            return drink == null ? null : _mapper.Map<DrinkDto>(drink);
        }
        public async Task<DrinkDto> AddAsync(DrinkDto drinkDto)
        {
            var drink = _mapper.Map<Drink>(drinkDto);
            var added = await _repo.AddAsync(drink);
            return _mapper.Map<DrinkDto>(added);
        }
        public async Task<bool> UpdateAsync(DrinkDto drinkDto)
        {
            var drink = _mapper.Map<Drink>(drinkDto);
            return await _repo.UpdateAsync(drink);
        }
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
