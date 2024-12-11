namespace Chirp.Core;

public interface ICheepRepository
{
    // Queries
    public Task<List<CheepDto>> GetCheeps(string? author, int pageNumber);
    public int GetTotalCheepsCount(string? author);
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber);
    public Task<List<CheepDto>> GetAllCheeps(string? author);
    public Task<bool> IsCheepLikedByAuthor(string authorName, CheepDto cheep);
    public Task<bool> IsCheepDislikedByAuthor(string authorName, CheepDto cheep);
    public Task<int> GetCheepLikesCount(CheepDto cheep);
    public Task<int> GetCheepDislikesCount(CheepDto cheep);
    public Task<List<CheepDto>> GetLikedCheeps(string? authorName);
    public Task<List<CheepDto>> GetDislikedCheeps(string? authorName);
    
    // Commands
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep, string userName);
    public Task DeleteCheep(CheepDto cheep);
    public Task<int> FindCheepID(CheepDto cheepDto); 
    public Task LikeCheep(string authorName, CheepDto cheep);
    public Task DislikeCheep(string authorName, CheepDto cheep);
    public Task RemoveLikeCheep(string authorName, CheepDto cheep);
    public Task RemoveDislikeCheep(string authorName, CheepDto cheep);
}
