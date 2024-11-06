using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public List<CheepDto> Cheeps { get; set; }
    
    [BindProperty]
    [Required]
    [MinLength(2, ErrorMessage = "hey man, its too short, needs to be longer than {1}")]
    [MaxLength(160, ErrorMessage = "dude nobody will read all that, max length is {1}")]
    public string Message { get; set; }
    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        Cheeps = new List<CheepDto>();
    }

    public async Task<ActionResult> OnGet(string author, [FromQuery] int? page)
    {
        CurrentPage = page ?? 1;        

        int totalCheeps = _service.GetTotalCheepsCount(author);
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)32);

        Cheeps = await _service.GetCheeps(author);

        return Page();
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
            await _service.FindAuthorByName(User.Identity.Name);
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

        try
        {
            CheepDto cheep = new CheepDto(Message, DateTime.UtcNow.ToString(), User.Identity.Name);
            await _service.CreateCheep(cheep);
                
            return Redirect(User.Identity.Name);
        }
        catch
        {
            return Redirect("/");
        }
    }
}
