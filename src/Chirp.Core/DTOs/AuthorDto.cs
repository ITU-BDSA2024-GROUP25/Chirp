namespace Chirp.Core;

/// <summary>
/// A DTO is used to communicate author information between back-end and front-end only communicating necessary information.
/// </summary>
/// /// <param name="userName">The username of the author.</param>
/// <param name="email">The email address of the author.</param>
public record AuthorDto(string userName, string email);