using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
    private readonly string _path = "chirp_cli_db.csv";
    public record Cheep(string Author, string Message, long Timestamp);
    
    public void Store(T record)
    {
        
        using (var stream = new FileStream(_path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(_path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();
            string output;
            while (csv.Read())
            {
                var record = csv.GetRecord<Cheep>();
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(record.Timestamp).DateTime;

                output = $"{record.Author} @ {dateTime}: {record.Message}";
                    
                Console.WriteLine(output);
            }
            return null;
        }
    }
}