namespace Chirp.Core;

public interface ICheepRepository
{
    /// <summary>
    /// We use a repository pattern to abstract away the database. This interface defines the Cheep specific methods that we need in our repository.
    /// </summary>
    Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1, int pageSize = 32, string? authorName = null);
    Task<IEnumerable<CheepDTO>> GetCheepsFromAuthors(ICollection<Guid> authorsId, int page = 1, int pageSize = 32);
    Task<long> GetCheepsAmount(string? authorName = null);
    void CreateCheep(createCheepDTO cheepDTO, DateTime? Timestamp);

    Task<int> GetPageAmount(String? authorName = null);
    Task ReactToCheep(string type, Guid cheepId);

    //Task<IEnumerable<CheepDTO>> CheepsToCheepDTOs(Task<List<cheep>> cheeps);
    // void Add(Cheep entity);
    // void Update(Cheep entity);
    // void Remove(Cheep entity);
}
