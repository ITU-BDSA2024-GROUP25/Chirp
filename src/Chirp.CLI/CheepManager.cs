using System.Globalization;
using CsvHelper;
using SimpleDB;


namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);

    IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;

    public void saveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }

        Cheep cheep = new Cheep(Environment.UserName, message, getTimestampUNIX(DateTimeOffset.UtcNow));

        database.Store(cheep);


    }

    public void readCheep(int? limit = null)
    {
          

            foreach (var cheep in database.Read(limit))
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).DateTime;
                
                var output = $"{cheep.Author} @ {dateTime}: {cheep.Message}";
                

                Console.WriteLine(output);
                
            }
    }

    public long getTimestampUNIX(DateTimeOffset dt)
    {
        return dt.ToUnixTimeSeconds();
    }
}