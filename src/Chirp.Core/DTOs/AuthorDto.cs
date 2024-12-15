namespace Chirp.Core;

/// <summary>
/// A DTO is used to communicate Author information between back-end and front-end only communicating necessary information  
/// </summary>
public record AuthorDto(string userName, string email);