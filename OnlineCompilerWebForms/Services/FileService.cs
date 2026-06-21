using OnlineCompilerWebForms.Model;
using OnlineCompilerWebForms.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace OnlineCompilerWebForms.Services
{
    public class FileService
    {
        private readonly FileRepository _fileRepo;

        public FileService()
        {
            _fileRepo = new FileRepository();
        }

        /// <summary>
        /// Returns project files with content read from disk.
        /// </summary>
        public List<ProjectFile> GetProjectFiles(Guid projectId, string basePath)
        {
            var files = _fileRepo.GetByProjectId(projectId);
            foreach (var file in files)
            {
                string filePath = GetFilePath(basePath, file.FileName);
                file.Content = File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
            }
            return files;
        }

        public void SaveFileContent(Guid fileId, string content, string basePath)
        {
            // Get the file name from DB (cheap operation)
            var file = _fileRepo.GetById(fileId);
            if (file == null) return;
            string filePath = GetFilePath(basePath, file.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, content ?? "");
            _fileRepo.UpdateTimestamp(fileId); // optional: keep UpdatedAt in sync
        }

        public ProjectFile AddFile(Guid projectId, string fileName, string content, string basePath)
        {
            var file = new ProjectFile
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                FileName = fileName,
                IsEntryPoint = false
            };
            // Save metadata to DB (no content)
            _fileRepo.Create(file);

            // Write content to disk
            string filePath = GetFilePath(basePath, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, content ?? "");

            file.Content = content;
            return file;
        }

        public void RenameFile(Guid fileId, string newName, string basePath)
        {
            var oldFile = _fileRepo.GetById(fileId);
            if (oldFile == null) return;
            string oldPath = GetFilePath(basePath, oldFile.FileName);
            string newPath = GetFilePath(basePath, newName);
            if (File.Exists(oldPath))
                File.Move(oldPath, newPath);
            _fileRepo.Rename(fileId, newName);
        }

        public void SetEntryPoint(Guid projectId, Guid fileId)
        {
            _fileRepo.SetEntryPoint(projectId, fileId);
        }

        public void DeleteFile(Guid fileId, string basePath)
        {
            var file = _fileRepo.GetById(fileId);
            if (file != null)
            {
                string filePath = GetFilePath(basePath, file.FileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);
                _fileRepo.Delete(fileId);
            }
        }

        private string GetFilePath(string basePath, string fileName)
        {
            return Path.Combine(basePath, fileName);
        }
    }
}