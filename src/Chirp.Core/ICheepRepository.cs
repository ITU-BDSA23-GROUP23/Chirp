namespace Chirp.Core;

public interface ICheepRepository
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null);
    void CreateCheep(AuthorDTO Author, string Message); 
    // void Add(Cheep entity);
    // void Update(Cheep entity);
    // void Remove(Cheep entity);
}

