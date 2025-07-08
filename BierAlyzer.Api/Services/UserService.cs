using AutoMapper;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Models;
using BierAlyzer.Api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> AddAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var added = await _repo.AddAsync(user);
            return _mapper.Map<UserDto>(added);
        }
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _repo.GetByUsernameAsync(loginDto.Username);
            if (user == null || user.Password != loginDto.Password) return null;
            // For demo: return a fake token
            return new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                DisplayName = user.DisplayName,
                Token = "demo-token-" + user.Id
            };
        }
    }
}
