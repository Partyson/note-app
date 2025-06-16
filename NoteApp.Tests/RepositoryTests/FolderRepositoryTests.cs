using Microsoft.EntityFrameworkCore;
using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;
using NoteApp.Infrastructure.Data;
using NoteApp.Infrastructure.Repositories;

namespace NoteApp.Tests.RepositoryTests;

public class FolderRepositoryTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateFolder_Should_Add_And_Return_Id()
    {
        var context = GetInMemoryDbContext();
        var repository = new FolderRepository(context);

        var folder = new Folder
        {
            Id = Guid.NewGuid(),
            Name = "Test Folder",
            UserId = Guid.NewGuid(),
            ImagePath = "test"
        };

        var id = await repository.CreateFolder(folder);
        var folderInDb = await context.Folders.FindAsync(id);

        Assert.NotNull(folderInDb);
        Assert.Equal(folder.Name, folderInDb?.Name);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidFolderTestCases))]
    public async Task CreateNote_ShouldThrow_OnInvalidData(string caseName, Folder invalidFolder, Type expectedException)
    {
        var context = GetInMemoryDbContext();
        var repository = new FolderRepository(context);

        var exception = await Record.ExceptionAsync(() => repository.CreateFolder(invalidFolder));

        Assert.NotNull(exception);
        Assert.IsType(expectedException, exception);
    }
    
    [Fact]
    public async Task GetFolderById_Should_Return_Folder_With_Notes_And_User()
    {
        var context = GetInMemoryDbContext();
        var repository = new FolderRepository(context);

        var userId = Guid.NewGuid();
        var folderId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Email = "Test@User",
            PasswordHash = "test"
        };

        var folder = new Folder
        {
            Id = folderId,
            ImagePath = "test",
            Name = "Test Folder",
            UserId = userId,
            User = user,
            Notes = new List<Note>
            {
                CreateTestNote(userId, "Note 1", "Content 1"),
                CreateTestNote(userId, "Note 2", "Content 2"),
            }
        };

        await context.Users.AddAsync(user);
        await context.Folders.AddAsync(folder);
        await context.Notes.AddRangeAsync(folder.Notes);
        await context.SaveChangesAsync();

        
        var result = await repository.GetFolderById(folderId, userId);

        
        Assert.NotNull(result);
        Assert.Equal(folderId, result.Id);

        Assert.NotNull(result.Notes);
        Assert.Equal(2, result.Notes.Count);

        Assert.NotNull(result.User);
        Assert.Equal(userId, result.User.Id);
    }

    
    [Fact]
    public async Task GetAllFoldersByUserId_Should_Return_Only_User_Folders()
    {
        var context = GetInMemoryDbContext();
        var repository = new FolderRepository(context);

        var userId = Guid.NewGuid();
        await context.Users.AddAsync(new User()
        {
            Id = userId,
            Email = "test@email.com",
            PasswordHash = "test"
        });
        await context.Folders.AddRangeAsync(new List<Folder>
        {
            new() { Id = Guid.NewGuid(), Name = "Folder 1", UserId = userId, ImagePath = "test" },
            new() { Id = Guid.NewGuid(), Name = "Folder 2", UserId = Guid.NewGuid(), ImagePath = "test" } // другой пользователь
        });
        await context.SaveChangesAsync();

        var result = await repository.GetAllFoldersByUserId(userId);
        Assert.Single(result);
        Assert.Equal("Folder 1", result[0].Name);
    }
    
    public static IEnumerable<object[]> GetInvalidFolderTestCases()
    {
        var userId = Guid.NewGuid();

        yield return
        [
            "Missing Name",
            new Folder { ImagePath = "test", UserId = userId },
            typeof(DbUpdateException)
        ];

        yield return
        [
            "Missing ImagePath",
            new Folder { Name = "Test note", UserId = userId },
            typeof(DbUpdateException)
        ];
    }
    
    private Note CreateTestNote(Guid userId, string title = "Test", string content = "Test",
        Importance importance = Importance.Common, bool isFavorite = false )
    {
        return new Note
        {
            Title = title,
            Content = content,
            UserId = userId,
            Importance = importance,
            IsFavorite = isFavorite
        };
    }
}