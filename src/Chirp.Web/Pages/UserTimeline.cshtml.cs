using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Chirp.Web.Pages.Shared;

namespace Chirp.Web.Pages;
[AllowAnonymous]
public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }

    public int TotalPages { get; set; }

    public PageNavModel PageNav;

    private readonly ILogger<UserTimelineModel> _logger;

    public UserTimelineModel(ICheepService service, ILogger<UserTimelineModel> logger)
    {
        _service = service;
        _logger = logger;
        //Cheeps = service.GetCheeps(null);
    }

    public ActionResult OnGet(string author, [FromQuery] int page)
    {

        //Cheeps = _service.GetCheeps(page);
        //Cheeps = _service.GetCheeps(author);
        var _Cheeps = _service.GetCheepsFromAuthor(author, page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;

        /* var _TotalPages = _service.GetPageAmount(author);
         _TotalPages.Wait();
         TotalPages = _TotalPages.Result; */
        TotalPages = 1;
        PageNav = new PageNavModel(_service, page, TotalPages);

        return Page();
    }

    public int FollowingCount(string author)
    {
        var _FollowingCount = _service.GetFollowingCount(author);
        Console.WriteLine(_FollowingCount.Result + " is the result of _FollowingCount");
        return _FollowingCount.Result;
    }
    public int FollowersCount(string author)
    {
        var _FollowersCount = _service.GetFollowersCount(author);
        Console.WriteLine(_FollowersCount.Result + " is the result of _FollowingCount");
        return _FollowersCount.Result;
    }

}
