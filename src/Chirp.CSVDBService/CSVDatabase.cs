using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
    // private readonly here, public only for testing
    public string path = "/data/chirp_cli_db.csv";
    public record Cheep(string Author, string Message, long Timestamp);
    
    private static CSVDatabase<T>? _instance = null;
    
    private CSVDatabase() { }

    public static CSVDatabase<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CSVDatabase<T>();
            }
            return _instance;
        }
    }
    public void Store(T record)
    {
        
        using (var stream = new FileStream(path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            IEnumerable<T> records;

            if (limit != null)
            {

                records = csv.GetRecords<T>().ToList().TakeLast((int)limit);


            }
            else
            {
                records = csv.GetRecords<T>().ToList();
            }
            return records;
        }
    }
}