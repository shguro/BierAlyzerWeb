using BierAlyzer.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
    }
}
