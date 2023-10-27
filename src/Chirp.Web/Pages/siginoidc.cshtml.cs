using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class siginoidc : PageModel
{
    private readonly ICheepService _service;
    private readonly ILogger<siginoidc> _logger;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }

    public siginoidc(ILogger<siginoidc> logger)
        {
            _logger = logger;
        }

    public siginoidc(ICheepService service)
    {
        _service = service;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet([FromQuery] int page)
    {
        var _Cheeps = _service.GetCheeps(page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;
        return Page();
    }
}