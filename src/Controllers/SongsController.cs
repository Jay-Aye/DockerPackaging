using Microsoft.AspNetCore.Mvc;
using DockerPackaging.Models;
using DockerPackaging.Services;

namespace DockerPackaging.Controllers;

/// <summary>
/// Provides CRUD operations for managing songs in the music library
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class SongsController : ControllerBase
{
    private readonly ISongService _songService;

    public SongsController(ISongService songService)
    {
        _songService = songService;
    }

    /// <summary>
    /// Retrieves all songs from the music library
    /// </summary>
    /// <returns>A collection of all songs</returns>
    /// <response code="200">Returns the list of songs</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Song>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
    {
        var songs = await _songService.GetAllSongsAsync();
        return Ok(songs);
    }

    /// <summary>
    /// Retrieves a specific song by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the song</param>
    /// <returns>The requested song</returns>
    /// <response code="200">Returns the requested song</response>
    /// <response code="404">If the song with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Song), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Song>> GetSong(int id)
    {
        var song = await _songService.GetSongByIdAsync(id);
        
        if (song == null)
        {
            return NotFound(new { message = "Song not found" });
        }

        return Ok(song);
    }

    /// <summary>
    /// Creates a new song in the music library
    /// </summary>
    /// <param name="song">The song data to create</param>
    /// <returns>The newly created song with assigned ID</returns>
    /// <response code="201">Returns the newly created song</response>
    /// <response code="400">If the song data is invalid (missing title or artist)</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Song), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Song>> CreateSong(Song song)
    {
        try
        {
            var createdSong = await _songService.CreateSongAsync(song);
            return CreatedAtAction(nameof(GetSong), new { id = createdSong.Id }, createdSong);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing song in the music library
    /// </summary>
    /// <param name="id">The unique identifier of the song to update</param>
    /// <param name="songUpdate">The updated song data</param>
    /// <returns>No content on successful update</returns>
    /// <response code="204">If the song was successfully updated</response>
    /// <response code="400">If the song data is invalid (missing title or artist)</response>
    /// <response code="404">If the song with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSong(int id, Song songUpdate)
    {
        try
        {
            var success = await _songService.UpdateSongAsync(id, songUpdate);
            
            if (!success)
            {
                return NotFound(new { message = "Song not found" });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Removes a song from the music library
    /// </summary>
    /// <param name="id">The unique identifier of the song to delete</param>
    /// <returns>Success message on deletion</returns>
    /// <response code="200">If the song was successfully deleted</response>
    /// <response code="404">If the song with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSong(int id)
    {
        var success = await _songService.DeleteSongAsync(id);
        
        if (!success)
        {
            return NotFound(new { message = "Song not found" });
        }

        return Ok(new { message = "Song deleted successfully" });
    }
}
