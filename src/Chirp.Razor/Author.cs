
public class Author

{
    public int AuthorId { get; set; }
    public string name { get; set; }
    public String email { get; set; }
    public required ICollection<Cheep> Cheeps { get; set; }
    
    
    
}