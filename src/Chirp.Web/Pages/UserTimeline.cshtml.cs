using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public List<CheepDto> Cheeps { get; set; }

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
}
