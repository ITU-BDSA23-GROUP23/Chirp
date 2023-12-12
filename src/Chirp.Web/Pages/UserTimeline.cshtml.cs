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

    private readonly IAuthorRepository authorRepository;

    private readonly ICheepRepository cheepRepository;

    private readonly ILogger<UserTimelineModel> _logger;

    public string author;

    public UserTimelineModel(ILogger<UserTimelineModel> logger, IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _logger = logger;
        this.authorRepository = authorRepository;
        this.cheepRepository = cheepRepository;
        PageNav = new PageNavModel(1, TotalPages);
    }

    public async Task<ActionResult> OnGet(string author, [FromQuery] int page)
    {
        
        //Cheeps = _service.GetCheeps(page);
        //Cheeps = _service.GetCheeps(author);
        if(page == 0)
        {
            page = 1;
        }
        try 
        {
            var _Cheeps = cheepRepository.GetCheeps(page, authorName: author);
            _Cheeps.Wait();
            Cheeps = _Cheeps.Result;
        }
        catch (AggregateException e)
        {
            Console.WriteLine(e);
        }   
        
        try
        {

            var _TotalPages = cheepRepository.GetPageAmount(author);
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




    public async Task<ActionResult> OnPost([FromQuery] int page, [FromQuery] string f, [FromQuery] string uf, [FromQuery] string c, [FromQuery] string li, [FromQuery] string di, [FromQuery] string lo)
    {
        li = HttpContext.Request.Query["li"].ToString();
        di = HttpContext.Request.Query["di"].ToString();
        lo = HttpContext.Request.Query["lo"].ToString();
        if (string.IsNullOrEmpty(li))
        {
            li = Request.Form["Like"];
        }

        if (string.IsNullOrEmpty(di))
        {
            di = Request.Form["Dislike"];
        }

        if (string.IsNullOrEmpty(lo))
        {
            lo = Request.Form["Love"];
        }
        
        if (f != null)
        {
            await Methods.FollowAuthor(authorRepository, f, Request.Form["Follow"]);
        } else if (uf != null)
        {
            await Methods.UnfollowAuthor(authorRepository, uf, Request.Form["Unfollow"]);
        } 
        else if(li != null)
        {
            Guid liGuid = Guid.Parse(li);
            await cheepRepository.ReactToCheep(author, "Like", liGuid);
        } else if (di != null)
        {   
            Guid diGuid = Guid.Parse(di);
            await cheepRepository.ReactToCheep(author, "Dislike", diGuid);
        } else if (lo != null)
        {
            Guid loGuid = Guid.Parse(lo);
            await cheepRepository.ReactToCheep(author, "Love", loGuid);
        }
        return RedirectToPage(author);
    }


    public CheepModel GenerateCheepModel(CheepDTO cheep)
    {
        return new CheepModel(authorRepository, cheepRepository, cheep);
    }
}
