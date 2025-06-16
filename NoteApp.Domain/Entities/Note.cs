using NoteApp.Domain.Enums;

namespace NoteApp.Domain.Entities;

public class Note : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Importance Importance { get; set; }
    public bool IsFavorite { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public Folder? Folder { get; set; }
    public Guid? FolderId { get; set; }
}