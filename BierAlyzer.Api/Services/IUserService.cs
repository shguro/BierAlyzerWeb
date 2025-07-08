using BierAlyzer.Api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> AddAsync(UserDto userDto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto);
    }
}
