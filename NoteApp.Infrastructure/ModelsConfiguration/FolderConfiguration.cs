using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteApp.Domain.Entities;

namespace NoteApp.Infrastructure.ModelsConfiguration;

public class FolderConfiguration: IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.HasMany(f => f.Notes)
            .WithOne(f => f.Folder)
            .HasForeignKey(f => f.FolderId);
        
        builder.Property(f => f.Name).IsRequired();
        builder.Property(f => f.ImagePath).IsRequired();
    }
}