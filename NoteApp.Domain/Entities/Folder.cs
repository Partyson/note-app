namespace NoteApp.Domain.Entities;

public class Folder : BaseEntity
{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Note> Notes { get; set; }
}