using NoteApp.Domain.Entities;

namespace NoteApp.Infrastructure.Interfaces;

public interface IFolderRepository
{
    Task<Guid> CreateFolder(Folder folder);
    Task<Folder?> GetFolderById(Guid id, Guid userId);
    Task<List<Folder>> GetAllFoldersByUserId(Guid userId);
}