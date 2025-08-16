using Microsoft.EntityFrameworkCore;
using DockerPackaging.Data;

namespace DockerPackaging.Tests.TestHelpers;

public static class TestDbContext
{
    public static ApplicationDbContext CreateInMemoryContext(string databaseName = "TestDb")
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        
        return context;
    }

    public static ApplicationDbContext CreateInMemoryContextWithoutSeeding(string databaseName = "TestDb")
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        
        // Clear any seeded data
        context.Songs.RemoveRange(context.Songs);
        context.SaveChanges();
        
        return context;
    }
}
