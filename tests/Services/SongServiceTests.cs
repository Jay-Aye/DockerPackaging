using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DockerPackaging.Data;
using DockerPackaging.Models;
using DockerPackaging.Services;
using DockerPackaging.Tests.TestHelpers;

namespace DockerPackaging.Tests.Services;

public class SongServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly SongService _songService;

    public SongServiceTests()
    {
        _context = TestDbContext.CreateInMemoryContextWithoutSeeding(Guid.NewGuid().ToString());
        _songService = new SongService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAllSongsAsync_ShouldReturnAllSongs()
    {
        // Arrange
        var songs = new List<Song>
        {
            new() { Title = "Test Song 1", Artist = "Test Artist 1", ReleaseDate = DateTime.UtcNow },
            new() { Title = "Test Song 2", Artist = "Test Artist 2", ReleaseDate = DateTime.UtcNow }
        };
        
        _context.Songs.AddRange(songs);
        await _context.SaveChangesAsync();

        // Act
        var result = await _songService.GetAllSongsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.Title == "Test Song 1");
        result.Should().Contain(s => s.Title == "Test Song 2");
    }

    [Fact]
    public async Task GetSongByIdAsync_WithValidId_ShouldReturnSong()
    {
        // Arrange
        var song = new Song { Title = "Test Song", Artist = "Test Artist", ReleaseDate = DateTime.UtcNow };
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        // Act
        var result = await _songService.GetSongByIdAsync(song.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Song");
        result.Artist.Should().Be("Test Artist");
    }

    [Fact]
    public async Task GetSongByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _songService.GetSongByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateSongAsync_WithValidData_ShouldCreateSong()
    {
        // Arrange
        var song = new Song { Title = "New Song", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };

        // Act
        var result = await _songService.CreateSongAsync(song);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be("New Song");
        result.Artist.Should().Be("New Artist");

        // Verify it was saved to database
        var savedSong = await _context.Songs.FindAsync(result.Id);
        savedSong.Should().NotBeNull();
        savedSong!.Title.Should().Be("New Song");
    }

    [Fact]
    public async Task CreateSongAsync_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var song = new Song { Title = "", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _songService.CreateSongAsync(song));
    }

    [Fact]
    public async Task CreateSongAsync_WithEmptyArtist_ShouldThrowArgumentException()
    {
        // Arrange
        var song = new Song { Title = "New Song", Artist = "", ReleaseDate = DateTime.UtcNow };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _songService.CreateSongAsync(song));
    }

    [Fact]
    public async Task CreateSongAsync_WithWhitespaceTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var song = new Song { Title = "   ", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _songService.CreateSongAsync(song));
    }

    [Fact]
    public async Task CreateSongAsync_WithExistingId_ShouldResetIdToZero()
    {
        // Arrange
        var song = new Song { Id = 999, Title = "New Song", Artist = "New Artist", ReleaseDate = DateTime.UtcNow };

        // Act
        var result = await _songService.CreateSongAsync(song);

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.Id.Should().NotBe(999);
    }

    [Fact]
    public async Task UpdateSongAsync_WithValidData_ShouldUpdateSong()
    {
        // Arrange
        var song = new Song { Title = "Original Title", Artist = "Original Artist", ReleaseDate = DateTime.UtcNow };
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        var updateData = new Song { Id = song.Id, Title = "Updated Title", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow.AddDays(1) };

        // Act
        var result = await _songService.UpdateSongAsync(song.Id, updateData);

        // Assert
        result.Should().BeTrue();

        // Verify the update in database
        var updatedSong = await _context.Songs.FindAsync(song.Id);
        updatedSong.Should().NotBeNull();
        updatedSong!.Title.Should().Be("Updated Title");
        updatedSong.Artist.Should().Be("Updated Artist");
        updatedSong.ReleaseDate.Should().Be(updateData.ReleaseDate);
    }

    [Fact]
    public async Task UpdateSongAsync_WithIdMismatch_ShouldThrowArgumentException()
    {
        // Arrange
        var song = new Song { Title = "Original Title", Artist = "Original Artist", ReleaseDate = DateTime.UtcNow };
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        var updateData = new Song { Id = 999, Title = "Updated Title", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _songService.UpdateSongAsync(song.Id, updateData));
    }

    [Fact]
    public async Task UpdateSongAsync_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var song = new Song { Title = "Original Title", Artist = "Original Artist", ReleaseDate = DateTime.UtcNow };
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        var updateData = new Song { Id = song.Id, Title = "", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _songService.UpdateSongAsync(song.Id, updateData));
    }

    [Fact]
    public async Task UpdateSongAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var updateData = new Song { Id = 999, Title = "Updated Title", Artist = "Updated Artist", ReleaseDate = DateTime.UtcNow };

        // Act
        var result = await _songService.UpdateSongAsync(999, updateData);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteSongAsync_WithValidId_ShouldDeleteSong()
    {
        // Arrange
        var song = new Song { Title = "Song to Delete", Artist = "Artist", ReleaseDate = DateTime.UtcNow };
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        // Act
        var result = await _songService.DeleteSongAsync(song.Id);

        // Assert
        result.Should().BeTrue();

        // Verify it was deleted from database
        var deletedSong = await _context.Songs.FindAsync(song.Id);
        deletedSong.Should().BeNull();
    }

    [Fact]
    public async Task DeleteSongAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Act
        var result = await _songService.DeleteSongAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllSongsAsync_WithNoSongs_ShouldReturnEmptyList()
    {
        // Act
        var result = await _songService.GetAllSongsAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
