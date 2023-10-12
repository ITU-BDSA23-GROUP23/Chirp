using Chirp.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        //Cheeps = _service.GetCheeps(page);
        //Cheeps = _service.GetCheeps(author);
        Cheeps = _service.GetCheepsFromAuthor(author, page);
        return Page();
    }
}
