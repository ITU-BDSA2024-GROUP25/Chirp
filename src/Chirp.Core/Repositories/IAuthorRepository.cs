namespace Chirp.Core;

public interface IAuthorRepository
{
    // Queries
    public Task<Author?> FindAuthorByName(string name);
    public Task<Author?> GetAuthorById (int id);
    public Task<bool> IsFollowing(string userName, string targetUserName);
    public Task<IList<AuthorDto>> GetFollowers(string authorName);
    
    // Commands
    public Task CreateAuthor(AuthorDto authorDto);
    public Task FollowAuthor(string userName, string targetUserName);
    public Task UnfollowAuthor(string userName, string targetUserName);
}