using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

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
        var _Cheeps = _service.GetCheepsFromAuthor(author, page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;
        return Page();
    }
}
