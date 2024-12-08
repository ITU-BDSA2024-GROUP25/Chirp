using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class Author

{
    [Key]
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required ICollection<Cheep> Cheeps { get; set; }
    
    public ICollection<Author>? Following { get; set; }
}