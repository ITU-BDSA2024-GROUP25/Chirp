using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);




    public void saveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }

        Cheep cheep = new Cheep(Environment.UserName, message, getTimestampUNIX(DateTimeOffset.UtcNow));

        Console.WriteLine("Error: non-empty cheep message");


    }

    public void readCheep(int? limit = null)
    {
          

            /*foreach (var cheep in database.Read(limit))
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).DateTime;
                
                var output = $"{cheep.Author} @ {dateTime}: {cheep.Message}";
                

                Console.WriteLine(output);
                
            }*/
            Console.WriteLine("Error: reading!");
    }

    public long getTimestampUNIX(DateTimeOffset dt)
    {
        return dt.ToUnixTimeSeconds();
    }
}