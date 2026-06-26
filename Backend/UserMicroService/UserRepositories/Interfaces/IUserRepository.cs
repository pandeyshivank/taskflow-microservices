
using UserMicroService.Entities;

namespace UserMicroService.UserRepositories.Interfaces
   
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(User user);
        public Task<User?> UpdateUserAsync(User user);
        public Task<bool?> DeleteUserAsync(Guid id);

        public Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);

      


    }
}
