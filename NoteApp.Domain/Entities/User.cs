using Microsoft.AspNetCore.Identity;

namespace NoteApp.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public ICollection<Note> Notes { get; set; }
    public ICollection<Folder> Folders { get; set; }
}