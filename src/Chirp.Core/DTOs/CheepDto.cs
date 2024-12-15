namespace Chirp.Core;

/// <summary>
/// A DTO is used to communicate Cheep information between back-end and front-end only communicating necessary information  
/// </summary>
public record CheepDto(string text, string postedTime, string authorName);
