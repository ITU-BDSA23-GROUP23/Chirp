using Chirp.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly CheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public UserTimelineModel(CheepService service)
    {
        _service = service;
        Cheeps = service.GetCheeps();
    }

    public ActionResult OnGet(string author)
    {
        //Cheeps = _service.GetCheepsFromAuthor(author);
        return Page();
    }
}
