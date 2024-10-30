using Chirp.Core;

namespace Chirp.Infrastructure;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);
    public int CurrentPage { get; set;}
    public Task CreateCheep(Cheep cheep);
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
 
    public int CurrentPage {get; set;}

    public int GetTotalCheepsCount(string? author = null)
    {
        return _cheepRepo.GetTotalCheepsCount(author);
    }

     public Task<List<CheepDto>> GetCheeps(string? author = null)
     {
        return _cheepRepo.GetCheeps(author);
     }

     public Task CreateCheep(Cheep cheep)
     {
         return _cheepRepo.CreateCheep(cheep);
     }

    public async Task<Author?> FindAuthorByName(string name) 
    {
        return await _cheepRepo.FindAuthorByName(name);
    }

    public Task CreateAuthor(Author author) 
    {
        return _cheepRepo.CreateAuthor(author);
    }
}
