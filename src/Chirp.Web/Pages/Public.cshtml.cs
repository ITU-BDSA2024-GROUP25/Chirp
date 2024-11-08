using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
        _service.CurrentPage = CurrentPage;
        Cheeps = new List<CheepDto>();
        Message = string.Empty;
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    [BindProperty]
    [Required]
    [MinLength(2, ErrorMessage = "hey man, its too short, needs to be longer than {1}")]
    [MaxLength(160, ErrorMessage = "dude nobody will read all that, max length is {1}")]
    public string Message { get; set; }

    public async Task<ActionResult> OnGet([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;

        int totalCheeps = _service.GetTotalCheepsCount(null);
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)32);

        // Fetch the cheeps for the requested page
        Cheeps = await _service.GetCheeps(null);

        return Page();
    }

    // code given from groupe number 3 
    public IActionResult OnGetLogin()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = "/signin-github"
        }, "GitHub");
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
            await _service.FindAuthorByName(GetUserName);
        }
        catch
        {
            await _service.CreateAuthor(new Author 
            {
                Name = GetUserName,
                Email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value ?? string.Empty,
                Cheeps = new List<Cheep>()
            });
        }

        try
        {
            CheepDto cheep = new CheepDto(Message, DateTime.UtcNow.ToString(), GetUserName);
            await _service.CreateCheep(cheep);
                
            return Redirect(GetUserName);
        }
        catch
        {
            return Redirect("/");
        }
    }
}
