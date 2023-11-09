
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Chirp.Web.Pages.Shared;
[AllowAnonymous]
public class PageNavModel : PageModel
{

    private readonly ICheepService _service;

    public int TotalPages { get; set; }

    public PublicModel(ICheepService service)
    { 
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int page)
    {

        var _TotalPages = _service.GetPageAmount();
        _TotalPages.Wait();
        TotalPages = _TotalPages.Result;

        return Page();
    }
