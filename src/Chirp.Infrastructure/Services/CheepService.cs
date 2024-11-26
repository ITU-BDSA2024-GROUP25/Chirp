using Chirp.Core;

namespace Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(string? author, int pageNumber);
    public int GetTotalCheepsCount(string? author);
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep, string authorName);
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber);
    public Task<List<CheepDto>> GetAllCheeps(string? author);
}

public class CheepService : ICheepService
{

    private CheepRepository _cheepRepo;
    public CheepService(ChirpDbContext context)
    {
        _cheepRepo = new CheepRepository(context);
    }
    
    public int GetTotalCheepsCount(string? author = null) => _cheepRepo.GetTotalCheepsCount(author);

    public Task<List<CheepDto>> GetCheeps(string? author, int pageNumber) => _cheepRepo.GetCheeps(author, pageNumber);

    public Task CreateCheep(Cheep cheep) => _cheepRepo.CreateCheep(cheep);

    public Task CreateCheep(CheepDto cheep, string authorName) => _cheepRepo.CreateCheep(cheep, authorName);
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber) => _cheepRepo.GetCheepsFromFollowers(authorName, followers, pageNumber);
    public Task<List<CheepDto>> GetAllCheeps(string? author) => _cheepRepo.GetAllCheeps(author);
}
