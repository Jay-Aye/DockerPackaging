using Microsoft.EntityFrameworkCore;
using DockerPackaging.Models;

namespace DockerPackaging.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Song> Songs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed sample data using the SeedData class
        modelBuilder.Entity<Song>().HasData(SeedData.Songs);
    }
}
