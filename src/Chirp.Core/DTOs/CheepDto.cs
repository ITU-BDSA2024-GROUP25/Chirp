namespace Chirp.Core;

/// <summary>
/// A DTO is used to communicate Cheep information between back-end and front-end only communicating necessary information  
/// </summary>
/// <param name="text">The content of the cheep</param>
/// <param name="postedTime">The time when the cheep was posted</param>
/// <param name="authorName">The name of the author that posted the cheep</param>
public record CheepDto(string text, string postedTime, string authorName);
