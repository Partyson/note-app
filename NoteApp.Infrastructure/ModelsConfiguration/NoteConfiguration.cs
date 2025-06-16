using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using NoteApp.Domain.Entities;

namespace NoteApp.Infrastructure.ModelsConfiguration;

public class NoteConfiguration: IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.UserId).IsRequired();
        builder.Property(n => n.Title).IsRequired();
        builder.Property(n => n.Content).IsRequired();
        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.UpdatedAt).IsRequired();
    }
}