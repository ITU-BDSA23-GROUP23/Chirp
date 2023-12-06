using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Chirp.Web.Pages.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Humanizer;
using SQLitePCL;

namespace Chirp.Web.Pages;
[AllowAnonymous]
public class PublicModel : PageModel
{
    private readonly IAuthorRepository authorRepository;
    private readonly ICheepRepository cheepRepository;
    public CreateCheepModel CreateCheep;
    private readonly ILogger<PublicModel> _logger;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }
    public PageNavModel PageNav;
    public int TotalPages { get; set; }
    public string? author { get; set; }

    public PublicModel(ILogger<PublicModel> logger, IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _logger = logger;
        this.authorRepository = authorRepository;
        this.cheepRepository = cheepRepository;
        PageNav = new PageNavModel(1, TotalPages);
        CreateCheep = new CreateCheepModel(authorRepository, cheepRepository);
        
    }

    public virtual async Task<ActionResult> OnGet([FromQuery] int page)
    {
        var _TotalPages = cheepRepository.GetPageAmount();
        _TotalPages.Wait();
        TotalPages = _TotalPages.Result;
        PageNav = new PageNavModel(page, TotalPages);
       
        var _Cheeps = cheepRepository.GetCheeps(page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;

        return Page();
    }
 
    public async Task<ActionResult> OnPost([FromQuery] int page, [FromQuery] string f, [FromQuery] string uf, [FromQuery] string c, [FromQuery] string li, [FromQuery] string di, [FromQuery] string lo)
    {
     
        var _Cheeps = await cheepRepository.GetCheeps(page);
        Cheeps = _Cheeps;

        var _TotalPages = await cheepRepository.GetPageAmount();
        TotalPages = _TotalPages;
        PageNav = new PageNavModel(page, TotalPages);
        if (c != null) 
        {
            string? Message = Request.Form["Cheep"];
            CreateCheep.OnPostCheep(c, Message);
            return RedirectToPage("");
        } 
        
        return Page();
    }

    public CheepModel GenerateCheepModel(CheepDTO cheep)
    {
        return new CheepModel(authorRepository, cheepRepository, cheep);
    }
}

    
