using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private const int PageSize = 10;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public ActionResult OnGet(int? page)
    {
        CurrentPage = page ?? 1;        

        int totalCheeps = _service.GetTotalCheepsCount();
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)PageSize);

        // Fetch the cheeps for the requested page
        Cheeps = _service.GetCheeps(CurrentPage, PageSize);

        return Page();
    }
}
