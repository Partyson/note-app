using Microsoft.EntityFrameworkCore;
using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;
using NoteApp.Infrastructure.Data;
using NoteApp.Infrastructure.Interfaces;

namespace NoteApp.Infrastructure.Repositories;

public class NoteRepository : INotesRepository
{
    private readonly AppDbContext context;

    public NoteRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> CreateNote(Note note)
    {
        var noteEntity = await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
        return noteEntity.Entity.Id;
    }

    public async Task<IReadOnlyCollection<Note>> GetAllByQuery(Guid userId, string query)
    {
        var notes = await context.Notes
            .Where(note => note.UserId == userId && note.Title.ToLower().Contains(query.ToLower()))
            .OrderByDescending(note => note.UpdatedAt)
            .Include(n => n.Folder)
            .ToArrayAsync();
        return notes;
    }

    public async Task<IReadOnlyCollection<Note>> GetAllByFolder(Guid userId, Guid folderId,
        SortParameter sortedBy, bool descending)
    {
        var query = context.Notes
            .Include(n => n.Folder)
            .Where(note => note.UserId == userId && note.FolderId == folderId);
        var sortedQuery = descending
            ? query.OrderByDescending(p => EF.Property<object>(p, sortedBy.ToString()))
            : query.OrderBy(p => EF.Property<object>(p, sortedBy.ToString()));
        var notes = await sortedQuery
            .ThenByDescending(x => x.UpdatedAt)
            .ToArrayAsync();
        return notes;
    }

    public async Task<IReadOnlyCollection<Note>> GetNotesWithoutFolders(Guid userId)
    {
        var notes = await context.Notes
            .Where(note => note.UserId == userId && note.FolderId == null)
            .ToArrayAsync();
        return notes;
    }

    public async Task<Note?> GetNoteById(Guid noteId, Guid userId)
    {
        var note = await context.Notes
            .Include(note => note.Folder)
            .Where(note => note.Id == noteId && note.UserId == userId)
            .FirstOrDefaultAsync();
        return note;
    }

    public async Task<bool> UpdateNote(Note updateNote)
    {
        var note = await context.Notes
            .Where(n => n.Id == updateNote.Id && n.UserId == updateNote.UserId)
            .FirstOrDefaultAsync();
        if (note == null)
            return false;
        
        note.Title = updateNote.Title;
        note.Content = updateNote.Content;
        note.FolderId = updateNote.FolderId;
        note.Importance = updateNote.Importance;
        note.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeFavouriteStatus(Guid noteId, Guid userId)
    {
        var note = await context.Notes
            .Where(n => n.Id == noteId && n.UserId == userId)
            .FirstOrDefaultAsync();
        if (note == null)
            return false;
        note.IsFavorite = !note.IsFavorite;
        await context.SaveChangesAsync();
        return true;
    }
}