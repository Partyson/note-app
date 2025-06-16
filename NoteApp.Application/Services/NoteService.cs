using Mapster;
using NoteApp.Application.DTOs;
using NoteApp.Application.Interfaces;
using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;
using NoteApp.Infrastructure.Interfaces;

namespace NoteApp.Application.Services;

public class NoteService : INoteService
{
    private readonly INotesRepository notesRepository;

    public NoteService(INotesRepository notesRepository)
    {
        this.notesRepository = notesRepository;
    }

    public async Task<Guid> CreateNote(CreateNoteDto createNoteDto, Guid userId)
    {
        var note = createNoteDto.Adapt<Note>();
        note.UserId = userId;
        var noteId = await notesRepository.CreateNote(note);
        return noteId;
    }

    public async Task<IEnumerable<NoteResponseDto>> GetAllByQuery(Guid userId, string query)
    {
        var notes = await notesRepository.GetAllByQuery(userId, query);
        var notesResponseDto = notes
            .Select(n => n.Adapt<NoteResponseDto>());
        return notesResponseDto;
    }

    public async Task<IEnumerable<NoteResponseDto>> GetAllByFolder(Guid userId, Guid folderId, SortParameter sortedBy, bool descending)
    {
        var notes = await notesRepository.GetAllByFolder(userId, folderId, 
            sortedBy, descending);
        var notesResponseDto = notes
            .Select(n => n.Adapt<NoteResponseDto>());
        return notesResponseDto;
    }

    public async Task<IEnumerable<NoteResponseDto>> GetNotesWithoutFolders(Guid userId)
    {
        var notes = await notesRepository.GetNotesWithoutFolders(userId);
        var notesResponseDto = notes
            .Select(n => n.Adapt<NoteResponseDto>());
        return notesResponseDto;
    }

    public async Task<NoteDto?> GetNoteById(Guid noteId, Guid userId)
    {
        var note = await notesRepository.GetNoteById(noteId, userId);
        var noteDto = note.Adapt<NoteDto>();
        return noteDto;
    }

    public async Task<bool> UpdateNote(UpdateNoteDto updateNoteDto, Guid noteId, Guid userId)
    {
        var updateNote = updateNoteDto.Adapt<Note>();
        updateNote.Id = noteId;
        updateNote.UserId = userId;
        var updateResult = await notesRepository.UpdateNote(updateNote);
        return updateResult;
    }

    public async Task<bool> ChangeFavouriteStatus(Guid noteId, Guid userId)
    {
        var result = await notesRepository.ChangeFavouriteStatus(noteId, userId);
        return result;
    }
}