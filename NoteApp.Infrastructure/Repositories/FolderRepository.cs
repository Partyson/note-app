using Microsoft.EntityFrameworkCore;
using NoteApp.Domain.Entities;
using NoteApp.Infrastructure.Data;
using NoteApp.Infrastructure.Interfaces;

namespace NoteApp.Infrastructure.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly AppDbContext context;

    public FolderRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> CreateFolder(Folder folder)
    {
        var folderEntity = await context.Folders.AddAsync(folder);
        await context.SaveChangesAsync();
        return folderEntity.Entity.Id;
    }

    public async Task<Folder?> GetFolderById(Guid id, Guid userId)
    {
        var folder = await context.Folders
            .Include(f => f.Notes)
            .Include(f => f.User)
            .Where(f => f.UserId == userId && f.Id == id)
            .FirstOrDefaultAsync();
        return folder;
    }

    public async Task<List<Folder>> GetAllFoldersByUserId(Guid userId)
    {
        var folders = await context.Folders
            .Where(f => f.UserId == userId)
            .Include(f => f.Notes)
            .Include(f => f.User)
            .ToListAsync();
        return folders;
    }
}