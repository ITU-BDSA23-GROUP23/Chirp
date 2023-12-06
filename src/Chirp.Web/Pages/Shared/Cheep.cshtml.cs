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

namespace Chirp.Web.Pages.Shared;
[AllowAnonymous]
public class CheepModel : PageModel
{
    protected readonly IAuthorRepository authorRepository;
    protected readonly ICheepRepository cheepRepository;
    public readonly CheepDTO cheep;
    public string? author;

    public CheepModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository, CheepDTO cheep)
    {
        this.authorRepository = authorRepository;
        this.cheepRepository = cheepRepository;
        this.cheep = cheep;
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
            string? AuthorName = Request.Form["Follow"];
            await FollowAuthor(f, AuthorName);
        } else if (uf != null)
        {
            string? AuthorName = Request.Form["Unfollow"];
            await UnfollowAuthor(uf, AuthorName);
        } 
        else if(li != null)
        {
            Guid liGuid = Guid.Parse(li);
            await cheepRepository.ReactToCheep(author, "Like", liGuid);
        } else if (di != null)
        {   
            Console.WriteLine("Dislike called \n \n \n \n \n \n \n");
            Guid diGuid = Guid.Parse(di);
            await cheepRepository.ReactToCheep(author, "Dislike", diGuid);
        } else if (lo != null)
        {
            Console.WriteLine("Love called \n \n \n \n \n \n \n");
            Guid loGuid = Guid.Parse(lo);
            await cheepRepository.ReactToCheep(author, "Love", loGuid);
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

    public async Task<int> GetReactionCount(Reactiontype type)
    {
        var reaction = await cheepRepository.GetReactions(cheep.Id, (int)type);

        return reaction.Count;
    }
}
