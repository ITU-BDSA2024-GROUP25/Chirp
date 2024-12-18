using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

/// <summary>
/// Represents the functionality for the UserTimeline page also known as the Private Timeline page.
/// Specifies which cheeps to be shown.
/// </summary>
public class UserTimelineModel : SharedModel
{
    public UserTimelineModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) { }
    
    /// <summary>
    /// Retrieves the cheeps from a specified author.
    /// If it is the users own page, also show their following author's cheeps.
    /// </summary>
    /// <returns>A list of cheeps</returns>
    public override async Task<IList<CheepDto>> GetCheeps()
    {
        var authorName = HttpContext.GetRouteValue("author")?.ToString();

        // If it is the users own account, followed cheeps should also be shown
        if (authorName == GetUserName)
        {
            var followers = await AuthorService.GetFollowers(authorName);
            
            CheepAmount = CheepService.GetTotalCheepsCount(authorName);
            foreach (var follower in followers)
            {
                CheepAmount += CheepService.GetTotalCheepsCount(follower.userName);
            }
            
            return await CheepService.GetCheepsFromFollowers(authorName, followers, CurrentPage);
        }
        
        // Otherwise fetch only that author's cheeps
        CheepAmount = CheepService.GetTotalCheepsCount(authorName);
        return await CheepService.GetCheeps(authorName, CurrentPage);
    }
}
