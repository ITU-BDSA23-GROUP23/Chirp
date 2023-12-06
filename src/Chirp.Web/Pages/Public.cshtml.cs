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
    protected readonly IAuthorRepository authorRepository;
    protected readonly ICheepRepository cheepRepository;
    public CreateCheepModel CreateCheep;
    private readonly ILogger<PublicModel> _logger;
    public IEnumerable<CheepDTO>? Cheeps { get; set; }
    public PageNavModel PageNav;
    public int TotalPages { get; set; }

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
     
        Console.WriteLine("OnPost called!");
        var _Cheeps = await cheepRepository.GetCheeps(page);
        Cheeps = _Cheeps;

        var _TotalPages = await cheepRepository.GetPageAmount();
        TotalPages = _TotalPages;
        PageNav = new PageNavModel(page, TotalPages);
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
        Console.WriteLine($"li is {li}");
        Console.WriteLine($"uf is {uf}");
        
        if (f != null)
        {
            string? AuthorName = Request.Form["Follow"];
            await FollowAuthor(f, AuthorName);
        } else if (uf != null)
        {
            string? AuthorName = Request.Form["Unfollow"];
            await UnfollowAuthor(uf, AuthorName);
        } else if (c != null) 
        {
            string? Message = Request.Form["Cheep"];
            CreateCheep.OnPostCheep(c, Message);
            return RedirectToPage("");
        } 
        else if(li != null)
        {
            Console.WriteLine("Li is: " + li);
            Guid liGuid = Guid.Parse(li);
            await cheepRepository.ReactToCheep("Like", liGuid);
        } else if (di != null)
        {   
            Guid diGuid = Guid.Parse(di);
            await cheepRepository.ReactToCheep("Dislike", diGuid);
        } else if (lo != null)
        {
            Guid loGuid = Guid.Parse(lo);
            await cheepRepository.ReactToCheep("Love", loGuid);
        }
        
        return Page();
    }

    
    
    public async Task FollowAuthor(string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName}");
        
        var _follower = await authorRepository.FindAuthorByName(followerName);
        if (_follower == null) 
        {
            authorRepository.CreateAuthor(new CreateAuthorDTO(followerName, ""));
            _follower = await authorRepository.FindAuthorByName(followerName);
        }

        var _following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.FollowAuthor(_following, _follower);
    }

    public async Task UnfollowAuthor(string followerName, string followingName)
    {

        Console.WriteLine($"UnfollowAuthor called with followerName: {followerName}, followingName: {followingName}");

        var _follower = await authorRepository.FindAuthorByName(followerName);
        if (_follower == null) 
        {
            Console.WriteLine("Follower is null");
            authorRepository.CreateAuthor(new CreateAuthorDTO(followerName, ""));
            _follower = await authorRepository.FindAuthorByName(followerName);
        }

        var _following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.UnfollowAuthor(_following, _follower);
    }

    public async Task<bool> IsFollowing(string self, string other)
    {
        Console.WriteLine("IsFollowing called");
        var _self = await authorRepository.FindAuthorByName(self);
        if (_self == null)
        {
            return false;
        }
        var _other = await authorRepository.FindAuthorByName(other);
        if (_other.Followers.Contains(_self.Id))
        {
            return true;
        }
        return false;
    }
}
