using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.Repositories
{
    public class FileRepository
    {
        public void Create(ProjectFile file)
        {
            string query = @"INSERT INTO Files (Id, ProjectId, FileName, IsEntryPoint) 
                             VALUES (@Id, @ProjectId, @FileName, @IsEntryPoint)";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Id", file.Id),
                new SqlParameter("@ProjectId", file.ProjectId),
                new SqlParameter("@FileName", file.FileName),
                new SqlParameter("@IsEntryPoint", file.IsEntryPoint)
            );
        }

        public List<ProjectFile> GetByProjectId(Guid projectId)
        {
            string query = "SELECT Id, ProjectId, FileName, IsEntryPoint, CreatedAt, UpdatedAt FROM Files WHERE ProjectId = @ProjectId ORDER BY CreatedAt ASC";
            var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@ProjectId", projectId));
            var files = new List<ProjectFile>();
            foreach (DataRow row in dt.Rows)
            {
                files.Add(new ProjectFile
                {
                    Id = (Guid)row["Id"],
                    ProjectId = (Guid)row["ProjectId"],
                    FileName = row["FileName"].ToString(),
                    // Content is omitted – will be filled from disk
                    IsEntryPoint = (bool)row["IsEntryPoint"],
                    CreatedAt = (DateTime)row["CreatedAt"],
                    UpdatedAt = (DateTime)row["UpdatedAt"]
                });
            }
            return files;
        }

        public ProjectFile GetById(Guid fileId)
        {
            string query = "SELECT Id, ProjectId, FileName, IsEntryPoint, CreatedAt, UpdatedAt FROM Files WHERE Id = @Id";
            var dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", fileId));
            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            return new ProjectFile
            {
                Id = (Guid)row["Id"],
                ProjectId = (Guid)row["ProjectId"],
                FileName = row["FileName"].ToString(),
                IsEntryPoint = (bool)row["IsEntryPoint"],
                CreatedAt = (DateTime)row["CreatedAt"],
                UpdatedAt = (DateTime)row["UpdatedAt"]
            };
        }

        public void SetEntryPoint(Guid projectId, Guid fileId)
        {
            string query1 = "UPDATE Files SET IsEntryPoint = 0 WHERE ProjectId = @ProjectId";
            DatabaseHelper.ExecuteNonQuery(query1, new SqlParameter("@ProjectId", projectId));
            string query2 = "UPDATE Files SET IsEntryPoint = 1 WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query2, new SqlParameter("@Id", fileId));
        }

        public void Rename(Guid fileId, string newName)
        {
            string query = "UPDATE Files SET FileName = @Name, UpdatedAt = GETDATE() WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@Name", newName),
                new SqlParameter("@Id", fileId));
        }

        public void Delete(Guid fileId)
        {
            string query = "DELETE FROM Files WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", fileId));
        }

        public void UpdateTimestamp(Guid fileId)
        {
            string query = "UPDATE Files SET UpdatedAt = GETDATE() WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", fileId));
        }
    }
}