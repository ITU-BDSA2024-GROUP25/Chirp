
public class Cheep
{
    public  int CheepId { get; set; }
    public required String Text{get;set;}
    public DateTime TimeStamp{get;set;}
    public int AuthorId { get; set; }
    public required Author Author{get;set;}
    
}