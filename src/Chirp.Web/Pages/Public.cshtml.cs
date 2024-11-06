using System.ComponentModel.DataAnnotations;
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

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model is invalid");
            return Page(); // Show page with previously entered data and error markers
            
        }
        
            try
            {
                Console.WriteLine("Trying to find author with name: " + User.Identity.Name);
                await _service.FindAuthorByName(User.Identity.Name);
                Console.WriteLine("Found User");
            }
            catch
            {
                Console.WriteLine("Did not found user");
                await _service.CreateAuthor(new Author
                {
                    Name = User.Identity.Name,
                    Email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value,
                    Cheeps = new List<Cheep>()
                });
                Console.WriteLine("Created User");
            }

            try
            {
                Console.WriteLine("Creating Cheep");
                Cheep cheep = new Cheep
                {
                    Text = Message,
                    Author = await _service.FindAuthorByName(User.Identity.Name)
                };
                
                Author author3 = new Author() { AuthorId = 15, Name = " Amalia testPerson", Email = "Amalia+tpe@hotmail.com", Cheeps = new List<Cheep>() };
                var cheep2 = new Cheep() { CheepId = 706, AuthorId = author3.AuthorId, Author = author3, Text = "Test string TP", TimeStamp = DateTime.Parse("2023-08-02 13:16:22") };
                
                Console.WriteLine("Trying to send cheep with message: " + cheep2.Text);
                await _service.CreateCheep(cheep2);
                Console.WriteLine("Created Cheep");
                return Redirect(User.Identity.Name);
            }
            catch
            {
                Console.WriteLine("Did not create Cheep");
                return Redirect("/");
            }
        
    }
}
