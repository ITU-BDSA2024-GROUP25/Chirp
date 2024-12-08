﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;

namespace Chirp.Web.Pages;

public abstract class SharedModel : PageModel
{
    public SharedModel(ICheepService cheepService, IAuthorService authorService)
    {
        _cheepService = cheepService;
        _authorService = authorService;
        
        Cheeps = new List<CheepDto>();
        
        //_cheepService.CurrentPage = CurrentPage;
        Message = string.Empty;
    }

    public void CreateAuthor()
    {
        if (User != null)
        {
            if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated) _authorService.CreateAuthor(new AuthorDto(GetUserName, GetUserEmail));
            }
        }
    }
    
    protected readonly ICheepService _cheepService;
    protected readonly IAuthorService _authorService;
    public IList<CheepDto> Cheeps { get; set; }
    public async Task<IList<AuthorDto>> Followers() => await _authorService.GetFollowers(GetUserName);
    public int CurrentPage { get; set; }

    public int CheepAmount { get; set; }
    public int TotalPages { get; set; }

    [BindProperty]
    [Required]
    [MinLength(2, ErrorMessage = "hey man, its too short, needs to be longer than {1}")]
    [MaxLength(160, ErrorMessage = "dude nobody will read all that, max length is {1}")]
    public string Message { get; set; }
    public string GetUserName => User.Identity?.Name ?? string.Empty;
    public string GetUserEmail => User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    public abstract Task<IList<CheepDto>> GetCheeps(); 
    
    public async Task<ActionResult> OnGet([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        
        // Fetch the cheeps for the requested page
        Cheeps = await GetCheeps();
        
        // Fetch amount of cheeps to decide ammount of pages
        TotalPages = (int)Math.Ceiling(CheepAmount / (double)32);
        
        return Page();
    }
    
    public async Task<bool> IsFollowing(string? targetAuthorName)
    {
        var userName = GetUserName;
        bool isFollowing = await _authorService.IsFollowing(userName, targetAuthorName);
        return isFollowing;
    }

    public async Task<IActionResult> OnPostFollow(string authorName)
    {
        if (string.IsNullOrEmpty(authorName))
        {
            ModelState.AddModelError(string.Empty, "Author name cannot be empty.");
            return Page();
        }

        try
        {
            await _authorService.FollowAuthor(GetUserName, authorName);
        }
        catch (Exception ex)
        {
            // Log exception and handle errors
            Console.WriteLine($"Error following author: {ex.Message}");
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName)
    {
        if (string.IsNullOrEmpty(authorName))
        {
            ModelState.AddModelError(string.Empty, "Author name cannot be empty.");
            return Page();
        }

        try
        {
            await _authorService.UnfollowAuthor(GetUserName, authorName);
        }
        catch (Exception ex)
        {
            // Log exception and handle errors
            Console.WriteLine($"Error unfollowing author: {ex.Message}");
        }

        return RedirectToPage();
    }


    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model is invalid");
            return Page(); // Show page with previously entered data and error markers
        }
        
        try
        {
            await _authorService.FindAuthorByName(GetUserName);
        }
        catch
        {
            string mail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value ?? string.Empty;
            AuthorDto author = new AuthorDto(GetUserName, mail);
            await _authorService.CreateAuthor(author);
        }

        try
        {
            CheepDto cheep = new CheepDto(Message, DateTime.UtcNow.ToString(), GetUserName);
            await _cheepService.CreateCheep(cheep, GetUserName);
                
            return Redirect(GetUserName);
        }
        catch
        {
            return Redirect("/");
        }
    }

    public async Task<IActionResult> OnPostDelete(string cheepText, string time)
    {
        CheepDto cheep = new CheepDto(cheepText, time, GetUserName);
        Console.WriteLine("cheep text: " + cheepText + " time: " + time + " name: " + GetUserName);

        try
        {
            await _cheepService.DeleteCheep(cheep);
            return RedirectToPage();
        }
        catch
        {
            return Redirect("/");
        }
    }
}
