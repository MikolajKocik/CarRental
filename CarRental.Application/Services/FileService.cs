using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<string>> SaveFilesAsync(List<IFormFile> files, string folder)
    {
        var savedFiles = new List<string>();

        if (files == null || files.Count == 0)
            return savedFiles;

        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                savedFiles.Add($"{folder}/{fileName}");
            }
        }

        return savedFiles;
    }

    public async Task DeleteFilesAsync(List<string> filePaths, string folder)
    {
        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

        foreach (var filePath in filePaths)
        {
            var fullPath = Path.Combine(uploadFolder, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        await Task.CompletedTask;
    }
}
