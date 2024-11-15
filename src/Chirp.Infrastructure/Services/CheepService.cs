using Chirp.Core;

namespace Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);
    public int CurrentPage { get; set; }
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep, string authorName);
}

public class CheepService : ICheepService
{

    private CheepRepository _cheepRepo;
    public CheepService(ChirpDbContext context)
    {
        _cheepRepo = new CheepRepository(context);
    }

    public int CurrentPage { get; set; }

    public int GetTotalCheepsCount(string? author = null) => _cheepRepo.GetTotalCheepsCount(author);

    public Task<List<CheepDto>> GetCheeps(string? author = null) => _cheepRepo.GetCheeps(author);

    public Task CreateCheep(Cheep cheep) => _cheepRepo.CreateCheep(cheep);

    public Task CreateCheep(CheepDto cheep, string authorName) => _cheepRepo.CreateCheep(cheep, authorName);
    
}
