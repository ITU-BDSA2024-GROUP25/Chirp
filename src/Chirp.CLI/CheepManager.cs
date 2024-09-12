using System.Globalization;
using CsvHelper;
using SimpleDB;


namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);
    IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

    public void saveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }

        long unixTimestamp = get_UNIX_Timestamp(DateTimeOffset.UtcNow);
        
        Cheep cheep = new Cheep(Environment.UserName, message, unixTimestamp);
        
        database.Store(cheep);
        
    }
    public void readCheep(int? limit = null) 
    { 
         database.Read(limit);
    }

    public long get_UNIX_Timestamp(DateTimeOffset time)
    {
        return time.ToUnixTimeSeconds();
    }
}