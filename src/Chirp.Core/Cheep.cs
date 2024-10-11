namespace Chirp.Core;

using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId { get; set; }
    [Required]
    [StringLength(160)]
    public required String Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; }
    [Required]
    public required Author Author { get; set; }
    
}