using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDto>  Cheeps { get; set; }
    
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
    
    public ActionResult OnPost(string Message)
    {
        Author author = new Author
        {
            Name = User.Identity.Name,
            Email = User.Identity.Name + "@gmail.com",
            Cheeps = new List<Cheep>()
        };
        
        Cheep cheep = new Cheep
        {
            Text = Message,
            Author = author
        };
        
        _service.CreateCheep(cheep);
        
        return RedirectToPage("Public"); // it is good practice to redirect the user after a post request
    }
}
