using NoteApp.UI.Enums;
namespace NoteApp.UI.DTOs;

public class UpdateNoteDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid? FolderId { get; set; }
    public Importance Importance { get; set; }
}