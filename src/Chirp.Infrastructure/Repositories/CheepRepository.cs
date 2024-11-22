using System;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDbContext _context;
    public CheepRepository(ChirpDbContext context)
    {
        _context = context;
        // should be seeeded in program cs : Arent we already?
        //DbInitializer.SeedDatabase(context);
    }

    //adapted from slides session 6 page 8
    public async Task<List<CheepDto>> GetCheeps(string? author = null, int pageNumber = 1)
    {
        var query = (from cheep in _context.Cheeps
                     orderby cheep.TimeStamp descending
                     select cheep)
            .Include(c => c.Author)
            .Where(c => author == null || c.Author.Name == author)
            .Skip((pageNumber - 1) * 32).Take(32)
            .Select(cheep => new CheepDto(cheep.Text, cheep.TimeStamp.ToString(), cheep.Author.Name));
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber = 1)
    {
        var authorNames = followers.Select(a => a.userName).ToList();
        authorNames.Add(authorName); // also include owners cheeps
        
        var query = (from cheep in _context.Cheeps
                where authorNames.Contains(cheep.Author.Name)
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip((pageNumber - 1) * 32).Take(32)
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

    public async Task CreateCheep(Cheep cheep)
    {
        if (cheep.TimeStamp == default)
            cheep.TimeStamp = DateTime.UtcNow;

        _context.Cheeps.Add(cheep);
        await _context.SaveChangesAsync();
    }

    public async Task CreateCheep(CheepDto cheep, string authorName)
    {
        var author = await _context.Authors
            .Where(a => a.Name == authorName)
            .FirstOrDefaultAsync();

        if (author == null)
        {
            throw new Exception("Author not found"); // Replace with appropriate error handling.
        }

        Cheep newCheep = new Cheep
        {
            CheepId = _context.Cheeps.Count() + 1,
            Text = cheep.text,
            TimeStamp = DateTime.Parse(cheep.postedTime),
            AuthorId = author.AuthorId,
            Author = author
        };

        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }
}