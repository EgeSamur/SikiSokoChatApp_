using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SikiSokoChatApp.Application.Abstractions.Services;
using SikiSokoChatApp.Domain.Enums;

namespace SikiSokoChatApp.Application.Features.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly string _baseStoragePath;

    public FileStorageService()
    {
        _baseStoragePath = "/var/www/storage";
    }

    public async Task<List<string>> SaveFileAsync(List<IFormFile> files, int userId, MessageType messageType)
    {
        var storagePaths = new List<string>();

        foreach (var file in files)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var folderType = messageType switch
            {
                MessageType.Image => "images",
                MessageType.Video => "videos",
                MessageType.Voice => "voices",
                MessageType.Document => "documents",
                _ => throw new ArgumentException("Geçersiz dosya tipi")
            };

            var relativePath = folderType;
            var fullPath = Path.Combine(_baseStoragePath, relativePath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var filePath = Path.Combine(fullPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // URL formatında path döndürüyoruz
            storagePaths.Add($"{folderType}/{fileName}");
        }

        return storagePaths;
    }

    public async Task<byte[]> GetFileAsync(string path)
    {
        var fullPath = Path.Combine(_baseStoragePath, path);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Dosya bulunamadı");
        }

        return await File.ReadAllBytesAsync(fullPath);
    }

    public async Task DeleteFileAsync(string path)
    {
        var fullPath = Path.Combine(_baseStoragePath, path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}