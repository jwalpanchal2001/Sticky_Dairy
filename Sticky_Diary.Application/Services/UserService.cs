
using Microsoft.AspNetCore.Identity;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Domain.Models.Entities;
using System.Threading.Tasks;

namespace Sticky_Dairy.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserService(IUserRepository userRepository, EmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        //public async Task<bool> IsEmailRegisteredAsync(string email)
        //{
        //    return await _userRepository.IsEmailRegisteredAsync(email);
        //}

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // Email not found
            }

            // Generate a temporary password
            string tempPassword = GenerateRandomPassword();

            // Hash and store the temporary password
            user.Password = _passwordHasher.HashPassword(user, tempPassword);
            await _userRepository.UpdateUserAsync(user);

            // Send email with temporary password
            string emailBody = $"<h3>Temporary Password</h3><p>Your new temporary password is: <strong>{tempPassword}</strong></p>";
            await _emailService.SendEmailAsync(user.EmailAddress, "Password Reset Request", emailBody);

            return true;
        }


        public async Task<bool> RegisterUserAsync(User user)
        {
            if (await _userRepository.IsEmailRegisteredAsync(user.EmailAddress))
            {
                return false; // Email already registered
            }

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            string emailBody = $@"
                <h3>Welcome to Sticky Diary – Registration Successful! 🎉 </h3>
                <p>Dear <strong>{user.Name}</strong>,</p>
                <p>We are thrilled to welcome you to <strong>Sticky Diary!</strong> Your registration was successful, and your account is now ready to use.</p>
                <p>You can log in anytime to start adding notes, saving files, and organizing your thoughts.</p>
                <br>
                <p>Best regards,</p>
                <p><strong>Sticky Diary Team</strong></p>";

            await _emailService.SendEmailAsync(user.EmailAddress, "Registration Successful", emailBody);
            return true;
        }

        public async Task<bool> VerifyLoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            return _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;
        }

        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, oldPassword);
            if (verificationResult != PasswordVerificationResult.Success) return false;

            user.Password = _passwordHasher.HashPassword(user, newPassword);
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }


        private string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8); // 8-character random password
        }
    }
}
