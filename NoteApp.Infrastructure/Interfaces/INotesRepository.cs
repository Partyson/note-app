using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;

namespace NoteApp.Infrastructure.Interfaces;

public interface INotesRepository
{
    Task<Guid> CreateNote(Note note);
    Task<IReadOnlyCollection<Note>> GetAllByQuery(Guid userId, string query);
    Task<IReadOnlyCollection<Note>> GetAllByFolder(Guid userId, Guid folderId, SortParameter sortedBy, bool descending);
    Task<IReadOnlyCollection<Note>> GetNotesWithoutFolders(Guid userId);
    Task<Note?> GetNoteById(Guid noteId, Guid userId);
    Task<bool> UpdateNote(Note updateNote);
    Task<bool> ChangeFavouriteStatus(Guid noteId, Guid userId);
}