using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.Services
{
    public class MigrationService
    {
        public void RunMigrations()
        {
            EnsureMigrationHistoryTable();

            string migrationsPath = HostingEnvironment.MapPath("~/App_Data/Migrations");
            if (!Directory.Exists(migrationsPath))
            {
                return;
            }

            string[] sqlFiles = Directory.GetFiles(migrationsPath, "*.sql");
            Array.Sort(sqlFiles);

            foreach (var file in sqlFiles)
            {
                string fileName = Path.GetFileName(file);
                if (!HasMigrationRun(fileName))
                {
                    ExecuteMigration(file, fileName);
                }
            }
        }

        private void EnsureMigrationHistoryTable()
        {
            string query = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MigrationHistory]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[MigrationHistory] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                        [FileName] NVARCHAR(255) NOT NULL,
                        [ExecutedAt] DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END";

            DatabaseHelper.ExecuteNonQuery(query);
        }

        private bool HasMigrationRun(string fileName)
        {
            string query = "SELECT COUNT(1) FROM MigrationHistory WHERE FileName = @FileName";
            int count = (int)DatabaseHelper.ExecuteScalar(query, new SqlParameter("@FileName", fileName));
            return count > 0;
        }

        private void ExecuteMigration(string filePath, string fileName)
        {
            string script = File.ReadAllText(filePath);
            
            // Execute the script
            DatabaseHelper.ExecuteNonQuery(script);

            // Record execution
            string recordQuery = "INSERT INTO MigrationHistory (FileName, ExecutedAt) VALUES (@FileName, @ExecutedAt)";
            DatabaseHelper.ExecuteNonQuery(recordQuery,
                new SqlParameter("@FileName", fileName),
                new SqlParameter("@ExecutedAt", DateTime.Now)
            );
        }
    }
}
