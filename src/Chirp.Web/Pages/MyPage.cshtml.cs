using System.Text;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

/// <summary>
/// Represents the functionality for the My-Page page.
/// Specifies which cheeps to be shown.
/// Extended functionality: Deleting user and downloading user data.
/// </summary>
public class MyPageModel : SharedModel
{
    # region Fields
    
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private string AuthorName => HttpContext.GetRouteValue("author")?.ToString() 
                                 ?? throw new NullReferenceException("Author name could not be found");
    
    # endregion
    
    # region Constructor
    
    public MyPageModel(ICheepService cheepService, IAuthorService authorService, UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager) : base(cheepService, authorService) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    # endregion

    # region Queries
    
    public override async Task<IList<CheepDto>> GetCheeps() => await CheepService.GetAllCheeps(AuthorName);
    public async Task<IList<CheepDto>> GetLikedCheeps() => await CheepService.GetLikedCheeps(AuthorName);
    public async Task<IList<CheepDto>> GetDislikedCheeps() => await CheepService.GetDislikedCheeps(AuthorName);
    
    #endregion
    

    # region Metods
    
    /// <summary>
    /// Deletes the user from both the identity and as an author in the database.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when if the user manager fails to delete the user.</exception>
    public async Task<IActionResult> OnPostDeleteDataAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred deleting user.");
            }
            
            await AuthorService.DeleteAuthor(AuthorName);
        }
        
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }

    /// <summary>
    /// Creates and returns a text file with all data stored about the author. 
    /// </summary>
    public async Task<IActionResult> OnPostDownloadInfoAsync()
    {
        var author = await AuthorService.FindAuthorByName(AuthorName);

        // Create the text file
        var content = new StringBuilder();
        content.AppendLine($"{author.Name}'s Information");
        content.AppendLine($"--------------------------");
        
        content.AppendLine($"Name: {author.Name}");
        
        if (!string.IsNullOrEmpty(author.Email) && author.Email != " ") content.AppendLine($"Email: {author.Email}");
        else content.AppendLine($"Email: No Email");
        content.AppendLine("");
            
        content.AppendLine("Following:");
        if (author.Following.Any())
        {
           foreach (var follow in author.Following)
           {
               content.AppendLine($"- {follow.Name}"); 
                    
           }
        }
        else content.AppendLine("- No Following");
        
        content.AppendLine("");    
        
        content.AppendLine("Cheeps:");
        if (GetCheeps().Result.Any())
        {
            foreach (var cheep in await GetCheeps())
            {
                content.AppendLine($"- \"{cheep.text}\" ({cheep.postedTime})");
                content.AppendLine($"  Likes: {await CheepLikesAmount(cheep)} Dislikes: {await CheepDislikeAmount(cheep)}");
            }
        }
        else content.AppendLine("- No Cheeps");
        
        content.AppendLine("");
        
        content.AppendLine("Liked Cheeps:");
        var likedCheeps = await GetLikedCheeps();
        
        if (likedCheeps.Any())
        {
            foreach (var cheep in likedCheeps)
            {
                content.AppendLine($"- {cheep.authorName}: \"{cheep.text}\" ({cheep.postedTime})");
                content.AppendLine($"  Likes: {await CheepLikesAmount(cheep)} Dislikes: {await CheepDislikeAmount(cheep)}");
            }
        }
        else content.AppendLine("- No Liked Cheeps");
        
        content.AppendLine("");
        
        content.AppendLine("Disliked Cheeps:");
        var dislikedCheeps = await GetDislikedCheeps();
        
        if (dislikedCheeps.Any())
        {
            foreach (var cheep in dislikedCheeps)
            {
                content.AppendLine($"- {cheep.authorName}: \"{cheep.text}\" ({cheep.postedTime})");
                content.AppendLine($"  Likes: {await CheepLikesAmount(cheep)} Dislikes: {await CheepDislikeAmount(cheep)}");
            }
        }
        else content.AppendLine("- No Disliked Cheeps");
        
        
        // Convert content to bytes and return file
        byte[] fileBytes = Encoding.UTF8.GetBytes(content.ToString());
        string fileName = $"{author.Name}_Chirp_Data.txt";
        return File(fileBytes, "text/plain", fileName);
    }
    
    #endregion
}
