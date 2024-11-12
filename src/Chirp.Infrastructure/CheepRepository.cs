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
// should be seeeded in program cs 
        DbInitializer.SeedDatabase(context);
    }
    public int CurrentPage { get; set; }

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

    public async Task<Author?> FindAuthorByName(string name)
    {
        return await _context.Authors
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();
    }

    public async Task<Author?> FindAuthorByEmail(string email)
    {
        try
        {
            return await _context.Authors
            .Where(a => a.Email == email)
            .FirstOrDefaultAsync();
        }
        catch
        {
            return null;
        }
    }

    public async Task<Author?> GetAuthorById(int id)
    {
        return await _context.Authors
            .Where(a => a.AuthorId == id)
            .FirstOrDefaultAsync();
    }

    public async Task CreateCheep(Cheep cheep)
    {
        if (await FindAuthorByName(cheep.Author.Name) == null)
        {
            await CreateAuthor(cheep.Author);
        }

        if (cheep.TimeStamp == default)
            cheep.TimeStamp = DateTime.UtcNow;

        _context.Cheeps.Add(cheep);
        await _context.SaveChangesAsync();
    }

    public async Task CreateCheep(CheepDto cheep)
    {
        if (await FindAuthorByName(cheep.authorName) == null)
        {
            await CreateAuthor(cheep.authorName);
        }

        Author? author = await FindAuthorByName(cheep.authorName);
        if (author == null)
        {
            throw new Exception("Author not found"); // Replace with appropriate error handling.
        }

        Cheep newCheep = new Cheep
        {
            CheepId = _context.Cheeps.Count() + 1,
            Text = cheep.text,
            TimeStamp = DateTime.Parse(cheep.postedTime),
            AuthorId = FindAuthorByName(cheep.authorName).Id,
            Author = author
        };

        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAuthor(String authorName)
    {
        Author author = new Author
        {
            AuthorId = _context.Authors.Count() + 1,
            Name = authorName,
            Email = authorName,
            Cheeps = new List<Cheep>()
        };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
    }
}