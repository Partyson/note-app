using Microsoft.AspNetCore.Http;

namespace NoteApp.Application.DTOs;

public class FolderDto
{
    public Guid Id { get; set; }
    public List<NoteResponseDto> Notes { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
}