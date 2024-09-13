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
    
        Cheep cheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        
        database.Store(cheep); // should we have some feedback here?  so the user can tell that it worked.
        
    }
    public void readCheep(int? limit = null) 
    { 
         database.Read(limit); // note: this needs to be changed. should fetch IEnumerable records from CSVDatabase
    }
        
}