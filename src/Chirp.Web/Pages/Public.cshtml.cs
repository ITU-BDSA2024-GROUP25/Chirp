using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    [BindProperty]
    public string Text { get; set; }

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

    public async Task<IActionResult> OnPost(string Message)
    {
        try
        {
            _service.FindAuthorByName(User.Identity.Name);
        }
        catch
        {
            await _service.CreateAuthor(new Author
            {
                Name = User.Identity.Name,
                Email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value,
                Cheeps = new List<Cheep>()
            });
        }

        try {
            Cheep cheep = new Cheep
            {
                Text = Message,
                Author =  await _service.FindAuthorByName(User.Identity.Name)
            };
            await _service.CreateCheep(cheep);
            return Redirect(User.Identity.Name);
        }
        catch {
            return Redirect("/");
        }
    }
}
