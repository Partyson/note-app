using NoteApp.Domain.Entities;

namespace NoteApp.Domain.Extensions;

public static class NoteExtensions
{
    public static void UpdateNoteFields(this Note note, Note updateNote)
    {
        note.Title = updateNote.Title;
        note.Content = updateNote.Content;
        note.FolderId = updateNote.FolderId;
        note.Importance = updateNote.Importance;
        note.UpdatedAt = DateTime.UtcNow;
    }
}