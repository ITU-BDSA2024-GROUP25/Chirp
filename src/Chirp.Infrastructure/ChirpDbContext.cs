using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

public class ChirpDbContext : IdentityDbContext<AppUser>

{
    
    
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
    
 
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options) {}
    
    // Taken from lecture 7 slides page 22  link : https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_07/Slides.html
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>()
            .HasIndex(c => c.Name)
            .IsUnique();
        modelBuilder.Entity<Author>()
            .HasIndex(c => c.Email)
            .IsUnique();
    }
   
}
