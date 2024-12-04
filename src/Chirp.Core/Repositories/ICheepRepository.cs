namespace Chirp.Core;

public interface ICheepRepository
{
    // Queries
    public Task<List<CheepDto>> GetCheeps(string? author, int pageNumber);
    public int GetTotalCheepsCount(string? author);
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber);
    public Task<List<CheepDto>> GetAllCheeps(string? author);
    
    // Commands
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep, string userName);
    public Task DeleteCheep(CheepDto cheep);

    public Task<int> FindCheepID(CheepDto cheepDto); 
}
