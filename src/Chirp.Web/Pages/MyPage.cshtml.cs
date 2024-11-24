using System.ComponentModel.DataAnnotations;
using System.Text;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Razor.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class MyPageModel : SharedModel
{
    public string? AuthorName => HttpContext.GetRouteValue("author")?.ToString();
    public MyPageModel(ICheepService cheepService, IAuthorService authorService) : base(cheepService, authorService) 
    {
        
    }
    
    public override async Task<IList<CheepDto>> GetCheeps()
    {
        if (AuthorName == null) throw new Exception("Author Name is null");
        
        return await _cheepService.GetAllCheeps(AuthorName);
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
        
        if (!string.IsNullOrEmpty(author.Email)) content.AppendLine($"Email: {{author.Email}}");
        else content.AppendLine($"Email: No Email");
            
        content.AppendLine("Following:");
        if (author.Following != null)
        {
            foreach (var follow in author.Following)
            {
                content.AppendLine($"- {follow.Name}");
            }
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
