using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Razor.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class MyPageModel : SharedModel
{
    public MyPageModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) 
    {
        
    }
    
    public override async Task<IList<CheepDto>> GetCheeps()
    {
        var authorName = HttpContext.GetRouteValue("author")?.ToString();

        return await _cheepService.GetAllCheeps(authorName);
    }
}
