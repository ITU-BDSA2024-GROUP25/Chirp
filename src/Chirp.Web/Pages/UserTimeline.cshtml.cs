using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _cheepService;
    private readonly IAuthorService _authorService;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public List<CheepDto> Cheeps { get; set; }
    
    [BindProperty]
    [Required]
    [MinLength(2, ErrorMessage = "hey man, its too short, needs to be longer than {1}")]
    [MaxLength(160, ErrorMessage = "dude nobody will read all that, max length is {1}")]
    public string Message { get; set; }
    public UserTimelineModel(ICheepService cheepService, IAuthorService authorService)
    {
        _cheepService = cheepService;
        _authorService = authorService;
        
        Cheeps = new List<CheepDto>();
        Message = string.Empty;
    }

    public async Task<ActionResult> OnGet(string author, [FromQuery] int? page)
    {

        if (author.Equals(User.Identity.Name))
        {
            
        }
        
        CurrentPage = page ?? 1;        

        int totalCheeps = _cheepService.GetTotalCheepsCount(author);
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)32);

        Cheeps = await _cheepService.GetCheeps(author);

        return Page();
    }
    public string GetUserName => User.Identity?.Name ?? string.Empty;

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
}
