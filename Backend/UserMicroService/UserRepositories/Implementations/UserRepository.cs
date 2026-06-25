

using Microsoft.EntityFrameworkCore;
using UserMicroService.Data;
using UserMicroService.Entities;
using UserMicroService.UserRepositories.Interfaces;

namespace UserMicroService.UserRepositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context) {
            _context=context;
        }
        public async Task<User> CreateUserAsync(User user)
        {
          await  _context.Users.AddAsync(user);
          await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool?> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users
                           .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
                    _context.Users.Remove(user);
              await _context.SaveChangesAsync();
                return true;
            
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
