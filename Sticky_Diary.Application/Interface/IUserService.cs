using Sticky_Dairy.Domain.Models.Entities;

namespace Sticky_Dairy.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        //Task<bool> IsEmailRegisteredAsync(string email);
        Task<bool> RegisterUserAsync(User user);
        Task<bool> VerifyLoginAsync(string email, string password);
        Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<bool> ForgotPasswordAsync(string email);
    }
}
