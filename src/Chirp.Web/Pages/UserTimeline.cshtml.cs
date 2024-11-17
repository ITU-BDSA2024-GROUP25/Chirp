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
        var followers = await _authorService.GetFollowers(GetUserName);

        var allCheeps = new List<CheepDto>();

        // Add Authors own cheeps
        allCheeps.AddRange(await _cheepService.GetCheeps(GetUserName)); 

        // Add Authors followers cheeps
        foreach (var follower in followers)
        {
            allCheeps.AddRange(await _cheepService.GetCheeps(follower.userName)); 
        }

        return allCheeps;
    }
}
