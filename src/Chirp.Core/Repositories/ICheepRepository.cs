namespace Chirp.Core;

/// <summary>
/// Interface for the Cheep Repository governing methods for the repository  .
/// </summary>
public interface ICheepRepository
{
    #region Queries

    /// <summary>
    /// Finds the paginated cheeps of the specified user. If none provided, retrieve all.
    /// Adapted from code given on the slides of session 6, p. 8.
    /// </summary>
    /// <param name="authorName">
    /// The name attribute of the author.
    /// If null get all cheeps from the database.
    /// </param>
    /// <param name="pageNumber">
    /// The page number of the current site for pagination.
    /// Starts a 1 to provide cheeps for the first page.
    /// </param>
    /// <returns>A list of cheep DTOs that belong to the specified author.</returns>
    public Task<List<CheepDto>> GetCheeps(string? authorName, int pageNumber);
    
    /// <summary>
    /// Gets the total cheeps from an author or the total amount in the database.
    /// </summary>
    /// <param name="authorName">
    /// The name attribute of the author.
    /// If null get total cheep count from the database.
    /// </param>
    /// <returns>An integer of the total amount of cheeps from an author or entirely.</returns>
    public int GetTotalCheepsCount(string? authorName);
    
    /// <summary>
    /// Retrieve all cheeps from the specified author as well as all the following authors cheeps.
    /// </summary>
    /// <param name="authorName">The name attribute of the author.</param>
    /// <param name="followers">List of all author DTOs that the specified author follows.</param>
    /// <param name="pageNumber">
    /// The page number of the current site for pagination.
    /// Starts a 1 to provide cheeps for the first page.
    /// </param>
    /// <returns>A list of all the cheeps from the specified author and all their following authors.</returns>
    public Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber);
    
    /// <summary>
    /// Finds all cheeps belonging to a specified author.
    /// </summary>
    /// <param name="authorName">The name attribute of the author.</param>
    /// <returns>A list of cheeps belonging to a specified author.</returns>
    public Task<List<CheepDto>> GetAllCheeps(string authorName);
    
    /// <summary>
    /// Retrieves the ID of a specified cheep DTO.
    /// </summary>
    /// <param name="cheepDto">The cheep of which the ID will be retrieved from.</param>
    /// <returns>The ID of a specified cheep.</returns>
    /// <exception cref="NullReferenceException">Thrown if the cheep is not found in the database from the ID.</exception>
    public Task<int> FindCheepId(CheepDto cheepDto); 
    
    /// <summary>
    /// Checks whether a cheep is liked by a specified author.
    /// </summary>
    /// <param name="authorName">The name attribute of the author.</param>
    /// <param name="cheep">The cheep which will be checked if it is liked.</param>
    /// <returns>True if the cheep is liked by the specified author, false otherwise.</returns>
    /// <exception cref="NullReferenceException">Thrown if the author is not found in the database from the name.</exception>
    public Task<bool> IsCheepLikedByAuthor(string authorName, CheepDto cheep);
    
    /// <summary>
    /// Checks whether a cheep is disliked by a specified author.
    /// </summary>
    /// <param name="authorName">The name attribute of the author.</param>
    /// <param name="cheep">The cheep which will be checked if it is disliked.</param>
    /// <returns>True if the cheep is disliked by the specified author, false otherwise.</returns>
    /// <exception cref="NullReferenceException">Thrown if the author is not found in the database from the name.</exception>
    public Task<bool> IsCheepDislikedByAuthor(string authorName, CheepDto cheep);
    
    /// <summary>
    /// Retrieves the total amount of likes from a cheep.
    /// </summary>
    /// <param name="cheepDto">The cheep which likes amount will be returned.</param>
    /// <returns>An integer of like amount.</returns>
    /// <exception cref="NullReferenceException">Thrown if the cheep cannot be found in the database.</exception>
    public Task<int> GetCheepLikesCount(CheepDto cheepDto);
    
    /// <summary>
    /// Retrieves the total amount of dislikes from a cheep.
    /// </summary>
    /// <param name="cheepDto">The cheep which dislikes amount will be returned.</param>
    /// <returns>An integer of dislike amount.</returns>
    /// <exception cref="NullReferenceException">Thrown if the cheep cannot be found in the database.</exception>
    public Task<int> GetCheepDislikesCount(CheepDto cheepDto);
    
    /// <summary>
    /// Retrieves the list of liked cheeps from a specified author.
    /// </summary>
    /// <param name="authorName">The name of the author which liked cheeps will be retrieved.</param>
    /// <returns>A list of all the authors liked cheeps.</returns>
    /// <exception cref="NullReferenceException">Thrown if the author cannot be found in the database.</exception>
    public Task<List<CheepDto>> GetLikedCheeps(string authorName);
    
    /// <summary>
    /// Retrieves the list of disliked cheeps from a specified author.
    /// </summary>
    /// <param name="authorName">The name of the author which disliked cheeps will be retrieved.</param>
    /// <returns>A list of all the authors disliked cheeps.</returns>
    /// <exception cref="NullReferenceException">Thrown if the author cannot be found in the database.</exception>
    public Task<List<CheepDto>> GetDislikedCheeps(string authorName);
    
    #endregion

    #region Commands
    
    /// <summary>
    /// Create a new cheep entity in the database from a corresponding DTO
    /// </summary>
    /// <param name="cheep">The created cheep that needs to be added to the database</param>
    /// <param name="authorName">The name attribute of the author</param>
    /// <exception cref="NullReferenceException">Thrown if the author is not found in the database from the name</exception>
    public Task CreateCheep(CheepDto cheep, string authorName);
    
    /// <summary>
    /// Finds and deletes a specified cheep from the database
    /// </summary>
    /// <param name="cheepDto">The cheep that needs to be deleted from the database</param>
    /// <exception cref="NullReferenceException">Thrown if the DTO does not match any cheep entity in the database</exception>
    public Task DeleteCheep(CheepDto cheepDto);
    
    /// <summary>
    /// Likes a cheep, creating a like relation for the cheep to a specified author
    /// </summary>
    /// <param name="authorName">The name attribute of the author that will like the cheep</param>
    /// <param name="cheepDto">The cheep that will be liked</param>
    /// <exception cref="NullReferenceException">Thrown if either the author or cheep cannot be found in the database</exception>
    public Task LikeCheep(string authorName, CheepDto cheepDto);
    
    /// <summary>
    /// Dislikes a cheep, creating a like relation for the cheep to a specified author
    /// </summary>
    /// <param name="authorName">The name attribute of the author that will dislike the cheep</param>
    /// <param name="cheepDto">The cheep that will be disliked</param>
    /// <exception cref="NullReferenceException">Thrown if either the author or cheep cannot be found in the database</exception>
    public Task DislikeCheep(string authorName, CheepDto cheepDto);
    
    /// <summary>
    /// Removes the like from a cheep by a specified user
    /// </summary>
    /// <param name="authorName">The name attribute of the user that will remove their like</param>
    /// <param name="cheepDto">The cheep that will be removed a like from</param>
    /// <exception cref="NullReferenceException">Thrown if either the author or cheep cannot be found in the database</exception>
    public Task RemoveLikeCheep(string authorName, CheepDto cheepDto);
    
    /// <summary>
    /// Removes the dislike from a cheep by a specified user
    /// </summary>
    /// <param name="authorName">The name attribute of the user that will remove their dislike</param>
    /// <param name="cheepDto">The cheep that will be removed a dislike from</param>
    /// <exception cref="NullReferenceException">Thrown if either the author or cheep cannot be found in the database</exception>
    public Task RemoveDislikeCheep(string authorName, CheepDto cheepDto);
    
    #endregion
}
