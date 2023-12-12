using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Web.Pages.Shared;
public class Methods
{
    
   
    
    public static async Task FollowAuthor(IAuthorRepository authorRepository, string followerName, string followingName)
    {
        Console.WriteLine($"FollowAuthor called with followerName: {followerName}, followingName: {followingName} \n \n \n \n \n \n \n \n");
        
        var _follower = await authorRepository.FindAuthorByName(followerName);
        if (_follower == null) 
        {
            authorRepository.CreateAuthor(new CreateAuthorDTO(followerName, ""));
            _follower = await authorRepository.FindAuthorByName(followerName);
        }

        var _following = await authorRepository.FindAuthorByName(followingName);

        await authorRepository.FollowAuthor(_following!, _follower!);
    }

    public static async Task UnfollowAuthor(IAuthorRepository authorRepository, string followerName, string followingName)
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

        await authorRepository.UnfollowAuthor(_following!, _follower!);
    }

    public static async Task<bool> IsFollowing(IAuthorRepository authorRepository, CheepRepository cheepRepository, string self, string other)
    {
        Console.WriteLine("IsFollowing called");
        var _self = await authorRepository.FindAuthorByName(self);
        if (_self == null)
        {
            return false;
        }
        var _other = await authorRepository.FindAuthorByName(other);
        if (_other!.Followers!.Contains(_self.Id))
        {
            return true;
        }
        return false;
    }

}
