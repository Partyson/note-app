using FluentAssertions;
using Moq;
using NoteApp.Application.DTOs;
using NoteApp.Application.Services;
using NoteApp.Domain.Entities;
using NoteApp.Domain.Enums;
using NoteApp.Infrastructure.Interfaces;

namespace NoteApp.Tests.ServiceTest;

public class NoteServiceTests
{
    private readonly Mock<INotesRepository> notesRepoMock = new();
    private readonly NoteService noteService;

    public NoteServiceTests()
    {
        noteService = new NoteService(notesRepoMock.Object);
    }

    [Fact]
    public async Task CreateNote_ShouldReturnNoteId()
    {
        var dto = new CreateNoteDto { Title = "Test", Content = "Test", IsFavorite = false, Importance = Importance.Common};
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();

        notesRepoMock.Setup(x => x.CreateNote(It.IsAny<Note>()))
                     .ReturnsAsync(noteId);

        var result = await noteService.CreateNote(dto, userId);

        result.Should().Be(noteId);
    }

    [Fact]
    public async Task GetNoteById_ShouldReturnNoteDto()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var note = new Note { Id = noteId, Title = "Test", UserId = userId };

        notesRepoMock.Setup(x => x.GetNoteById(noteId, userId))
                     .ReturnsAsync(note);

        var result = await noteService.GetNoteById(noteId, userId);

        result.Should().NotBeNull();
        result.Title.Should().Be("Test");
    }

    [Fact]
    public async Task GetAllByQuery_ShouldReturnList()
    {
        var userId = Guid.NewGuid();
        var notes = new List<Note> { new () { Id = Guid.NewGuid(), Title = "query" } };

        notesRepoMock.Setup(x => x.GetAllByQuery(userId, "query"))
                     .ReturnsAsync(notes);

        var result = await noteService.GetAllByQuery(userId, "query");

        result.Should().HaveCount(1);
        result.Should().Contain(x => x.Title == "query");
    }

    [Fact]
    public async Task ChangeFavouriteStatus_ShouldReturnTrue()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();

        notesRepoMock.Setup(x => x.ChangeFavouriteStatus(noteId, userId))
                     .ReturnsAsync(true);

        var result = await noteService.ChangeFavouriteStatus(noteId, userId);

        result.Should().BeTrue();
    }
}