using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteApp.Domain.Entities;

namespace NoteApp.Infrastructure.ModelsConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(u => u.Notes)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId);
        
        builder.HasMany(u => u.Folders)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserId);
    }
}