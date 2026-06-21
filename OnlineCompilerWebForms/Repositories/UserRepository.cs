using System;
using System.Data;
using System.Data.SqlClient;
using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.Repositories
{
    public class UserRepository
    {
        public void Create(User user)
        {
            string query = @"
                INSERT INTO Users (Id, Username, Email, PasswordHash, Salt, IsEmailConfirmed, EmailConfirmationToken, EmailTokenExpiry, Role, CreatedAt, UpdatedAt)
                VALUES (@Id, @Username, @Email, @PasswordHash, @Salt, @IsEmailConfirmed, @EmailConfirmationToken, @EmailTokenExpiry, @Role, @CreatedAt, @UpdatedAt)";

            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Id", user.Id),
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@Salt", user.Salt ?? (object)DBNull.Value),
                new SqlParameter("@IsEmailConfirmed", user.IsEmailConfirmed),
                new SqlParameter("@EmailConfirmationToken", user.EmailConfirmationToken ?? (object)DBNull.Value),
                new SqlParameter("@EmailTokenExpiry", user.EmailTokenExpiry ?? (object)DBNull.Value),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@CreatedAt", user.CreatedAt),
                new SqlParameter("@UpdatedAt", user.UpdatedAt)
            );
        }

        public User GetByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Email", email));
            return MapToUser(dt);
        }

        public User GetByUsername(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = @Username";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Username", username));
            return MapToUser(dt);
        }

        public User GetByEmailOrUsername(string usernameOrEmail)
        {
            string query = "SELECT * FROM Users WHERE Email = @Val OR Username = @Val";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Val", usernameOrEmail));
            return MapToUser(dt);
        }

        public void Update(User user)
        {
            string query = @"
                UPDATE Users SET 
                    PasswordHash = @PasswordHash,
                    IsEmailConfirmed = @IsEmailConfirmed,
                    EmailConfirmationToken = @EmailConfirmationToken,
                    EmailTokenExpiry = @EmailTokenExpiry,
                    FailedLoginAttempts = @FailedLoginAttempts,
                    IsLocked = @IsLocked,
                    LockoutEnd = @LockoutEnd,
                    Role = @Role,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Id", user.Id),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@IsEmailConfirmed", user.IsEmailConfirmed),
                new SqlParameter("@EmailConfirmationToken", user.EmailConfirmationToken ?? (object)DBNull.Value),
                new SqlParameter("@EmailTokenExpiry", user.EmailTokenExpiry ?? (object)DBNull.Value),
                new SqlParameter("@FailedLoginAttempts", user.FailedLoginAttempts),
                new SqlParameter("@IsLocked", user.IsLocked),
                new SqlParameter("@LockoutEnd", user.LockoutEnd ?? (object)DBNull.Value),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@UpdatedAt", DateTime.Now)
            );
        }

        private User MapToUser(DataTable dt)
        {
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new User
            {
                Id = (Guid)row["Id"],
                Username = row["Username"].ToString(),
                Email = row["Email"].ToString(),
                PasswordHash = row["PasswordHash"].ToString(),
                Salt = row["Salt"] != DBNull.Value ? row["Salt"].ToString() : null,
                IsEmailConfirmed = (bool)row["IsEmailConfirmed"],
                EmailConfirmationToken = row["EmailConfirmationToken"] != DBNull.Value ? row["EmailConfirmationToken"].ToString() : null,
                EmailTokenExpiry = row["EmailTokenExpiry"] != DBNull.Value ? (DateTime?)row["EmailTokenExpiry"] : null,
                FailedLoginAttempts = (int)row["FailedLoginAttempts"],
                IsLocked = (bool)row["IsLocked"],
                LockoutEnd = row["LockoutEnd"] != DBNull.Value ? (DateTime?)row["LockoutEnd"] : null,
                Role = row["Role"].ToString(),
                CreatedAt = (DateTime)row["CreatedAt"],
                UpdatedAt = (DateTime)row["UpdatedAt"]
            };
        }
    }
}
