using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

/// <summary>
/// This class represents the Author entity also known as Users.
/// </summary>
public class Author
{
    // Primary key
    [Key]
    public int AuthorId { get; set; }
    
    // Fields
    public required string Name { get; set; }
    public required string Email { get; set; }
    
    // Relationships
    public ICollection<Cheep> Cheeps { get; set; }  = new List<Cheep>();
    public ICollection<Author> Following { get; set; } = new List<Author>();
    public ICollection<Cheep> LikedCheeps { get; set; } = new List<Cheep>();
    public ICollection<Cheep> DislikedCheeps { get; set; } = new List<Cheep>();
}