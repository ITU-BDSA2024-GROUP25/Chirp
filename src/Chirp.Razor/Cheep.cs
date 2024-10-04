
public class Cheep
{
    public  int CheepId { get; set; }
    public required String text{get;set;}
    public DateTime timestamp{get;set;}
    public int AuthorId { get; set; }
    public required Author author{get;set;}
    
}