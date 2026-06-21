using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.Repositories
{
    public class ProjectRepository
    {
        public void Create(Project project)
        {
            string query = @"INSERT INTO Projects (Id, UserId, Name, Description, Language) 
                             VALUES (@Id, @UserId, @Name, @Description, @Language)";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Id", project.Id),
                new SqlParameter("@UserId", project.UserId),
                new SqlParameter("@Name", project.Name),
                new SqlParameter("@Description", (object)project.Description ?? DBNull.Value),
                new SqlParameter("@Language", project.Language)
            );
        }

        public Project GetById(Guid id)
        {
            string query = "SELECT * FROM Projects WHERE Id = @Id AND IsDeleted = 0";
            var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", id));
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new Project
            {
                Id = (Guid)row["Id"],
                UserId = (Guid)row["UserId"],
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Language = row["Language"].ToString(),
                CreatedAt = (DateTime)row["CreatedAt"],
                UpdatedAt = (DateTime)row["UpdatedAt"],
                IsDeleted = (bool)row["IsDeleted"]
            };
        }

        public List<Project> GetByUserId(Guid userId)
        {
            string query = "SELECT * FROM Projects WHERE UserId = @UserId AND IsDeleted = 0 ORDER BY UpdatedAt DESC";
            var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@UserId", userId));
            var projects = new List<Project>();

            foreach (DataRow row in dt.Rows)
            {
                projects.Add(new Project
                {
                    Id = (Guid)row["Id"],
                    UserId = (Guid)row["UserId"],
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Language = row["Language"].ToString(),
                    CreatedAt = (DateTime)row["CreatedAt"],
                    UpdatedAt = (DateTime)row["UpdatedAt"],
                    IsDeleted = (bool)row["IsDeleted"]
                });
            }
            return projects;
        }

        public List<Project> GetAllProjects()
        {
            string query = "SELECT * FROM Projects  ORDER BY UpdatedAt DESC";
            var dt = DatabaseHelper.ExecuteQuery(query);
            var projects = new List<Project>();

            foreach (DataRow row in dt.Rows)
            {
                projects.Add(new Project
                {
                    Id = (Guid)row["Id"],
                    UserId = (Guid)row["UserId"],
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Language = row["Language"].ToString(),
                    CreatedAt = (DateTime)row["CreatedAt"],
                    UpdatedAt = (DateTime)row["UpdatedAt"],
                    IsDeleted = (bool)row["IsDeleted"]
                });
            }
            return projects;
        }
        public void Update(Project project)
        {
            string query = @"UPDATE Projects SET Name = @Name, Description = @Description, 
                             Language = @Language, UpdatedAt = GETDATE() WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Name", project.Name),
                new SqlParameter("@Description", (object)project.Description ?? DBNull.Value),
                new SqlParameter("@Language", project.Language),
                new SqlParameter("@Id", project.Id)
            );
        }
        
        public void Delete(Guid id)
        {
            //string query = "UPDATE Projects SET IsDeleted = 1, UpdatedAt = GETDATE() WHERE Id = @Id";
            string query = "delete from Projects  where Id = @id";
            string fquery = "DELETE FROM Files where ProjectId = @id";
            DatabaseHelper.ExecuteNonQuery(fquery, new SqlParameter("@Id", id));
            DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id));
        }
    }
}
