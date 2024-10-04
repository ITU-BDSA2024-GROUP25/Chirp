
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class ChirpDbContext : DbContext

{
    
    
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
 
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options) {}
   
}