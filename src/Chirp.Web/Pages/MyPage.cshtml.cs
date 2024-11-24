using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Razor.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class MyPageModel : SharedModel
{
    public string? AuthorName => HttpContext.GetRouteValue("author")?.ToString();
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public MyPageModel(ICheepService cheepService, IAuthorService authorService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(cheepService, authorService) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public override async Task<IList<CheepDto>> GetCheeps()
    {
        if (AuthorName == null) throw new Exception("Author Name is null");
        
        return await _cheepService.GetAllCheeps(AuthorName);
    }

    public async Task<IActionResult> OnPostDeleteDataAsync()
    {
        if (AuthorName == null) throw new Exception("Author Name is null");
        await _authorService.DeleteAuthor(AuthorName);
        
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }
        }
        
        await _signInManager.SignOutAsync();
        
        Console.WriteLine("User authenticated: " + User.Identity?.IsAuthenticated.ToString());
        Console.WriteLine("User name " + GetUserName);
        return Redirect("/");
    }

    public async Task<IActionResult> OnPostDownloadInfoAsync()
    {
        if (AuthorName == null) throw new Exception("Author Name is null");
        
        Author? author = await _authorService.FindAuthorByName(AuthorName);
        if (author == null) throw new Exception("User: "+ AuthorName + " not found in database");

        // Create the text file
        var content = new StringBuilder();
        content.AppendLine($"{author.Name}'s Information");
        content.AppendLine($"--------------------------");
        
        content.AppendLine($"Name: {author.Name}");
        
        if (!string.IsNullOrEmpty(author.Email) && author.Email != " ") content.AppendLine($"Email: {author.Email}");
        else content.AppendLine($"Email: No Email");
            
        content.AppendLine("Following:");
        if (author.Following != null)
        {
            if (author.Following.Any())
            {
                foreach (var follow in author.Following)
                {
                    content.AppendLine($"- {follow.Name}");
                }
            }
            else content.AppendLine("- No Following");
        }
        else content.AppendLine($"- No Following");
            
        content.AppendLine("Cheeps:");
        if (GetCheeps != null)
        {
            foreach (var cheep in await GetCheeps())
            {
                content.AppendLine($"- \"{cheep.text}\" ({cheep.postedTime})");
            }
        }
        else content.AppendLine("- No Cheeps");
        
        // Convert content to bytes and return file
        byte[] fileBytes = Encoding.UTF8.GetBytes(content.ToString());
        string fileName = $"{author.Name}_Chirp_Data.txt";
        return File(fileBytes, "text/plain", fileName);
    }
}
