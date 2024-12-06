using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class Author

{
    [Key]
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
    public ICollection<Author>? Following { get; set; }
    public ICollection<Cheep>? LikedCheeps { get; set; } = new List<Cheep>();
    public ICollection<Cheep>? DislikedCheeps { get; set; } = new List<Cheep>();
}