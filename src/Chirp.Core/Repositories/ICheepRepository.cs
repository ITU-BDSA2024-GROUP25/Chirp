namespace Chirp.Core;

public interface ICheepRepository
{
    // Queries
    public int CurrentPage { get; set;  }
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);

    
    // Commands
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep, string userName);
}
