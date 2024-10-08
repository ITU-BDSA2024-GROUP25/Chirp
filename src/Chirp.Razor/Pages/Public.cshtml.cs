﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDto>  Cheeps { get; set; }
    
    public PublicModel(ICheepService service)
    {
        _service = service;
        _service.CurrentPage = CurrentPage;
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public async Task<ActionResult> OnGet([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;        

        int totalCheeps = _service.GetTotalCheepsCount(null);
        TotalPages = (int)Math.Ceiling(totalCheeps / (double)32);

        // Fetch the cheeps for the requested page
        Cheeps = await _service.GetCheeps(null);

        return Page();
    }
}
