using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Chirp.Web.Pages.Shared;
using System.Runtime.CompilerServices;

namespace Chirp.Web.Pages;
[AllowAnonymous]
public class UserTimelineModel : PageModel
{
    public IEnumerable<CheepDTO>? Cheeps { get; set; }

    public int TotalPages { get; set; }

    public PageNavModel PageNav;

    private readonly IAuthorRepository _authorRepository;

    private readonly ICheepRepository _cheepRepository;

    private readonly ILogger<UserTimelineModel> _logger;

    public UserTimelineModel(ILogger<UserTimelineModel> logger, IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _logger = logger;
        _authorRepository = authorRepository;
        _cheepRepository = cheepRepository;
        PageNav = new PageNavModel(1, TotalPages);
    }

    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        
        //Cheeps = _service.GetCheeps(page);
        //Cheeps = _service.GetCheeps(author);
        try 
        {
            var _Cheeps = _cheepRepository.GetCheeps(page, authorName: author);
            _Cheeps.Wait();
            Cheeps = _Cheeps.Result;
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
        }
        
        try
        {

            var _TotalPages = _cheepRepository.GetPageAmount(author);
             _TotalPages.Wait();
             TotalPages = _TotalPages.Result; 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TotalPages = 1;
        }
        PageNav = new PageNavModel(page, TotalPages);

        return Page();
    }
}
