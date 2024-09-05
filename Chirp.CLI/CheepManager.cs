using System.Globalization;
using CsvHelper;

namespace Chirp.CLI;

public class CheepManager
{
    public record Cheep(string Author, string Message, long Timestamp);
    
    private readonly string _path = "chirp_cli_db.csv";
    
    public CheepManager()
    { }

    public void SaveCheep(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Error: Empty cheep message");
            return;
        }

        Cheep cheep = new Cheep(Environment.UserName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        
        using (var stream = new FileStream(_path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(cheep);
            csv.NextRecord();
        }
    }

    public void ReadCheep()
    {
        using (var reader = new StreamReader(_path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = csv.GetRecord<Cheep>();
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(record.Timestamp).DateTime;

                string output = $"{record.Author} @ {dateTime}: {record.Message}";
                    
                Console.WriteLine(output);
            }
        }
    }
}