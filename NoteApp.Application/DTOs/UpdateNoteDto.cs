using System.ComponentModel.DataAnnotations;
using NoteApp.Domain.Enums;

namespace NoteApp.Application.DTOs;

public class UpdateNoteDto
{
    [Required(ErrorMessage = "Заголовок обязательно")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Текст заметки обязательно")]
    public string Content { get; set; }
    [Required(ErrorMessage = "Важность заметки обязательно")]
    public Importance Importance { get; set; }
    public Guid? FolderId { get; set; }
}