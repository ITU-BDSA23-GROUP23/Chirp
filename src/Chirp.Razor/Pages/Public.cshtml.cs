using Chirp.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly CheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public PublicModel(CheepService service)
    {
        _service = service;
        Cheeps = service.GetCheeps();
    }

    public ActionResult OnGet()
    {
        Cheeps = _service.GetCheeps();
        return Page();
    }
}
