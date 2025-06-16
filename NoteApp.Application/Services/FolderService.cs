using Mapster;
using NoteApp.Application.DTOs;
using NoteApp.Application.Interfaces;
using NoteApp.Domain.Entities;
using NoteApp.Infrastructure.Interfaces;

namespace NoteApp.Application.Services;

public class FolderService : IFolderService
{
    private readonly IFolderRepository folderRepository;
    private readonly IImageService imageService;

    public FolderService(IFolderRepository folderRepository, IImageService imageService)
    {
        this.folderRepository = folderRepository;
        this.imageService = imageService;
    }

    public async Task<Guid> CreateFolder(CreateFolderDto folderDto, Guid userId)
    {
        var folder = folderDto.Adapt<Folder>();
        folder.UserId = userId;
        folder.ImagePath = await imageService.UploadImageAsync(folderDto.Image, folder.Id);
        var folderId = await folderRepository.CreateFolder(folder);
        return folderId;
    }

    public async Task<FolderDto?> GetFolderById(Guid id, Guid userId)
    {
        var folder = await folderRepository.GetFolderById(id, userId);
        var folderDto = folder.Adapt<FolderDto>();
        return folderDto;
    }

    public async Task<List<FolderResponseDto>> GetAllFoldersByUserId(Guid userId)
    {
        var folders = await folderRepository.GetAllFoldersByUserId(userId);
        var foldersResponseDto = folders
            .Select(f => f.Adapt<FolderResponseDto>())
            .ToList();
        
        return foldersResponseDto;
    }
}