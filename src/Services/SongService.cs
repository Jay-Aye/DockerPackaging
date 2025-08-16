using Microsoft.EntityFrameworkCore;
using DockerPackaging.Data;
using DockerPackaging.Models;

namespace DockerPackaging.Services;

public class SongService : ISongService
{
    private readonly ApplicationDbContext _context;

    public SongService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Song>> GetAllSongsAsync()
    {
        return await _context.Songs.ToListAsync();
    }

    public async Task<Song?> GetSongByIdAsync(int id)
    {
        return await _context.Songs.FindAsync(id);
    }

    public async Task<Song> CreateSongAsync(Song song)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(song.Title) || string.IsNullOrWhiteSpace(song.Artist))
        {
            throw new ArgumentException("Title and Artist are required");
        }

        // Ensure the song doesn't have an ID (it will be auto-generated)
        song.Id = 0;
        
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        return song;
    }

    public async Task<bool> UpdateSongAsync(int id, Song songUpdate)
    {
        // Validate ID match
        if (id != songUpdate.Id)
        {
            throw new ArgumentException("ID mismatch");
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(songUpdate.Title) || string.IsNullOrWhiteSpace(songUpdate.Artist))
        {
            throw new ArgumentException("Title and Artist are required");
        }

        var existingSong = await _context.Songs.FindAsync(id);
        if (existingSong == null)
        {
            return false; // Song not found
        }

        // Update the existing song properties
        existingSong.Title = songUpdate.Title;
        existingSong.Artist = songUpdate.Artist;
        existingSong.ReleaseDate = songUpdate.ReleaseDate;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await SongExistsAsync(id))
            {
                return false; // Song no longer exists
            }
            throw; // Re-throw for other concurrency issues
        }
    }

    public async Task<bool> DeleteSongAsync(int id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null)
        {
            return false; // Song not found
        }

        _context.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> SongExistsAsync(int id)
    {
        return await _context.Songs.AnyAsync(e => e.Id == id);
    }
}
