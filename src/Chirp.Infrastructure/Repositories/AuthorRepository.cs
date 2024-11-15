using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDbContext _context;
    
    public AuthorRepository(ChirpDbContext context)
    {
        _context = context;
        // should be seeeded in program cs 
        // DbInitializer.SeedDatabase(context);
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
}