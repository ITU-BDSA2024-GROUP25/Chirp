using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

/// <summary>
/// This class provides CRUD operations for the author entity and manages database relationships.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDbContext _context;

    /// <summary>
    /// Initializes a new instance of the AuthorRepository class with a specified database context.
    /// </summary>
    /// <param name="context">The database context used for accessing the database.</param>
    public AuthorRepository(ChirpDbContext context)
    {
        _context = context;
    }

    #region Queries

    /// <inheritdoc/>
    public async Task<Author> FindAuthorByName(string name)
    {
        var author = await _context.Authors
            .Include(a => a.Following)
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();

        if (author == null) throw new NullReferenceException("Author not found.");
        return author;
    }

    /// <inheritdoc/>
    public async Task<Author> GetAuthorById(int id)
    {
        var author = await _context.Authors
            .Where(a => a.AuthorId == id)
            .FirstOrDefaultAsync();

        if (author == null) throw new NullReferenceException("Author not found.");
        return author;
    }

    /// <inheritdoc/>
    public async Task<bool> IsFollowing(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        var targetAuthor = await FindAuthorByName(targetUserName);

        // Return false if the user's following list is not instantiated, meaning it is empty
        return author.Following?.Contains(targetAuthor) ?? false;
    }

    /// <inheritdoc/>
    public async Task<IList<AuthorDto>> GetFollowers(string authorName)
    {
        var author = await FindAuthorByName(authorName);

        return author.Following
            .Select(follower => new AuthorDto(follower.Name, follower.Email))
            .ToList();
    }

    #endregion

    #region Commands

    /// <inheritdoc/>
    public async Task CreateAuthor(AuthorDto authorDto)
    {
        var doesAuthorExist = await _context.Authors.AnyAsync(a => a.Name == authorDto.userName);

        if (!doesAuthorExist)
        {
            var author = new Author
            {
                Name = authorDto.userName,
                Email = authorDto.email,
                Cheeps = new List<Cheep>()
            };

            // Set email as empty if none is provided (e.g. for GitHub login)
            if (string.IsNullOrEmpty(author.Email)) author.Email = " ";

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task FollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        var targetAuthor = await FindAuthorByName(targetUserName);

        // Only follow authors that are not already followed
        if (!author.Following.Contains(targetAuthor))
        {
            author.Following.Add(targetAuthor);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task UnfollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        var targetAuthor = await FindAuthorByName(targetUserName);
        
        // Only unfollow authors that are currently followed
        if (author.Following.Contains(targetAuthor))
        {
            author.Following.Remove(targetAuthor);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAuthor(string authorName)
    {
        var author = await FindAuthorByName(authorName);

        // Remove relationships from following list
        foreach (var followedAuthor in author.Following.ToList())
                followedAuthor.Following?.Remove(author);

        // Remove the author's cheeps
        var cheeps = _context.Cheeps.Where(c => c.Author.Name == author.Name).ToList();
        foreach (var cheep in cheeps)
        {
            _context.Cheeps.Remove(cheep);
        }

        // Remove liked cheep relationships
        foreach (var likedCheep in author.LikedCheeps.ToList())
        {
            likedCheep.LikeAmount -= 1;
            likedCheep.LikedBy?.Remove(author);
        }
        author.LikedCheeps.Clear();

        // Remove disliked cheep relationships
        foreach (var dislikedCheep in author.DislikedCheeps.ToList())
        {
            dislikedCheep.DislikeAmount -= 1;
            dislikedCheep.DislikedBy?.Remove(author);
        }
        author.DislikedCheeps.Clear();
        
        // Remove the author
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    #endregion
}
