using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NoteApp.Application.DTOs;
using NoteApp.Application.Interfaces;
using NoteApp.Application.Services;
using NoteApp.Domain.Entities;
using NoteApp.Infrastructure.Interfaces;

namespace NoteApp.Tests.ServiceTest;

public class FolderServiceTests
{
    private readonly Mock<IFolderRepository> folderRepoMock = new();
    private readonly Mock<IImageService> imageServiceMock = new();

    private readonly FolderService folderService;

    public FolderServiceTests()
    {
        folderService = new FolderService(folderRepoMock.Object, imageServiceMock.Object);
    }

    [Fact]
    public async Task CreateFolder_ShouldReturnFolderId()
    {
        var folderDto = new CreateFolderDto { Name = "Test", Image = new FormFileMock() };
        var userId = Guid.NewGuid();
        var expectedFolderId = Guid.NewGuid();

        imageServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<Guid>()))
                        .ReturnsAsync("path/to/image.jpg");

        folderRepoMock.Setup(x => x.CreateFolder(It.IsAny<Folder>()))
                      .ReturnsAsync(expectedFolderId);

        var result = await folderService.CreateFolder(folderDto, userId);

        result.Should().Be(expectedFolderId);
    }

    [Fact]
    public async Task GetFolderById_ShouldReturnFolderDto()
    {
        var folderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var folder = new Folder { Id = folderId, Name = "Test", UserId = userId };

        folderRepoMock.Setup(x => x.GetFolderById(folderId, userId))
                      .ReturnsAsync(folder);

        var result = await folderService.GetFolderById(folderId, userId);

        result.Should().NotBeNull();
        result.Id.Should().Be(folderId);
        result.Name.Should().Be("Test");
    }

    [Fact]
    public async Task GetAllFoldersByUserId_ShouldReturnListOfFolderResponseDto()
    {
        var userId = Guid.NewGuid();
        var folders = new List<Folder> { new () { Id = Guid.NewGuid(), Name = "One", UserId = userId } };

        folderRepoMock.Setup(x => x.GetAllFoldersByUserId(userId))
                      .ReturnsAsync(folders);

        var result = await folderService.GetAllFoldersByUserId(userId);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("One");
    }
}

public class FormFileMock : IFormFile
{
    public string ContentType => "image/jpeg";
    public string ContentDisposition => "inline";
    public IHeaderDictionary Headers => new HeaderDictionary();
    public long Length => 100;
    public string Name => "file";
    public string FileName => "file.jpg";

    public void CopyTo(Stream target) { }
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Stream OpenReadStream() => new MemoryStream(new byte[100]);
}