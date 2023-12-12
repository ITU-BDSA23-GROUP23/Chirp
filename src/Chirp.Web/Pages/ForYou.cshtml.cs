
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
    public IEnumerable<CheepDTO>? Cheeps;
    public string? author;

    public ForYouModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository) 
        {
            this.authorRepository = authorRepository;
            this.cheepRepository = cheepRepository;
            PageNav = new PageNavModel(1, 1);
        }

    public async Task<ActionResult> OnGet(string authorName, [FromQuery] int page)
    {
        
        var TotalPages = await cheepRepository.GetPageAmount();
        try
        {
            var followingAuthors = (await authorRepository.FindAuthorByName(authorName))!.Following;

            long totalCheeps = 0;

            foreach (var author in followingAuthors!)
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


    public async Task<ActionResult> OnPost(string authorName, [FromQuery] int page, [FromQuery] string f, [FromQuery] string uf, [FromQuery] string c, [FromQuery] string li, [FromQuery] string di, [FromQuery] string lo)
    {
        await OnGet(authorName, page);

        li = HttpContext.Request.Query["li"].ToString();
        di = HttpContext.Request.Query["di"].ToString();
        lo = HttpContext.Request.Query["lo"].ToString();
        if (string.IsNullOrEmpty(li))
        {
            li = Request.Form["Like"]!;
        }

        if (string.IsNullOrEmpty(di))
        {
            di = Request.Form["Dislike"]!;
        }

        if (string.IsNullOrEmpty(lo))
        {
            lo = Request.Form["Love"]!;
        }
        
        if (f != null)
        {
            await Methods.FollowAuthor(authorRepository, f, Request.Form["Follow"]!);
        } else if (uf != null)
        {
            await Methods.UnfollowAuthor(authorRepository, uf, Request.Form["Unfollow"]!);
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
        
        return await OnGet(authorName, page);
    }



    public CheepModel GenerateCheepModel(CheepDTO cheep)
    {
        return new CheepModel(authorRepository, cheepRepository, cheep);
    }


}