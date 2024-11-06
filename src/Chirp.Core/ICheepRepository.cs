namespace Chirp.Core;

public interface ICheepRepository
{
    // Queries
    public int CurrentPage { get; set;  }
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);
    public Task<Author?> FindAuthorByName(string name);
    public Task<Author?> FindAuthorByEmail(string email);
    public Task<Author?> GetAuthorById (int id);
    
    // Commands
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep);
    public Task CreateAuthor(Author author);
}
