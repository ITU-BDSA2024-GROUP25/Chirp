using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public interface ICheepRepository
{
    public Task CreateCheep(Cheep cheep);
    public int CurrentPage { get; set;  }
    public Task<List<Cheep>> GetCheeps();
    public Task<List<Cheep>> GetCheeps(Author author);
}
public class CheepRepository : ICheepRepository 
{
    private readonly ChirpDbContext _context;
    public CheepRepository(ChirpDbContext context)
    {
        _context = context;
    }
    public int CurrentPage { get; set;  }
    
    public async Task CreateCheep(Cheep cheep)
    {
        
    }
    
    //adapted from slides session 6 page 8
    public async Task<List<Cheep>> GetCheeps() 
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip(CurrentPage * 32).Take(32);
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<Cheep>> GetCheeps(Author author)
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.TimeStamp descending
                select cheep)
            .Where(c => c.Author == author)
            .Skip(CurrentPage * 32).Take(32);
        var result = await query.ToListAsync();
        return result;
    }
    
}