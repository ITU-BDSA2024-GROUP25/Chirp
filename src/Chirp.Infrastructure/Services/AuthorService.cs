using Chirp.Core;

namespace Chirp.Infrastructure;

/// <summary>
/// Interface for the author service governing author related methods for the service
/// This service layer primarily delegates to the AuthorRepository class 
/// This structure ensures abstraction of data access details from the upper layers
/// </summary>
public interface IAuthorService
{
    #region Queries

    public Task<Author> FindAuthorByName(string name);
    public Task<bool> IsFollowing(string userName, string targetUserName);
    public Task<IList<AuthorDto>> GetFollowers(string authorName);

    #endregion

    #region Commands

    public Task CreateAuthor(AuthorDto authorDto);
    public Task FollowAuthor(string userName, string targetUserName);
    public Task UnfollowAuthor(string userName, string targetUserName);
    public Task DeleteAuthor(string authorName);

    #endregion
}

/// <summary>
/// Service implementation for managing authors and their relationships
/// </summary>
public class AuthorService : IAuthorService
{
    private AuthorRepository _authorRepo;
        
    /// <summary>
    /// Initializes a new instance of the AuthorRepository class with a specified database context
    /// </summary>
    /// <param name="context">The database context used for accessing the database</param>
    public AuthorService(ChirpDbContext context)
    {
        _authorRepo = new AuthorRepository(context);
    }
        
    #region Queries

    public async Task<Author> FindAuthorByName(string name) => await _authorRepo.FindAuthorByName(name);
    public Task<bool> IsFollowing(string userName, string targetUserName) => _authorRepo.IsFollowing(userName, targetUserName);
    public async Task<IList<AuthorDto>> GetFollowers(string authorName) => await _authorRepo.GetFollowers(authorName);

    #endregion

    #region Commands

    public Task CreateAuthor(AuthorDto authorDto) => _authorRepo.CreateAuthor(authorDto);
    public Task FollowAuthor(string userName, string targetUserName) => _authorRepo.FollowAuthor(userName, targetUserName);
    public Task UnfollowAuthor(string userName, string targetUserName) => _authorRepo.UnfollowAuthor(userName, targetUserName);
    public Task DeleteAuthor(string authorName) => _authorRepo.DeleteAuthor(authorName);

    #endregion
}