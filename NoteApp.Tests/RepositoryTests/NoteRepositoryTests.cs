using Microsoft.EntityFrameworkCore;
using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;
using NoteApp.Infrastructure.Data;
using NoteApp.Infrastructure.Repositories;

namespace NoteApp.Tests.RepositoryTests;

public class NoteRepositoryTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    
    [Fact]
    public async Task CreateNote_Should_Add_Note()
    {
        var context = GetInMemoryDbContext();
        var repository = new NoteRepository(context);
        
        var note = CreateTestNote(Guid.NewGuid());

        var id = await repository.CreateNote(note);
        var saved = await context.Notes.FindAsync(id);

        Assert.NotNull(saved);
        Assert.Equal(id, saved?.Id);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidNoteTestCases))]
    public async Task CreateNote_ShouldThrow_OnInvalidData(string caseName, Note invalidNote, Type expectedException)
    {
        var context = GetInMemoryDbContext();
        var repository = new NoteRepository(context);

        var exception = await Record.ExceptionAsync(() => repository.CreateNote(invalidNote));

        Assert.NotNull(exception);
        Assert.IsType(expectedException, exception);
    }

    [Fact]
    public async Task GetAllByQuery_Should_Get_Notes_By_Query()
    {
        var context = GetInMemoryDbContext();
        var repo = new NoteRepository(context);
        var userId = Guid.NewGuid();

        await context.Notes.AddRangeAsync(new List<Note>
        {
            CreateTestNote(userId, title:"Important"),
            CreateTestNote(userId, title:"Other"),
            CreateTestNote(userId, title:"Important Note")
        });
        await context.SaveChangesAsync();

        var result = await repo.GetAllByQuery(userId, "important");
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task GetAllByQuery_Should_ReturnNotes_OrderedByUpdatedAtDescending()
    {
        var context = GetInMemoryDbContext();
        var repo = new NoteRepository(context);
        var userId = Guid.NewGuid();

        var now = DateTime.UtcNow;

        var note1 = CreateTestNote(userId, "Search Match", "Content A");
        note1.UpdatedAt = now.AddMinutes(-10);
        note1.CreatedAt = now.AddMinutes(-15);

        var note2 = CreateTestNote(userId, "Search Match", "Content B");
        note2.UpdatedAt = now.AddMinutes(-5);
        note2.CreatedAt = now.AddMinutes(-6);

        var note3 = CreateTestNote(userId, "Search Match", "Content C");
        note3.UpdatedAt = now.AddMinutes(-1);
        note3.CreatedAt = now.AddMinutes(-2);

        context.Notes.AddRange(note1, note2, note3);
        await context.SaveChangesAsync();
        
        var result = await repo.GetAllByQuery(userId, "Search Match");
        
        Assert.Equal(3, result.Count);
        var resultList = result.ToList();
        Assert.True(resultList[0].UpdatedAt > resultList[1].UpdatedAt);
        Assert.True(resultList[1].UpdatedAt > resultList[2].UpdatedAt);
    }
    
    [Fact]
    public async Task GetAllByFolder_Should_ReturnNotes_SortedByImportance_ThenByUpdatedAtDescending()
    {
        var context = GetInMemoryDbContext();
        var repo = new NoteRepository(context);
        var userId = Guid.NewGuid();
        var folderId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var note1 = CreateTestNote(userId, "Note 1", importance: Importance.Common);
        note1.FolderId = folderId;
        note1.UpdatedAt = now.AddMinutes(-1);

        var note2 = CreateTestNote(userId, "Note 2", importance: Importance.Common);
        note2.FolderId = folderId;
        note2.UpdatedAt = now.AddMinutes(-5);

        var note3 = CreateTestNote(userId, "Note 3", importance: Importance.High);
        note3.FolderId = folderId;
        note3.UpdatedAt = now.AddMinutes(-3);

        var note4 = CreateTestNote(userId, "Note 4", importance: Importance.VeryHigh);
        note4.FolderId = folderId;
        note4.UpdatedAt = now.AddMinutes(-2);

        context.Notes.AddRange(note1, note2, note3, note4);
        await context.SaveChangesAsync();

        var result = await repo.GetAllByFolder(userId, folderId, SortParameter.Importance, descending: false);

        var ordered = result.ToList();
    
        Assert.Equal(4, ordered.Count);
        
        Assert.Equal("Note 1", ordered[0].Title); 
        Assert.Equal("Note 2", ordered[1].Title); 
        Assert.Equal("Note 3", ordered[2].Title);
        Assert.Equal("Note 4", ordered[3].Title);
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
    
    
    public static IEnumerable<object[]> GetInvalidNoteTestCases()
    {
        var userId = Guid.NewGuid();

        yield return
        [
            "Missing Title",
            new Note { Content = "Some content", UserId = userId },
            typeof(DbUpdateException)
        ];

        yield return
        [
            "Missing Content",
            new Note { Title = "Test note", UserId = userId },
            typeof(DbUpdateException)
        ];

        yield return
        [
            "Missing Title and Content",
            new Note { UserId = userId },
            typeof(DbUpdateException)
        ];
    }

}
