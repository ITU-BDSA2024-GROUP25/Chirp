using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Razor.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Chirp.Web.Pages;

public class PublicModel : SharedModel
{
    public PublicModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) 
    {

    }

    public override async Task<IList<CheepDto>> GetCheeps()
    {
        CheepAmount = _cheepService.GetTotalCheepsCount(null);
        return await _cheepService.GetCheeps(null);
    }
}
