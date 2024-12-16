using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Chirp.Web.Pages;

/// <summary>
/// Represents the functionality for the Public Timeline page.
/// Specifies which cheeps to be shown.
/// </summary>
public class PublicModel : SharedModel
{
    public PublicModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) { }

    /// <summary>
    /// Retrieves all cheeps in the database paginated.
    /// </summary>
    /// <returns>A list of paginated cheeps</returns>
    public override async Task<IList<CheepDto>> GetCheeps()
    {
        CheepAmount = CheepService.GetTotalCheepsCount(null);
        return await CheepService.GetCheeps(null, CurrentPage);
    }
}
