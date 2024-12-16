namespace Chirp.Core;

/// <summary>
/// Interface for the author repository governing methods for the repository acting as a contract.
/// </summary>
public interface IAuthorRepository
{
    #region Queries
    
    /// <summary>
    /// Finds an Author entity from its corresponding name.
    /// </summary>
    /// <param name="name">The name attribute of the author.</param>
    /// <returns>Returns the author found in the database, otherwise null.</returns>
    /// <exception cref="NullReferenceException">Thrown if the author is not found in the database from the name.</exception>
    public Task<Author> FindAuthorByName(string name);
    
    /// <summary>
    /// Finds an Author entity from its corresponding ID.
    /// </summary>
    /// <param name="id">The id property of the author.</param>
    /// <returns>Returns the author found in the database, otherwise null.</returns>
    /// <exception cref="NullReferenceException">Thrown if the author is not found in the database from the name.</exception>
    public Task<Author> GetAuthorById (int id);
    
    /// <summary>
    /// Determines whether the user is following the target author.
    /// </summary>
    /// <param name="userName">The name of the user currently logged in.</param>
    /// <param name="targetUserName">The name of the author that the user wants to follow.</param>
    /// <returns>True if the user follows the target author, false otherwise.</returns>
    public Task<bool> IsFollowing(string userName, string targetUserName);
    
    /// <summary>
    /// Finds the list of followers of a specified author.
    /// </summary>
    /// <param name="authorName">The name attribute of the author.</param>
    /// <returns>A list of author DTOs that a specified author follows.</returns>
    public Task<IList<AuthorDto>> GetFollowers(string authorName);
    
    #endregion
    
    #region Commands
    
    /// <summary>
    /// Creates a new author if it does not already exist.
    /// </summary>
    /// <param name="authorDto">Data transfer object of an author from the front-end.</param>
    public Task CreateAuthor(AuthorDto authorDto);
    
    /// <summary>
    /// Creates a following relationship between the current user and the target author.
    /// </summary>
    /// <param name="userName">The name of the user currently logged in.</param>
    /// <param name="targetUserName">The name of the author that the user wants to follow.</param>
    public Task FollowAuthor(string userName, string targetUserName);
    
    /// <summary>
    /// Removes the following relationship between the current user and the target author.
    /// </summary>
    /// <param name="userName">The name of the user currently logged in.</param>
    /// <param name="targetUserName">The name of the author that the user wants to follow.</param>
    public Task UnfollowAuthor(string userName, string targetUserName);
    
    /// <summary>
    /// Finds and deletes a specified author.
    /// </summary>
    /// <param name="authorName">The name attribute of the author.</param>
    public Task DeleteAuthor(string authorName);
 
    #endregion
}