using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public interface ICheepRepository
{
    public Task CreateCheep(Cheep cheep);
    public int CurrentPage { get; set;  }
    public Task<List<Cheep>> GetCheeps(string? author);
}
public class CheepRepository : ICheepRepository 
{
    private readonly ChirpDbContext _context;
    public CheepRepository(ChirpDbContext context)
    {
        _context = context;
        DbInitializer.SeedDatabase(context);
    }
    public int CurrentPage { get; set;  }

    public async Task CreateCheep(Cheep cheep)
    {
        await Task.CompletedTask;
    }

    //adapted from slides session 6 page 8
    public async Task<List<Cheep>> GetCheeps(string? author = null) 
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            //.Include(c => c.Author.Name == author || author == null)
            .Skip(CurrentPage * 32).Take(32);
        var result = await query.ToListAsync();
        return result; 
    }
}