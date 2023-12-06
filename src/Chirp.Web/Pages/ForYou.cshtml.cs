
using System.Diagnostics.CodeAnalysis;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ForYouModel : PageModel
{
    private readonly IAuthorRepository authorRepository;
    private readonly ICheepRepository cheepRepository;
    public PageNavModel PageNav;
    public IEnumerable<CheepDTO> Cheeps;

    public ForYouModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository) 
        {
            this.authorRepository = authorRepository;
            this.cheepRepository = cheepRepository;
            PageNav = new PageNavModel(1, 1);
        }

    public async Task<ActionResult> OnGet(string authorName, [FromQuery] int page)
    {
        Console.WriteLine("ForYouPage for author: " + authorName);
        
        var TotalPages = await cheepRepository.GetPageAmount();
        try
        {
            var followingAuthors = (await authorRepository.FindAuthorByName(authorName)).Following;

            long totalCheeps = 0;

            foreach (var author in followingAuthors)
            {
               totalCheeps += await authorRepository.GetCheepAmount(await authorRepository.GetAuthorName(author)); 
            }

            PageNav = new PageNavModel(page, (int)Math.Ceiling((double)totalCheeps / 32));

            Cheeps = await cheepRepository.GetCheepsFromAuthors(followingAuthors, page, 32);


        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
        }
        return Page();
    } 

    public async Task<IActionResult> OnPost(string authorName, [FromQuery] int page, [FromQuery] string uf)
    {
        
        Console.WriteLine("UF NOT NULL!!!!");
        string? AuthorName = Request.Form["Unfollow"];
        await UnfollowAuthor(uf, AuthorName);
        
        return await OnGet(authorName, page);
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
}
