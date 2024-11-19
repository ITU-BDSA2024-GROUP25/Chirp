using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Razor.Pages;
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
        var cheeps = new List<CheepDto>();
        
        CheepAmount = _cheepService.GetTotalCheepsCount(authorName);
        
        var userCheeps = await _cheepService.GetCheeps(authorName, CurrentPage);
        cheeps.AddRange(userCheeps);

        // If it is the users own account, followed cheeps should also be shown
        if (authorName == GetUserName)
        {
            var followers = await _authorService.GetFollowers(GetUserName);

            foreach (var follower in followers)
            {
                CheepAmount += _cheepService.GetTotalCheepsCount(follower.userName);
                cheeps.AddRange(await _cheepService.GetCheeps(follower.userName, CurrentPage));
            }
        }

        return cheeps;
    }
}
