namespace Chirp.Core;

public interface ICheepRepository
    {
        public Task CreateCheep(Cheep cheep);
        public int CurrentPage { get; set;  }
        public Task<List<CheepDto>> GetCheeps(string? author);
        public int GetTotalCheepsCount(string? author);
    }
