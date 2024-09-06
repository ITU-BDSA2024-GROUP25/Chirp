using System.Globalization;
using CsvHelper;
using SimpleDB;


namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);
    IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

    public void SaveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }
    
        Cheep cheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        
        database.Store(cheep);
        
    }
    public void ReadCheep() // this should be able to take ints!
    { 
         database.Read();
    }
        
}