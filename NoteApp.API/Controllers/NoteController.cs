using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.API.Extensions;
using NoteApp.Application.DTOs;
using NoteApp.Application.Interfaces;
using NoteApp.Domain.Enums;

namespace NoteApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/notes")]
public class NoteController : ControllerBase
{
    private readonly INoteService noteService;

    public NoteController(INoteService noteService)
    {
        this.noteService = noteService;
    }

    
    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto createNoteDto)
    {
        var noteId = await noteService.CreateNote(createNoteDto, User.GetUserId());
        return Ok(noteId);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllByQuery(
        [FromQuery] string query = "")
    {
        var notes = await noteService.GetAllByQuery(User.GetUserId(), query);
        return Ok(notes);
    }

    [HttpGet("folder/{folderId:guid}")]
    public async Task<IActionResult> GetAllByFolder([FromRoute] Guid folderId,
        [FromQuery] SortParameter sortedBy = SortParameter.UpdatedAt, [FromQuery] bool descending = false)
    {
        var notes = await noteService.GetAllByFolder(User.GetUserId(),
            folderId, sortedBy, descending);
        return Ok(notes);
    }

    [HttpGet("root")]
    public async Task<IActionResult> GetNotesWithoutFolders()
    {
        var notes = await noteService.GetNotesWithoutFolders(User.GetUserId());
        return Ok(notes);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var note = await noteService.GetNoteById(id, User.GetUserId());
        if (note == null)
            return NotFound();
        return Ok(note);
    }
    
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateNoteDto updateNoteDto)
    {
        var updateResult = await noteService.UpdateNote(updateNoteDto, id, User.GetUserId());
        return Ok(updateResult);
    }

    [HttpPatch("{id:guid}/favorite")]
    public async Task<IActionResult> ChangeFavoriteStatus([FromRoute] Guid id)
    {
        var result = await noteService.ChangeFavouriteStatus(id, User.GetUserId());
        return Ok(result);
    }
}