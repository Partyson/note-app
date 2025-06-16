using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace NoteApp.Application.DTOs;

public class CreateFolderDto
{
    [Required(ErrorMessage = "Название обязательно")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Изображение обязательно")]
    public IFormFile Image { get; set; }
}