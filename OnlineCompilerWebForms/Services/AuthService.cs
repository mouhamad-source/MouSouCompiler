using System;
using System.Threading.Tasks;
using System.Web;
using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Model.DTOs;
using OnlineCompilerWebForms.Repositories;
using OnlineCompilerWebForms.Security;

namespace OnlineCompilerWebForms.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepo;
        private readonly PasswordResetRepository _resetRepo;
        private readonly EmailService _emailService;
        private readonly TokenService _tokenService;

        public AuthService()
        {
            _userRepo = new UserRepository();
            _resetRepo = new PasswordResetRepository();
            _emailService = new EmailService();
            _tokenService = new TokenService();
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO dto)
        {
            if (_userRepo.GetByEmail(dto.Email) != null) return (false, "Email is already registered.");
            if (_userRepo.GetByUsername(dto.Username) != null) return (false, "Username is taken.");

            var token = _tokenService.GenerateToken();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                IsEmailConfirmed = false,
                EmailConfirmationToken = token,
                EmailTokenExpiry = DateTime.Now.AddMinutes(30),
                Role = "User",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _userRepo.Create(user);

            // In Web Forms we might use Request.Url to build the link, passing a simple relative path here
            string confirmLink = $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/confirm?token={token}";
            string body = $"<h2>Welcome to MouSou!</h2><p>Please confirm your email by clicking the link below:</p><a href='{confirmLink}'>Confirm Email</a>";

            await _emailService.SendEmailAsync(dto.Email, "Confirm your MouSou Account", body);

            return (true, "Registration successful. Please check your email to confirm your account.");
        }

        public (bool Success, string Message, User User) Login(LoginDTO dto)
        {
            var user = _userRepo.GetByEmailOrUsername(dto.UsernameOrEmail);
            
            if (user == null) return (false, "Invalid credentials.", null);

            if (user.IsLocked && user.LockoutEnd > DateTime.Now)
            {
                return (false, $"Account is locked. Try again after {user.LockoutEnd.Value.ToString("g")}.", null);
            }
            else if (user.IsLocked && user.LockoutEnd <= DateTime.Now)
            {
                user.IsLocked = false;
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                _userRepo.Update(user);
            }

            if (!user.IsEmailConfirmed)
            {
                return (false, "Please confirm your email before logging in.", null);
            }

            if (!PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                {
                    user.IsLocked = true;
                    user.LockoutEnd = DateTime.Now.AddMinutes(15);
                }
                _userRepo.Update(user);

                return (false, "Invalid credentials.", null);
            }

            // Success
            user.FailedLoginAttempts = 0;
            _userRepo.Update(user);

            return (true, "Login successful.", user);
        }

        public bool ConfirmEmail(string token)
        {
            // For a real prod system we'd look up by token, but we only have GetByEmail/Username in our repo.
            // Let's add a quick custom query here, or we should have added GetByToken to UserRepository.
            // I'll add the query directly here for simplicity or assume we can query users by token.
            string query = "SELECT * FROM Users WHERE EmailConfirmationToken = @Token";
            var dt = OnlineCompilerWebForms.Utils.DatabaseHelper.ExecuteQuery(query, new System.Data.SqlClient.SqlParameter("@Token", token));
            
            if (dt.Rows.Count == 0) return false;

            var row = dt.Rows[0];
            var expiry = row["EmailTokenExpiry"] != DBNull.Value ? (DateTime?)row["EmailTokenExpiry"] : null;
            
            if (expiry == null || expiry.Value < DateTime.Now) return false;

            var userId = (Guid)row["Id"];
            
            // Re-fetch user to use update
            var user = _userRepo.GetByEmail(row["Email"].ToString());
            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailTokenExpiry = null;
            
            _userRepo.Update(user);
            return true;
        }

        public async Task<(bool Success, string Message)> ForgotPasswordAsync(string email)
        {
            var user = _userRepo.GetByEmail(email);
            if (user == null || !user.IsEmailConfirmed) return (false, "Invalid or unconfirmed email.");

            var token = _tokenService.GenerateToken();

            var reset = new PasswordReset
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = token,
                ExpiryDate = DateTime.Now.AddMinutes(15),
                IsUsed = false
            };

            _resetRepo.Create(reset);

            string resetLink = $"{HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/reset-password?token={token}";
            string body = $"<h2>Password Reset</h2><p>Click the link below to reset your password:</p><a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(email, "Reset your password", body);

            return (true, "If that email exists, a reset link has been sent.");
        }

        public (bool Success, string Message) ResetPassword(string token, string newPassword)
        {
            var reset = _resetRepo.GetByToken(token);
            if (reset == null || reset.ExpiryDate < DateTime.Now) return (false, "Invalid or expired token.");

            string userQuery = "SELECT * FROM Users WHERE Id = @Id";
            var dt = OnlineCompilerWebForms.Utils.DatabaseHelper.ExecuteQuery(userQuery, new System.Data.SqlClient.SqlParameter("@Id", reset.UserId));
            
            if (dt.Rows.Count == 0) return (false, "User not found.");
            
            var user = _userRepo.GetByEmail(dt.Rows[0]["Email"].ToString());
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            
            _userRepo.Update(user);
            _resetRepo.MarkAsUsed(reset.Id);

            return (true, "Password reset successful.");
        }
    }
}
