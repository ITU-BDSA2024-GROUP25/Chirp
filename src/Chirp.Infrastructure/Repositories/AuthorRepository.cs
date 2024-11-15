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

    public async Task CreateAuthor(AuthorDto authorDto)
    {
        Author author = new Author
        {
            AuthorId = _context.Authors.Count() + 1,
            Name = authorDto.userName,
            Email = authorDto.email,
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

    public async Task FollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByEmail(userName);

        var targetAuthor = await FindAuthorByName(targetUserName);

        if (!author.Following.Contains(targetAuthor))
        {
            author.Following.Add(targetAuthor);
            await _context.SaveChangesAsync(); 
        }
    }

    public async Task<bool> IsFollowing(string userName, string targetUserName)
    {
        var author = await FindAuthorByEmail(userName);

        var targetAuthor = await FindAuthorByName(targetUserName);
        
        return author.Following.Contains(targetAuthor);
    }

    public async Task UnfollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByEmail(userName);

        var targetAuthor = await FindAuthorByName(targetUserName);

        if (!author.Following.Contains(targetAuthor))
        {
            author.Following.Remove(targetAuthor);
            await _context.SaveChangesAsync(); 
        }
    }
}