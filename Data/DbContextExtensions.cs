using Microsoft.EntityFrameworkCore;

namespace DockerPackaging.Data;

public static class DbContextExtensions
{
    public static void EnsureDatabaseSeeded(this ApplicationDbContext context)
    {
        // Ensure the database is created
        context.Database.EnsureCreated();
        
        // Note: HasData() automatically seeds the data when EnsureCreated() is called
        // No additional seeding logic needed here
    }
}
