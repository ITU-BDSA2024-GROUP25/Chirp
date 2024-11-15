namespace Chirp.Core;

public interface IAuthorRepository
{
    // Queries
    public Task<Author?> FindAuthorByName(string name);
    public Task<Author?> FindAuthorByEmail(string email);
    public Task<Author?> GetAuthorById (int id);
    
    // Commands
    public Task CreateAuthor(Author author);
    public Task CreateAuthor(string authorName);
}