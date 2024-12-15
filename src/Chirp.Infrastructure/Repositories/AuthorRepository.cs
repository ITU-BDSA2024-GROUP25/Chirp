using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

/// <summary>
/// This class provides CRUD operation used on the author entity and managing database relationships
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDbContext _context;
    
    /// <summary>
    /// Initializes a new instance of the AuthorRepository class with a specified database context
    /// </summary>
    /// <param name="context">Is used to access the specified database</param>
    public AuthorRepository(ChirpDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new author if it does not already exist
    /// </summary>
    /// <param name="authorDto">Data transfer object of an author from the front-end</param>
    /// <returns>Asynchronous operation</returns>
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

            // Set mail as empty if none is provided (In case of GitHub login) 
            if (string.IsNullOrEmpty(author.Email)) author.Email = " ";   
            
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }
    }
    
    /// <summary>
    /// Finds an Author entity from its corresponding name
    /// </summary>
    /// <param name="name">The name attribute of the author</param>
    /// <returns>Returns the author found in the database, otherwise null</returns>
    /// <exception cref="Exception">Thrown if the author is not found in the database from the name</exception>
    public async Task<Author> FindAuthorByName(string name)
    {
        var author = await _context.Authors
            .Include(a => a.Following)
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();
        
        if (author == null) throw new NullReferenceException("Author not found");
        return author;
    }
    
    /// <summary>
    /// Finds an Author entity from its corresponding Id
    /// </summary>
    /// <param name="id">The id property of the author</param>
    /// <returns>Returns the author found in the database, otherwise null</returns>
    /// <exception cref="Exception">Thrown if the author is not found in the database from the id</exception>
    public async Task<Author> GetAuthorById(int id)
    {
        var author = await _context.Authors
            .Where(a => a.AuthorId == id)
            .FirstOrDefaultAsync();
        
        if (author == null) throw new NullReferenceException("Author not found");
        return author;
    }

    /// <summary>
    /// Creates a following relationship between the current user and the target author
    /// </summary>
    /// <param name="userName">The name of the user currently logged in</param>
    /// <param name="targetUserName">The name of the author that the user wants to follow</param>
    public async Task FollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        var targetAuthor = await FindAuthorByName(targetUserName);

        // Instantiate a list of following author if the author does not have one
        author.Following ??= new List<Author>();
        
        // Only follow authors that are not followed already
        if (!author.Following.Contains(targetAuthor))
        {
            author.Following.Add(targetAuthor);
            await _context.SaveChangesAsync(); 
        }
    }
    
    /// <summary>
    /// Removes the following relationship between the current user and the target author
    /// </summary>
    /// <param name="userName">The name of the user currently logged in</param>
    /// <param name="targetUserName">The name of the author that the user wants to follow</param>
    public async Task UnfollowAuthor(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        var targetAuthor = await FindAuthorByName(targetUserName);

        if (author.Following == null) throw new Exception(userName +"'s following list is null");
        
        // Only unfollow authors that are followed
        if (author.Following.Contains(targetAuthor))
        {
            author.Following.Remove(targetAuthor);
            await _context.SaveChangesAsync(); 
        }
    }

    /// <summary>
    /// Determines whether the user is following the target author
    /// </summary>
    /// <param name="userName">The name of the user currently logged in</param>
    /// <param name="targetUserName">The name of the author that the user wants to follow</param>
    /// <returns>True if the user follows the target author, false otherwise</returns>
    public async Task<bool> IsFollowing(string userName, string targetUserName)
    {
        var author = await FindAuthorByName(userName);
        var targetAuthor = await FindAuthorByName(targetUserName);
        
        // Return false if the users following list is not instantiated, meaning it is empty
        return author.Following?.Contains(targetAuthor) ?? false;
    }

    /// <summary>
    /// Finds the list of followers of a specified author
    /// </summary>
    /// <param name="authorName">The name attribute of the author</param>
    /// <returns>A list of author DTOs that a specified author follows</returns>
    public async Task<IList<AuthorDto>> GetFollowers(string authorName)
    {
        var author = await FindAuthorByName(authorName);
        
        // Return empty list if the users following list is not instantiated, meaning it is empty
        author.Following ??= new List<Author>();

        return author.Following
            .Select(follower => new AuthorDto(follower.Name, follower.Email))
            .ToList();   
    }

    /// <summary>
    /// Finds and deletes a specified author
    /// </summary>
    /// <param name="authorName">The name attribute of the author</param>
    public async Task DeleteAuthor(string authorName)
    {
        var author = await FindAuthorByName(authorName);
        
        // Remove each author following relation
        if (author.Following != null)
        {
            foreach (var followedAuthor in author.Following.ToList())
                followedAuthor.Following?.Remove(author);
        }

        // Remove each cheep of the author's cheeps
        var cheeps = _context.Cheeps.Where(c => c.Author.Name == author.Name).ToList();
        foreach (var cheep in cheeps)
        {
            _context.Cheeps.Remove(cheep);
        }

        // Remove each liked cheep relation
        if (author.LikedCheeps != null)
        {
            foreach (var likedCheep in author.LikedCheeps.ToList())
            {
                likedCheep.LikeAmount -= 1;
                likedCheep.LikedBy?.Remove(author);
            }
            author.LikedCheeps.Clear();
        }

        // Remove each disliked cheep relation
        if (author.DislikedCheeps != null)
        {
            foreach (var dislikedCheep in author.DislikedCheeps.ToList())
            {
                dislikedCheep.DislikeAmount -= 1;
                dislikedCheep.DislikedBy?.Remove(author);
            }
            author.DislikedCheeps.Clear();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}