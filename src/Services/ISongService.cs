using DockerPackaging.Models;

namespace DockerPackaging.Services;

public interface ISongService
{
    Task<IEnumerable<Song>> GetAllSongsAsync();
    Task<Song?> GetSongByIdAsync(int id);
    Task<Song> CreateSongAsync(Song song);
    Task<bool> UpdateSongAsync(int id, Song songUpdate);
    Task<bool> DeleteSongAsync(int id);
}
