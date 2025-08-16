namespace DockerPackaging.Models;

/// <summary>
/// Represents a music song with basic metadata
/// </summary>
public class Song
{
    /// <summary>
    /// Unique identifier for the song
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Title of the song
    /// </summary>
    /// <example>Me Myself and I</example>
    /// <remarks>Required field. Cannot be empty or whitespace.</remarks>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Name of the artist or band who performed the song
    /// </summary>
    /// <example>De La Soul</example>
    /// <remarks>Required field. Cannot be empty or whitespace.</remarks>
    public string Artist { get; set; } = string.Empty;

    /// <summary>
    /// Date when the song was released
    /// </summary>
    /// <example>1989-03-14T00:00:00.000Z</example>
    /// <remarks>Should be in UTC format for consistency.</remarks>
    public DateTime ReleaseDate { get; set; }
}
