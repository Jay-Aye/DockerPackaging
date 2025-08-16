using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DockerPackaging.Controllers;
using DockerPackaging.Models;
using DockerPackaging.Services;

namespace DockerPackaging.Tests.Controllers;

public class SongsControllerTests
{
    private readonly Mock<ISongService> _mockSongService;
    private readonly SongsController _controller;

    public SongsControllerTests()
    {
        _mockSongService = new Mock<ISongService>();
        _controller = new SongsController(_mockSongService.Object);
    }

    [Fact]
    public async Task GetSongs_ShouldReturnOkResultWithSongs()
    {
        // Arrange
        var expectedSongs = new List<Song>
        {
            new() { Id = 1, Title = "Song 1", Artist = "Artist 1", ReleaseDate = DateTime.UtcNow },
            new() { Id = 2, Title = "Song 2", Artist = "Artist 2", ReleaseDate = DateTime.UtcNow }
        };
        _mockSongService.Setup(s => s.GetAllSongsAsync()).ReturnsAsync(expectedSongs);

        // Act
        var result = await _controller.GetSongs();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var songs = okResult.Value.Should().BeOfType<List<Song>>().Subject;
        songs.Should().HaveCount(2);
        songs.Should().BeEquivalentTo(expectedSongs);
    }

    [Fact]
    public async Task GetSong_WithValidId_ShouldReturnOkResultWithSong()
    {
        // Arrange
        var song = new Song { Id = 1, Title = "Test Song", Artist = "Test Artist", ReleaseDate = DateTime.UtcNow };
        _mockSongService.Setup(s => s.GetSongByIdAsync(1)).ReturnsAsync(song);

        // Act
        var result = await _controller.GetSong(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedSong = okResult.Value.Should().BeOfType<Song>().Subject;
        returnedSong.Should().BeEquivalentTo(song);
    }

    [Fact]
    public async Task GetSong_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockSongService.Setup(s => s.GetSongByIdAsync(999)).ReturnsAsync((Song?)null);

        // Act
        var result = await _controller.GetSong(999);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        // Note: We can't easily test the anonymous object content, but we can verify the status code
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task CreateSong_WithValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var songToCreate = new Song { Title = "New Song", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };
        var createdSong = new Song { Id = 1, Title = "New Song", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };
        _mockSongService.Setup(s => s.CreateSongAsync(It.IsAny<Song>())).ReturnsAsync(createdSong);

        // Act
        var result = await _controller.CreateSong(songToCreate);

        // Assert
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(SongsController.GetSong));
        createdAtActionResult.RouteValues.Should().ContainKey("id");
        createdAtActionResult.RouteValues["id"].Should().Be(1);
        var returnedSong = createdAtActionResult.Value.Should().BeOfType<Song>().Subject;
        returnedSong.Should().BeEquivalentTo(createdSong);
    }

    [Fact]
    public async Task CreateSong_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var songToCreate = new Song { Title = "", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };
        _mockSongService.Setup(s => s.CreateSongAsync(It.IsAny<Song>()))
            .ThrowsAsync(new ArgumentException("Title and Artist are required"));

        // Act
        var result = await _controller.CreateSong(songToCreate);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateSong_WithValidData_ShouldReturnNoContent()
    {
        // Arrange
        var songUpdate = new Song { Id = 1, Title = "Updated Song", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow };
        _mockSongService.Setup(s => s.UpdateSongAsync(1, songUpdate)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateSong(1, songUpdate);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateSong_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var songUpdate = new Song { Id = 999, Title = "Updated Song", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow };
        _mockSongService.Setup(s => s.UpdateSongAsync(999, songUpdate)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateSong(999, songUpdate);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateSong_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var songUpdate = new Song { Id = 1, Title = "", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow };
        _mockSongService.Setup(s => s.UpdateSongAsync(1, songUpdate))
            .ThrowsAsync(new ArgumentException("Title and Artist are required"));

        // Act
        var result = await _controller.UpdateSong(1, songUpdate);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task DeleteSong_WithValidId_ShouldReturnOk()
    {
        // Arrange
        _mockSongService.Setup(s => s.DeleteSongAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteSong(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteSong_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockSongService.Setup(s => s.DeleteSongAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteSong(999);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }
}
