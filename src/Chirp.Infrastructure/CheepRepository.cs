using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

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
    public async Task<List<CheepDto>> GetCheeps(string? author = null)
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            .Where(c => author == null || c.Author.Name == author)
            .Skip(CurrentPage * 32).Take(32)
            .Select(cheep => new CheepDto(cheep.Text, cheep.TimeStamp.ToString(), cheep.Author.Name));
        var result = await query.ToListAsync();
        return result; 
    }

    public int GetTotalCheepsCount(string? author = null)
    {
        var query = _context.Cheeps.AsQueryable();

        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(c => c.Author.Name == author);
        }

        return query.Count();
    }
}