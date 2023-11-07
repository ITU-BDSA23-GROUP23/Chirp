using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Chirp.Web.Pages;



public class CreateCheepModel : PageModel {


    private readonly ICheepService _service;
    private readonly ILogger<PublicModel> _logger;

    public CreateCheepModel(ICheepService service, ILogger<PublicModel> logger)
    {
        _service = service;
        _logger = logger;

    }








}