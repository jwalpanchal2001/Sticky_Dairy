using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Data;
using System.Threading.Tasks;

namespace Sticky_Dairy.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Sticky_Dairy_dbContext _context;

        public UserRepository(Sticky_Dairy_dbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.EmailAddress == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
