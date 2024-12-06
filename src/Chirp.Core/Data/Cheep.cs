namespace Chirp.Core;

using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId { get; set; }
    [Required]
    [StringLength(160)]
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; }
    [Required]
    public required Author Author { get; set; }
    public int LikeAmount { get; set; }
    public int DislikeAmount { get; set; }
}