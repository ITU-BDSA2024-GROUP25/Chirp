namespace Chirp.Core;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// This class represents the Cheep entity
/// </summary>
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
    public ICollection<Author>? LikedBy { get; set; }
    public ICollection<Author>? DislikedBy { get; set; }
}