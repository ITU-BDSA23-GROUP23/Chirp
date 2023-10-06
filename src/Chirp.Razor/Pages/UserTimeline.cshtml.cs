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
        Cheeps = service.GetCheeps();
    }

    public ActionResult OnGet(int author)
    {
        Cheeps = _service.GetCheepsFromAuthor(author);
        return Page();
    }
}
