

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page, int pageSize);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
    public int GetTotalCheepsCount();
}

public class CheepService : ICheepService
{
 
    private static readonly List<CheepViewModel> _cheeps = SQLReader.reader();

    public int GetTotalCheepsCount()
    {
        return _cheeps.Count();
    }

     public List<CheepViewModel> GetCheeps(int pageNumber, int pageSize)
    {
        return _cheeps
            // .OrderByDescending(c => c.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
