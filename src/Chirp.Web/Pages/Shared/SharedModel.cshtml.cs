using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Web.Pages;

/// <summary>
/// Provides shared behavior and properties code between all Razor pages
/// </summary>
public abstract class SharedModel : PageModel
{
    #region Fields

    protected readonly ICheepService CheepService;
    protected readonly IAuthorService AuthorService;

    #endregion

    #region Properties

    public IList<CheepDto> Cheeps { get; set; }
    public int CurrentPage { get; set; }
    public int CheepAmount { get; set; }
    public int TotalPages { get; set; }

    [BindProperty]
    [Required]
    [MinLength(2, ErrorMessage = "Cheep is too short, needs to contain more than {1} characters")]
    [MaxLength(160, ErrorMessage = "Cheep is too long, needs to contain less than {1} characters")]
    public string Message { get; set; }

    public string GetUserName => User.Identity?.Name ?? string.Empty;
    public string GetUserEmail => User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    #endregion

    #region Class Setup

    /// <summary>
    /// Initializes the <see cref="SharedModel"/> with the required services.
    /// </summary>
    protected SharedModel(ICheepService cheepService, IAuthorService authorService)
    {
        CheepService = cheepService;
        AuthorService = authorService;

        Cheeps = new List<CheepDto>();
        Message = string.Empty;
    }

    /// <summary>
    /// Retrieves the cheeps to display on the current page.
    /// Each page has own implementation of how and which cheeps to get.
    /// </summary>
    public abstract Task<IList<CheepDto>> GetCheeps();

    #endregion

    #region Page Lifecycle
    
    /// <summary>
    /// Handles the GET HTTP request to load pages and updates the cheeps and page number. 
    /// </summary>
    /// <param name="pageNumber">The page number of what page to display. Defaults to 1 if none is given.</param>
    /// <returns>A task representing an asynchronous operation.</returns>
    public async Task<ActionResult> OnGet([FromQuery] int? pageNumber)
    {
        CurrentPage = pageNumber ?? 1;

        Cheeps = await GetCheeps();
        TotalPages = (int)Math.Ceiling(CheepAmount / 32.0);

        return Page();
    }
    
    #endregion
    
    #region Queries

    public async Task<IList<AuthorDto>> Followers() => await AuthorService.GetFollowers(GetUserName);

    public async Task<bool> IsFollowing(string? targetAuthorName)
    {
        if (targetAuthorName == null) throw new ArgumentNullException(nameof(targetAuthorName));
        return await AuthorService.IsFollowing(GetUserName, targetAuthorName);
    } 
    public async Task<int> CheepLikesAmount(CheepDto cheep) => await CheepService.GetCheepLikesCount(cheep);
    public async Task<int> CheepDislikeAmount(CheepDto cheep) => await CheepService.GetCheepDislikesCount(cheep);
    public async Task<bool> IsCheepLikedByAuthor(CheepDto cheep) => await CheepService.IsCheepLikedByAuthor(GetUserName, cheep);
    public async Task<bool> IsCheepDislikedByAuthor(CheepDto cheep) => await CheepService.IsCheepDislikedByAuthor(GetUserName, cheep);

    #endregion

    #region Commands

    public void CreateAuthor()
    {
        if (User.Identity is { IsAuthenticated: true })
            AuthorService.CreateAuthor(new AuthorDto(GetUserName, GetUserEmail));
    }
    
    public async Task<IActionResult> OnPostFollow(string authorName)
    {
        await AuthorService.FollowAuthor(GetUserName, authorName);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName)
    {
        await AuthorService.UnfollowAuthor(GetUserName, authorName);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            Cheeps = await GetCheeps();
            return Page();
        }
        
        await AuthorService.CreateAuthor(new AuthorDto(GetUserName, GetUserEmail));
        await CheepService.CreateCheep(new CheepDto(Message, DateTime.UtcNow.ToString(CultureInfo.CurrentCulture), GetUserName), GetUserName);
        
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDelete(string cheepText, string time)
    {
        await CheepService.DeleteCheep(new CheepDto(cheepText, time, GetUserName));
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLikeCheep(CheepDto cheep)
    {
        await CheepService.LikeCheep(GetUserName, cheep);
        
        // Author can not like and dislike a cheep at the same time
        if (await IsCheepDislikedByAuthor(cheep)) await CheepService.RemoveDislikeCheep(GetUserName, cheep);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveLikeCheep(CheepDto cheep)
    {
        await CheepService.RemoveLikeCheep(GetUserName, cheep);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDislikeCheep(CheepDto cheep)
    {
        await CheepService.DislikeCheep(GetUserName, cheep);
        
        // Author can not dislike and like a cheep at the same time
        if (await IsCheepLikedByAuthor(cheep)) await CheepService.RemoveLikeCheep(GetUserName, cheep);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveDislikeCheep(CheepDto cheep)
    {
        await CheepService.RemoveDislikeCheep(GetUserName, cheep);
        return RedirectToPage();
    }

    #endregion
}
