using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

/// <summary>
/// Represents the database context providing configurations for the author and cheep entities.
/// Developed with the help of an LLM.
/// </summary>
public class ChirpDbContext : IdentityDbContext<AppUser>
{
    // Tables in the database
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Author -> Cheeps (One-to-Many)
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Cheeps)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Author -> Following (Many-to-Many)
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Following)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "AuthorFollowings",
                j => j.HasOne<Author>().WithMany().HasForeignKey("FollowingId"),
                j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                j =>
                {
                    j.HasKey("AuthorId", "FollowingId");
                    j.ToTable("AuthorFollowings");
                });

        // Author -> LikedCheeps (Many-to-Many)
        modelBuilder.Entity<Author>()
            .HasMany(a => a.LikedCheeps)
            .WithMany(c => c.LikedBy)
            .UsingEntity<Dictionary<string, object>>(
                "AuthorLikedCheeps",
                j => j.HasOne<Cheep>().WithMany().HasForeignKey("CheepId"),
                j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                j =>
                {
                    j.HasKey("AuthorId", "CheepId");
                    j.ToTable("AuthorLikedCheeps");
                });

        // Author -> DislikedCheeps (Many-to-Many)
        modelBuilder.Entity<Author>()
            .HasMany(a => a.DislikedCheeps)
            .WithMany(c => c.DislikedBy)
            .UsingEntity<Dictionary<string, object>>(
                "AuthorDislikedCheeps",
                j => j.HasOne<Cheep>().WithMany().HasForeignKey("CheepId"),
                j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                j =>
                {
                    j.HasKey("AuthorId", "CheepId");
                    j.ToTable("AuthorDislikedCheeps");
                });

        // Cheep primary key
        modelBuilder.Entity<Cheep>()
            .HasKey(c => c.CheepId);
    }
}
