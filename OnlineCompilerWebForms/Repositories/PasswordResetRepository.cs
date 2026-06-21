using System;
using System.Data;
using System.Data.SqlClient;
using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.Repositories
{
    public class PasswordResetRepository
    {
        public void Create(PasswordReset reset)
        {
            string query = @"
                INSERT INTO PasswordResets (Id, UserId, Token, ExpiryDate, IsUsed)
                VALUES (@Id, @UserId, @Token, @ExpiryDate, @IsUsed)";

            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Id", reset.Id),
                new SqlParameter("@UserId", reset.UserId),
                new SqlParameter("@Token", reset.Token),
                new SqlParameter("@ExpiryDate", reset.ExpiryDate),
                new SqlParameter("@IsUsed", reset.IsUsed)
            );
        }

        public PasswordReset GetByToken(string token)
        {
            string query = "SELECT * FROM PasswordResets WHERE Token = @Token AND IsUsed = 0";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Token", token));

            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            
            return new PasswordReset
            {
                Id = (Guid)row["Id"],
                UserId = (Guid)row["UserId"],
                Token = row["Token"].ToString(),
                ExpiryDate = (DateTime)row["ExpiryDate"],
                IsUsed = (bool)row["IsUsed"]
            };
        }

        public void MarkAsUsed(Guid id)
        {
            string query = "UPDATE PasswordResets SET IsUsed = 1 WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id));
        }
    }
}
