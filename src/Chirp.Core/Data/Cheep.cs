namespace Chirp.Core;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// This class represents the Cheep entity.
/// </summary>
public class Cheep
{
    // Primary key
    public int CheepId { get; set; }

    // Fields
    [StringLength(160)]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public int LikeAmount { get; set; }
    public int DislikeAmount { get; set; }

    // Foreign keys and relationships
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    public ICollection<Author> LikedBy { get; set; } = new List<Author>();
    public ICollection<Author> DislikedBy { get; set; } = new List<Author>();
}