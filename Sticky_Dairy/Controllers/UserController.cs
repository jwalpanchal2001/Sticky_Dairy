using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Sticky_Dairy.Application.Interfaces;
using System.Security.Claims;
using Sticky_Dairy.Domain.Models.Entities;  
using Sticky_Dairy.Domain.Models;


namespace Sticky_Dairy.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid) return View(model);

            bool success = await _userService.RegisterUserAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Email already registered!");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid) return View(loginModel);

            bool isValidUser = await _userService.VerifyLoginAsync(loginModel.Email, loginModel.Password);
            if (!isValidUser)
            {
                ModelState.AddModelError("", "Invalid login attempt."); 
                return View();
            }

            var user = await _userService.GetUserByEmailAsync(loginModel.Email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return RedirectToAction("Index", "Notes");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }


        public IActionResult ChangePass() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangePassword model)
        {
            if (!ModelState.IsValid) return View(model);

            string email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            bool success = await _userService.ChangePasswordAsync(email, model.OldPassword, model.NewPassword);
            if (!success)
            {
                ModelState.AddModelError("", "Old password is incorrect or new password is the same as the old one.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Index", "Notes");
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            bool isReset = await _userService.ForgotPasswordAsync(email);

            if (!isReset)
            {
                ModelState.AddModelError("", "Email not found.");
                return View();
            }

            ViewData["PasswordChange"] = "Password has been changed successfully. Sent to your email.";
            return View("Login");
        }

    }
}



//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Sticky_Dairy.Infrastructure.Data;
//using Sticky_Dairy.Api.Models;
//using Sticky_Dairy.Api.Models.Entities;
//using Sticky_Dairy.Api.Repository;
//using Org.BouncyCastle.Asn1;

//namespace Sticky_Dairy.Api.Controllers
//{
//    public class UserController : Controller
//    {
//        private readonly Sticky_Dairy_dbContext _context;
//        private readonly EmailService _emailService;
//        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();




//        public UserController(Sticky_Dairy_dbContext context, EmailService emailService)
//        {
//            _context = context;
//            _emailService = emailService;
//        }

//        public IActionResult Register() => View();
//        [HttpGet]
//        public IActionResult ForgotPassword()
//        {
//            return View();
//        }





//        [HttpPost]
//        public async Task<IActionResult> ForgotPassword(string email)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
//            if (user == null)
//            {
//                ModelState.AddModelError("", "Email not found.");
//                return View();
//            }


//            // Generate a temporary password
//            string tempPassword = _emailService.GenerateRandomPassword();

//            // Hash and store the temporary password
//            user.Password = _passwordHasher.HashPassword(user, tempPassword);
//            await _context.SaveChangesAsync();

//            // Send email with temporary password
//            string emailBody = $"<h3>Temporary Password</h3><p>Your new temporary password is: <strong>{tempPassword}</strong></p>";
//            await _emailService.SendEmailAsync(user.EmailAddress, "Password Reset Request", emailBody);
//            ViewData["PasswordChange"] = "Password Has been Changed Successfully. Sent to your Email.";
//            return View("Login");
//        }



//        [HttpPost]
//        public async Task<IActionResult> Register(User model)
//        {
//            if (ModelState.IsValid)
//            {



//                if (_context.Users.Any(u => u.EmailAddress == model.EmailAddress))
//                {
//                    ModelState.AddModelError("", "Email Already registered!");
//                    return View(model);
//                }
//                model.Password = _passwordHasher.HashPassword(model, model.Password);

//                _context.Users.Add(model);
//                _context.SaveChanges();
//                string emailBody = $"<h3>Welcome to Sticky Diary – Registration Successful! 🎉 </h3>" +
//                    $"" +
//                    $@"
//                <p>Dear <strong>{model.Name}</strong>,</p>
//                <p>We are thrilled to welcome you to <strong>Sticky Diary!</strong> Your registration was successful, and your account is now ready to use.</p>
//                <p>You can log in anytime to start adding notes, saving files, and organizing your thoughts.</p>
//                <br>
//                <p>Best regards,</p>
//                <p><strong>Sticky Diary Team</strong></p>";



//                await _emailService.SendEmailAsync(model.EmailAddress, "Password Reset Request", emailBody);


//                return RedirectToAction("Login");
//            }
//            return View(model);
//        }

//        public IActionResult Login() => View();

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginModel loginModel)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == loginModel.Email);
//                if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password) != PasswordVerificationResult.Success)
//                {
//                    ModelState.AddModelError("", "Invalid login attempt.");
//                    return View();
//                }


//                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.Email, user.EmailAddress) };
//                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//                var authProperties = new AuthenticationProperties
//                {
//                    IsPersistent = true, // Keep session alive
//                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
//                };


//                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

//                return RedirectToAction("Index", "Notes");
//            }

//            return View(loginModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return RedirectToAction("Login", "User");
//        }

//        [HttpGet]
//        public IActionResult AccessDenied()
//        {
//            return View();
//        }


//        public IActionResult ChangePass()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult ChangePass(ChangePassword model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            string username = User.FindFirstValue(ClaimTypes.Email);
//            if (string.IsNullOrEmpty(username))
//            {
//                return Unauthorized(); // User is not logged in
//            }

//            // Fetch user from the database (assuming EF Core)
//            var user = _context.Users.FirstOrDefault(u => u.EmailAddress == username);

//            if (user == null)
//            {
//                return NotFound("User not found.");
//            }

//            // Verify old password (Assuming Identity password hashing)
//            var passwordHasher = new PasswordHasher<User>();
//            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, model.OldPassword);

//            if (verificationResult != PasswordVerificationResult.Success)
//            {
//                ModelState.AddModelError("", "Old password is incorrect.");
//                return View(model);
//            }

//            // Prevent setting the same password again
//            if (model.OldPassword == model.NewPassword)
//            {
//                ModelState.AddModelError("", "Old and new passwords cannot be the same.");
//                return View(model);
//            }

//            // Hash new password and update
//            user.Password = passwordHasher.HashPassword(user, model.NewPassword);
//            _context.SaveChanges();

//            TempData["SuccessMessage"] = "Password changed successfully!";
//            return RedirectToAction("Index", "Notes"); // Redirect to the profile or login page



//        }

//    }
//}
