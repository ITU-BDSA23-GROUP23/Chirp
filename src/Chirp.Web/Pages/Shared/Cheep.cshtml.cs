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

    public CheepModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository, CheepDTO cheep)
    {
        this.authorRepository = authorRepository;
        this.cheepRepository = cheepRepository;
        this.cheep = cheep;
    }


    
    
    public async Task FollowAuthor(string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName} \n \n \n \n \n \n \n \n");
        
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
