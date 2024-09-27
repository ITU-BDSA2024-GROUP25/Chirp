using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }
    private const int PageSize = 10;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, [FromQuery] int? page)
    {
        CurrentPage = page ?? 1;        

        int totalCheeps = _service.GetTotalCheepsCountFromAuthor(author);
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)PageSize);

        Cheeps = _service.GetCheepsFromAuthor(author, CurrentPage, PageSize);

        return Page();
    }
}
