namespace Chirp.Core;

public class Author

{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required String Email { get; set; }
    public required ICollection<Cheep> Cheeps { get; set; }
    
    
    
}