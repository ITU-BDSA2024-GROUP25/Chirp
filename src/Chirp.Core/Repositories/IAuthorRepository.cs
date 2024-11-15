namespace Chirp.Core;

public interface IAuthorRepository
{
    // Queries
    public Task<Author?> FindAuthorByName(string name);
    public Task<Author?> FindAuthorByEmail(string email);
    public Task<Author?> GetAuthorById (int id);
    public Task<bool> IsFollowing(string userName, string targetUserName);
    
    // Commands
    public Task CreateAuthor(Author author);
    public Task CreateAuthor(AuthorDto authorDto);
    public Task FollowAuthor(string userName, string targetUserName);
    public Task UnfollowAuthor(string userName, string targetUserName);
}