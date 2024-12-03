using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : SharedModel
{
    public UserTimelineModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) 
    {

    }
    
    public override async Task<IList<CheepDto>> GetCheeps()
    {
        var authorName = HttpContext.GetRouteValue("author")?.ToString();

        // If it is the users own account, followed cheeps should also be shown
        if (authorName == GetUserName)
        {
            var followers = await _authorService.GetFollowers(authorName);
            
            CheepAmount = _cheepService.GetTotalCheepsCount(authorName);
            foreach (var follower in followers)
            {
                CheepAmount += _cheepService.GetTotalCheepsCount(follower.userName);
            }
            
            return await _cheepService.GetCheepsFromFollowers(authorName, followers, CurrentPage);
        }
        else
        {
            CheepAmount = _cheepService.GetTotalCheepsCount(authorName);
            return await _cheepService.GetCheeps(authorName, CurrentPage);
        }
    }
}
