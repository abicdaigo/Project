using AutoPartsPM.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace AutoPartsPM.Infrastructure.Services;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    public async Task<string> UploadAsync(Stream stream, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        var uploadsDir = Path.Combine(env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(uploadsDir);

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var filePath = Path.Combine(uploadsDir, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);

        return Path.Combine("uploads", folder, uniqueFileName).Replace('\\', '/');
    }

    public Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(env.WebRootPath, filePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public Task<Stream?> GetFileStreamAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(env.WebRootPath, filePath);
        if (!File.Exists(fullPath))
            return Task.FromResult<Stream?>(null);
        return Task.FromResult<Stream?>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }
}
