using Microsoft.AspNetCore.Http;
using SikiSokoChatApp.Domain.Enums;

namespace SikiSokoChatApp.Application.Abstractions.Services;

public interface IFileStorageService
{
    Task<List<string>> SaveFileAsync(List<IFormFile> files, int userId, MessageType messageType);
    Task<byte[]> GetFileAsync(string path);
    Task DeleteFileAsync(string path);
}