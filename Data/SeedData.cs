using DockerPackaging.Models;

namespace DockerPackaging.Data;

public static class SeedData
{
    public static Song[] Songs => new[]
    {
        // De La Soul classics
        new Song { Id = 1, Title = "Me Myself and I", Artist = "De La Soul", ReleaseDate = new DateTime(1989, 3, 14, 0, 0, 0, DateTimeKind.Utc) },
        new Song { Id = 2, Title = "The Magic Number", Artist = "De La Soul", ReleaseDate = new DateTime(1989, 3, 14, 0, 0, 0, DateTimeKind.Utc) },
        new Song { Id = 3, Title = "Buddy", Artist = "De La Soul", ReleaseDate = new DateTime(1989, 3, 14, 0, 0, 0, DateTimeKind.Utc) },
        
        // TimeFlies hits
        new Song { Id = 4, Title = "I Choose You", Artist = "TimeFlies", ReleaseDate = new DateTime(2011, 6, 6, 0, 0, 0, DateTimeKind.Utc) },
        new Song { Id = 5, Title = "Just a Little Bit", Artist = "TimeFlies", ReleaseDate = new DateTime(2011, 6, 6, 0, 0, 0, DateTimeKind.Utc) },
        new Song { Id = 6, Title = "Turn Back Time", Artist = "TimeFlies", ReleaseDate = new DateTime(2011, 6, 6, 0, 0, 0, DateTimeKind.Utc) },
        
        // Killing Heidi favorites
        new Song { Id = 7, Title = "Weir", Artist = "Killing Heidi", ReleaseDate = new DateTime(2000, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
        new Song { Id = 8, Title = "Mascara", Artist = "Killing Heidi", ReleaseDate = new DateTime(2000, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
        new Song { Id = 9, Title = "Superman", Artist = "Killing Heidi", ReleaseDate = new DateTime(2000, 3, 20, 0, 0, 0, DateTimeKind.Utc) }
    };
}
