using BierAlyzer.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BierAlyzer.Api.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Id = 1, Username = "admin", Password = "admin", DisplayName = "Administrator" },
            new User { Id = 2, Username = "user", Password = "user", DisplayName = "User" }
        };

        public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(_users.AsEnumerable());
        public Task<User?> GetByIdAsync(int id) => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        public Task<User?> GetByUsernameAsync(string username) => Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        public Task<User> AddAsync(User user)
        {
            user.Id = _users.Max(u => u.Id) + 1;
            _users.Add(user);
            return Task.FromResult(user);
        }
    }
}
