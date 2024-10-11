namespace Chirp.Core;

public interface ICheepRepository
{
    // Queries
    public int CurrentPage { get; set;  }
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);
    public Task<Author?> FindAuthorByName(string name);
    public Task<Author?> FindAuthorByEmail(string email);

    
    // Commands
    public Task CreateCheep(Cheep cheep);
    public Task CreateAuthor(Author author);
}
