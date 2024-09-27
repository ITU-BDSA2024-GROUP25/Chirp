public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page, int pageSize);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
    public int GetTotalCheepsCount();
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("He1lge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adr2ian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helg3e", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adria4n", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge5", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adria6n", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge7", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adri8an", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helg9e", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adri10an", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge11", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adria12n", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("He13lge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        };

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
