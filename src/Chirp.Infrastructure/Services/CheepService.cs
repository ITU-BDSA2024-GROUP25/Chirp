using Chirp.Core;

namespace Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);

/// <summary>
/// Interface for the Author Service governing methods for the service  
/// </summary>
public interface ICheepService
{
    #region Queries

    public Task<List<CheepDto>> GetCheeps(string? author, int pageNumber);
    public int GetTotalCheepsCount(string? author);
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber);
    public Task<List<CheepDto>> GetAllCheeps(string author);
    public Task<bool> IsCheepLikedByAuthor(string authorName, CheepDto cheep);
    public Task<bool> IsCheepDislikedByAuthor(string authorName, CheepDto cheep);
    public Task<int> GetCheepLikesCount(CheepDto cheep);
    public Task<int> GetCheepDislikesCount(CheepDto cheep);
    public Task<List<CheepDto>> GetLikedCheeps(string authorName);
    public Task<List<CheepDto>> GetDislikedCheeps(string authorName);

    #endregion

    #region Commands

    public Task CreateCheep(CheepDto cheep, string authorName);
    public Task DeleteCheep(CheepDto cheep);
    public Task LikeCheep(string authorName, CheepDto cheep);
    public Task DislikeCheep(string authorName, CheepDto cheep);
    public Task RemoveLikeCheep(string authorName, CheepDto cheep);
    public Task RemoveDislikeCheep(string authorName, CheepDto cheep);

    #endregion
}

public class CheepService : ICheepService
{

    private CheepRepository _cheepRepo;
    public CheepService(ChirpDbContext context)
    {
        _cheepRepo = new CheepRepository(context);
    }
    
    #region Queries
    
    public Task<List<CheepDto>> GetCheeps(string? author, int pageNumber) => _cheepRepo.GetCheeps(author, pageNumber);
    public int GetTotalCheepsCount(string? author) => _cheepRepo.GetTotalCheepsCount(author);
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber) =>
        _cheepRepo.GetCheepsFromFollowers(authorName, followers, pageNumber);
    public Task<List<CheepDto>> GetAllCheeps(string author) => _cheepRepo.GetAllCheeps(author);
    public Task<bool> IsCheepLikedByAuthor(string authorName, CheepDto cheep) =>
        _cheepRepo.IsCheepLikedByAuthor(authorName, cheep);
    public Task<bool> IsCheepDislikedByAuthor(string authorName, CheepDto cheep) =>
        _cheepRepo.IsCheepDislikedByAuthor(authorName, cheep);
    public Task<int> GetCheepLikesCount(CheepDto cheep) => _cheepRepo.GetCheepLikesCount(cheep);
    public Task<int> GetCheepDislikesCount(CheepDto cheep) => _cheepRepo.GetCheepDislikesCount(cheep);
    public Task<List<CheepDto>> GetLikedCheeps(string authorName) => _cheepRepo.GetLikedCheeps(authorName);
    public Task<List<CheepDto>> GetDislikedCheeps(string authorName) => _cheepRepo.GetDislikedCheeps(authorName);

    #endregion

    #region Commands

    public Task CreateCheep(CheepDto cheep, string authorName) => _cheepRepo.CreateCheep(cheep, authorName);
    public Task DeleteCheep(CheepDto cheep) => _cheepRepo.DeleteCheep(cheep);
    public Task LikeCheep(string authorName, CheepDto cheep) => _cheepRepo.LikeCheep(authorName, cheep);
    public Task DislikeCheep(string authorName, CheepDto cheep) => _cheepRepo.DislikeCheep(authorName, cheep);
    public Task RemoveLikeCheep(string authorName, CheepDto cheep) => _cheepRepo.RemoveLikeCheep(authorName, cheep);
    public Task RemoveDislikeCheep(string authorName, CheepDto cheep) => _cheepRepo.RemoveDislikeCheep(authorName, cheep);

    #endregion
}
