using Chirp.Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

   public UserTimelineModel(ICheepService service)
    {
        _service = service;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet(int author, [FromQuery] int page)
    {
        Cheeps = _service.GetCheeps(page);
        //Cheeps = _service.GetCheeps(author);
       // Cheeps = _service.GetCheepsFromAuthor(author);
        return Page();
    }
}
