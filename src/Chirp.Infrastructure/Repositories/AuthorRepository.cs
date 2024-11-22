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
        //DbInitializer.SeedDatabase(context);
    }

    public async Task CreateAuthor(AuthorDto authorDto)
    {
        bool doesAuthorExist = await _context.Authors.AnyAsync(a => a.Name == authorDto.userName);

        if (!doesAuthorExist)
        {
            Author author = new Author
            {
                Name = authorDto.userName,
                Email = authorDto.email,
                Cheeps = new List<Cheep>()
            };

            if (author.Email == null) author.Email = author.Name + "@group25ChirpMail.com";
            
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<Author?> FindAuthorByName(string name)
    {
        return await _context.Authors
            .Include(a => a.Following)
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();
    }

    public async Task<Author?> FindAuthorByEmail(string email)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Author?> GetAuthorById(int id)
    {
        return await _context.Authors
            .Where(a => a.AuthorId == id)
            .FirstOrDefaultAsync();
    }

    public async Task FollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        if (author == null) throw new Exception("User: " + userName +" not found");

        
        var targetAuthor = await FindAuthorByName(targetUserName);
        if (targetAuthor == null) throw new Exception("User: " + targetUserName +" not found");

        if (author.Following == null) author.Following = new List<Author>();
        
        if (!author.Following.Contains(targetAuthor))
        {
            author.Following.Add(targetAuthor);
            Console.WriteLine("Added follower: [" + targetAuthor.Name + "] to [" + author.Name + "] following list");
            await _context.SaveChangesAsync(); 
        }
    }

    public async Task<bool> IsFollowing(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        if (author == null) throw new Exception("User: " + userName +" not found");
        
        var targetAuthor = await FindAuthorByName(targetUserName);
        if (targetAuthor == null) throw new Exception("User: " + targetUserName +" not found");
        
        if (author.Following == null) return false;
        
        return author.Following.Contains(targetAuthor);
    }

    public async Task<IList<AuthorDto>> GetFollowers(string authorName)
    {
        var author = await FindAuthorByName(authorName);
        
        if (author?.Following == null)
        {
            return new List<AuthorDto>();
        }

        return author.Following
            .Select(follower => new AuthorDto(follower.Name, follower.Email))
            .ToList();   
    }

    public async Task UnfollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        if (author == null) throw new Exception("User: " + userName +" not found");
        if (author.Following == null) throw new Exception(targetUserName +"'s following list is null");
        
        var targetAuthor = await FindAuthorByName(targetUserName);
        if (targetAuthor == null) throw new Exception("User: " + targetUserName +" not found");
        
        if (author.Following.Contains(targetAuthor))
        {
            author.Following.Remove(targetAuthor);
            await _context.SaveChangesAsync(); 
        }
    }
}