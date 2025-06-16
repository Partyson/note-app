using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.API.Extensions;
using NoteApp.Application.DTOs;
using NoteApp.Application.Interfaces;

namespace NoteApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/folders")]
public class FolderController(IFolderService folderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFolder(CreateFolderDto createFolderDto)
    {
        var newFolderId = await folderService.CreateFolder(createFolderDto, User.GetUserId());
        return Ok(newFolderId);
    }

    [HttpGet("{folderId:guid}")]
    public async Task<IActionResult> GetFolder([FromRoute] Guid folderId)
    {
        var folder = await folderService.GetFolderById(folderId, User.GetUserId());
        return Ok(folder);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var folders = await folderService.GetAllFoldersByUserId(userId);
        return Ok(folders);
    }
}