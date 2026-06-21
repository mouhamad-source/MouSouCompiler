using System;
using System.Collections.Generic;
using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Repositories;

namespace OnlineCompilerWebForms.Services
{
    public class ProjectService
    {
        private readonly ProjectRepository _projectRepo;
        private readonly FileRepository _fileRepo;

        public ProjectService()
        {
            _projectRepo = new ProjectRepository();
            _fileRepo = new FileRepository();
        }

        public Project CreateProject(Guid userId, string name, string description, string language)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                Description = description,
                Language = language
            };
            
            _projectRepo.Create(project);

            // Create default file based on language
            string defaultFileName = language == "csharp" ? "Program.cs" : "main.py";
            string defaultContent = language == "csharp" 
                ? "using System;\n\nclass Program\n{\n    static void Main(string[] args)\n    {\n        Console.WriteLine(\"Hello World!\");\n    }\n}" 
                : "print('Hello World!')";

            var defaultFile = new ProjectFile
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                FileName = defaultFileName,
                Content = defaultContent,
                IsEntryPoint = true
            };

            _fileRepo.Create(defaultFile);

            return project;
        }

        public List<Project> GetUserProjects(Guid userId)
        {
            return _projectRepo.GetByUserId(userId);
        }

        public Project GetProject(Guid id, Guid currentUserId)
        {
            var project = _projectRepo.GetById(id);
            if (project != null && project.UserId == currentUserId)
                return project;
            
            return null; // Not found or unauthorized
        }

        public void DeleteProject(Guid id, Guid currentUserId)
        {
            var project = GetProject(id, currentUserId);
            if (project != null)
            {
                _projectRepo.Delete(id);
            }
        }
    }
}
