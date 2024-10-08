
public class Author

{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public String Email { get; set; }
    public required ICollection<Cheep> Cheeps { get; set; }
    
    
    
}