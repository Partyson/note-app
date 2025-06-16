using NoteApp.Application.DTOs;

namespace NoteApp.Application.Interfaces;

public interface IFolderService
{
    Task<Guid> CreateFolder(CreateFolderDto folderDto, Guid userId);
    Task<FolderDto> GetFolderById(Guid id, Guid userId);
    Task<List<FolderResponseDto>> GetAllFoldersByUserId(Guid userId);

}