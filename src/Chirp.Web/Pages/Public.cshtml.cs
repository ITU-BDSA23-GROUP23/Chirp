﻿using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet([FromQuery] int page)
    {
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
}