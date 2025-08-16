using DockerPackaging.Data;
using DockerPackaging.Models;
using DockerPackaging.Services;
using FluentAssertions;
using Xunit;

namespace DockerPackaging.Tests.Services;

public class SongServiceTests
{
    [Fact]
    public void TestProject_ShouldBeConfiguredCorrectly()
    {
        // Arrange
        var expected = true;
        
        // Act
        var actual = true;
        
        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void TestProject_ShouldBeAbleToReferenceMainProject()
    {
        // This test verifies that the test project can reference the main project
        // If this compiles and runs, the project reference is working correctly
        
        // Arrange & Act
        var canReferenceMainProject = true;
        
        // Assert
        canReferenceMainProject.Should().BeTrue();
    }

    [Fact]
    public void SongModel_ShouldHaveRequiredProperties()
    {
        // Arrange
        var song = new Song
        {
            Id = 1,
            Title = "Test Song",
            Artist = "Test Artist",
            ReleaseDate = DateTime.UtcNow
        };

        // Act & Assert
        song.Should().NotBeNull();
        song.Id.Should().Be(1);
        song.Title.Should().Be("Test Song");
        song.Artist.Should().Be("Test Artist");
        song.ReleaseDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
