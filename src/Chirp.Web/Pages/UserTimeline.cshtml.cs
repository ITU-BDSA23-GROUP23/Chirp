﻿using Chirp.Core;
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
    private readonly ICheepService _service;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }

    public int TotalPages { get; set; }

    public PageNavModel PageNav;

    private readonly IAuthorRepository _authorRepository;

    private readonly ILogger<UserTimelineModel> _logger;

    public UserTimelineModel(ICheepService service, ILogger<UserTimelineModel> logger, IAuthorRepository authorRepository)
    {
        _service = service;
        _logger = logger;
        _authorRepository = authorRepository;
        PageNav = new PageNavModel(_service, 1, TotalPages);
    }

    public ActionResult OnGet(string author, [FromQuery] int page)
    {

        //Cheeps = _service.GetCheeps(page);
        //Cheeps = _service.GetCheeps(author);
        var _Cheeps = _service.GetCheepsFromAuthor(author, page);
        _Cheeps.Wait();
        Cheeps = _Cheeps.Result;

        try
        {
            var _TotalPages = _service.GetPageAmount(author);
            _TotalPages.Wait();
            TotalPages = _TotalPages.Result; // CHECK THAT THIS IS AS IT SHOULD BE!!!
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TotalPages = 1; // CHECK THAT THIS IS AS IT SHOULD BE!!!
        }

        PageNav = new PageNavModel(_service, page, TotalPages);

        return Page();
    }
}
