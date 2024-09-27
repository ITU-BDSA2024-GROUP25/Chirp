using System.Data;
using Microsoft.Data.Sqlite;

namespace chirp.razor;
public record CheepViewModel(string Author, string Message, string Timestamp);

public interface IDatabaseReader
{
    public void reader(String input)
}

public class SQLReader : IDatabaseReader
{
    public void reader(String input)
    {
        var sqlDBFilePath = "/data/chirp.db";
        var sqlQuery = @"SELECT * FROM message ORDER by message.pub_date desc";
        var allCheeps = new List<CheepViewModel>();

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                var dataRecord = (IDataRecord)reader;
                for (var i = 0; i < dataRecord.FieldCount; i++)
                    Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                var values = new object[reader.FieldCount];
                var fieldCount = reader.GetValues(values);
                for (var i = 0; i < fieldCount; i++)
                    Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
            }

        }
    }
}
