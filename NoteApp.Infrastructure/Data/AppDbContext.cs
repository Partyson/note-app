using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteApp.Domain.Entities;
using NoteApp.Infrastructure.ModelsConfiguration;

namespace NoteApp.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Folder> Folders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new NoteConfiguration());
            builder.ApplyConfiguration(new FolderConfiguration());
        }
    }
}