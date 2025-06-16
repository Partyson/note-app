using Microsoft.AspNetCore.Http;

namespace NoteApp.Application.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file, Guid folderId);
}