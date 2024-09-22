using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost:5137/"),
    };
    public async Task saveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }
        Cheep cheep = new Cheep(Environment.UserName, message, getTimestampUNIX(DateTimeOffset.UtcNow));

        await sharedClient.PostAsJsonAsync("cheep", cheep);
    }

    public async Task readCheep(int? limit = null)
    {

        Console.WriteLine("Reading cheep");
        var cheeps = await sharedClient.GetFromJsonAsync<List<Cheep>>($"cheeps/{limit}");

        if (cheeps != null)
            foreach (var cheep in cheeps)
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