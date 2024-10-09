
using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId { get; set; }
    [Required]
    [StringLength(160)]
    public String Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; }
    [Required]
    public Author Author { get; set; }
    
}