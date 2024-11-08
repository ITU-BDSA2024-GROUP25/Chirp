using Chirp.Core;

namespace Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);
    public int CurrentPage { get; set; }
    public Task CreateCheep(Cheep cheep);
    public Task CreateCheep(CheepDto cheep);
    public Task<Author?> FindAuthorByName(string name);
    public Task CreateAuthor(Author author);
}

public class CheepService : ICheepService
{

    private readonly ChirpDbContext _context;
    private CheepRepository _cheepRepo;
    public CheepService(ChirpDbContext context)
    {
        _context = context;
        _cheepRepo = new CheepRepository(context);
    }

    public int CurrentPage { get; set; }

    public int GetTotalCheepsCount(string? author = null) => _cheepRepo.GetTotalCheepsCount(author);

    public Task<List<CheepDto>> GetCheeps(string? author = null) => _cheepRepo.GetCheeps(author);

    public Task CreateCheep(Cheep cheep) => _cheepRepo.CreateCheep(cheep);

    public Task CreateCheep(CheepDto cheep) => _cheepRepo.CreateCheep(cheep);

    public async Task<Author?> FindAuthorByName(string name) => await _cheepRepo.FindAuthorByName(name);

    public Task CreateAuthor(Author author) => _cheepRepo.CreateAuthor(author);
}
