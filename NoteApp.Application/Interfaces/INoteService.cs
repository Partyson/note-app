using NoteApp.Application.DTOs;
using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;

namespace NoteApp.Application.Interfaces;

public interface INoteService
{
    Task<Guid> CreateNote(CreateNoteDto createNoteDto, Guid userId);
    Task<IEnumerable<NoteResponseDto>> GetAllByQuery(Guid userId, string query);
    Task<IEnumerable<NoteResponseDto>> GetAllByFolder(Guid userId, Guid folderId,
        SortParameter sortedBy, bool descending);

    Task<IEnumerable<NoteResponseDto>> GetNotesWithoutFolders(Guid userId);
    Task<NoteDto> GetNoteById(Guid noteId, Guid userId);
    Task<bool> UpdateNote(UpdateNoteDto updateNoteDto, Guid noteId, Guid userId);
    Task<bool> ChangeFavouriteStatus(Guid noteId, Guid userId);
}