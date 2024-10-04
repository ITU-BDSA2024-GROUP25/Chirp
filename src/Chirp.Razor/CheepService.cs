

using Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<Cheep> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNumber, int pageSize);
    public int GetTotalCheepsCount();
    public int GetTotalCheepsCountFromAuthor(string author);
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
 
    private static readonly List<CheepViewModel> _cheeps = SQLReader.reader();
    public int CurrentPage {get; set;}

    public int GetTotalCheepsCount()
    {
        return _cheeps.Count();
    }

    public int GetTotalCheepsCountFromAuthor(string author)
    {
        return _cheeps.Where(x => x.Author == author).ToList().Count();
    }

     public List<Cheep> GetCheeps()
     {
         List<Cheep> cheeps = _cheepRepo.GetCheeps();
     }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNumber, int pageSize)
    {
        // filter by the provided author name
        return _cheeps
            .Where(x => x.Author == author).ToList()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
