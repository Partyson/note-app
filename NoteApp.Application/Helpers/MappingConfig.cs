using JetBrains.Annotations;
using Mapster;
using NoteApp.Application.DTOs;
using NoteApp.Domain.Entities;

namespace NoteApp.Application.Helpers;

[UsedImplicitly]
public class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Folder, FolderDto>
            .NewConfig()
            .Map(dest => dest.Notes, src => src.Notes.Select(n => n.Adapt<NoteResponseDto>()));
        TypeAdapterConfig<Note, NoteResponseDto>
            .NewConfig()
            .Map(dest => dest.FolderName, src => src.Folder == null ? string.Empty : src.Folder.Name)
            .Map(dest => dest.Importance, src => src.Importance.ToString());
        TypeAdapterConfig<Note, NoteDto>
            .NewConfig()
            .Map(dest => dest.FolderName, src => src.Folder == null ? string.Empty : src.Folder.Name)
            .Map(dest => dest.Importance, src => src.Importance.ToString());
    }
}