using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public Task<List<Cheep>>  Cheeps { get; set; }
    public List<Cheep> CheepList { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
        _service.CurrentPage = CurrentPage;
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public ActionResult OnGet([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;        

        int totalCheeps = _service.GetTotalCheepsCount();
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)32);

        // Fetch the cheeps for the requested page
        CheepList = _service.GetCheeps();

        return Page();
    }
}
