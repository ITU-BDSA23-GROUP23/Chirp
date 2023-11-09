namespace Chirp.Core;

public interface ICheepRepository
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null);
    Task<long> GetCheepsAmount(string? authorName = null);
    void CreateCheep(AuthorDTO Author, string Message);

    //Task<IEnumerable<CheepDTO>> CheepsToCheepDTOs(Task<List<cheep>> cheeps);
    // void Add(Cheep entity);
    // void Update(Cheep entity);
    // void Remove(Cheep entity);
}

