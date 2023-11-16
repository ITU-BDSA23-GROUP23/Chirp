
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using AspNetCore;

namespace Chirp.Web.Pages.Shared;

//Model for page navigation needs to be instanced in other models
public class PageNavModel : PageModel
{

    private readonly ICheepService _service;

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; } = 1;

    public PageNavModel(ICheepService service, int page, int maxPages)
    {
        _service = service;
        TotalPages = maxPages;

        if (page > 1) {
            CurrentPage = page;
        } else {
            CurrentPage = 1;
        }
    }

    public string GetPageString(int mod = 0) {
        var _page = CurrentPage + mod;
        //Workaround that disables incrementing the page when it is on the last page
        if (_page > TotalPages) {
            _page = CurrentPage;
        }
        return "?page=" + _page;
    }

    public ActionResult OnGetPartial()
    {
        return Page();
    }
}
