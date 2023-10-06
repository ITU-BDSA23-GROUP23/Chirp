﻿using Chirp.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public PublicModel(ICheepService service, int page)
    {
        _service = service;
        Cheeps = service.GetCheeps(page);
    }

    public ActionResult OnGet(int page)
    {
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
}
