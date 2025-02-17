using Sticky_Dairy.Domain.Models.Entities;
using System.Threading.Tasks;

namespace Sticky_Dairy.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task SaveChangesAsync();
    }
}
