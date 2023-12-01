using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Chirp.Web.Pages.Shared;
using Humanizer;

namespace Chirp.Web.Pages;
[AllowAnonymous]
public class PublicModel : PageModel
{

    private readonly ICheepService _service;
    private readonly ILogger<PublicModel> _logger;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }
    public PageNavModel PageNav;

    public CreateCheepModel CreateCheep;

    public int TotalPages { get; set; }

    private readonly IAuthorRepository _Author_repository;

    private readonly ICheepRepository _Cheep_repository;

    public PublicModel(ICheepService service, ILogger<PublicModel> logger, IAuthorRepository Author_repository, ICheepRepository Cheep_repository)
    {
        _service = service;
        _logger = logger;
        _Author_repository = Author_repository;
        _Cheep_repository = Cheep_repository;
        CreateCheep = new CreateCheepModel(_service,_Author_repository, _Cheep_repository);
        PageNav = new PageNavModel(_service, 1, TotalPages);
        
        //Cheeps = service.GetCheeps(null);
        
    }

    public ActionResult OnGet([FromQuery] int page)
    {

        var _TotalPages = _service.GetPageAmount();
        _TotalPages.Wait();
        TotalPages = _TotalPages.Result;
        PageNav = new PageNavModel(_service, page, TotalPages);
       
        

        var _Cheeps = _service.GetCheeps(page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;

        return Page();
    }
}