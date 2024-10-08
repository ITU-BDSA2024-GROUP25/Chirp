
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class ChirpDbContext : DbContext

{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
 
    public ChirpDbContext() 
    {
        DbPath = "data/chirp.db";
    }
   
}