

using Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(string? author);
    public int GetTotalCheepsCount(string? author);
    public int CurrentPage { get; set;}
}

public class CheepService : ICheepService
{
    
    private readonly ChirpDbContext _context;
    private CheepRepository _cheepRepo;
    public CheepService(ChirpDbContext context)
    {
        _context = context;
        _cheepRepo = new CheepRepository(context);
    }
 
    public int CurrentPage {get; set;}

    public int GetTotalCheepsCount(string? author = null)
    {
        return _cheepRepo.GetTotalCheepsCount(author);
    }

     public Task<List<CheepDto>> GetCheeps(string? author = null)
     {
        return _cheepRepo.GetCheeps(author);
     }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
