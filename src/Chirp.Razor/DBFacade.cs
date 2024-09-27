using System.Data;
using Microsoft.Data.Sqlite;

public interface IDatabaseReader
{
    public static abstract List<CheepViewModel> reader();
}

public class SQLReader : IDatabaseReader
{
    public static List<CheepViewModel> reader()
    {
        var sqlDBFilePath = "data/chirp.db";
        var sqlQuery = @"SELECT * FROM user JOIN message on user_id = author_id ORDER by message.pub_date desc";
        var allCheeps = new List<CheepViewModel>();

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                CheepViewModel tempCheep = new CheepViewModel("null", "null", "null");
                // based on material from lecture 5
                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                var values = new object[reader.FieldCount];
                var fieldCount = reader.GetValues(values);
                tempCheep = new CheepViewModel($"{values[1]}", $"{values[5]}", $"{DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(values[6])).DateTime}");
                allCheeps.Add(tempCheep);
            }
            return allCheeps;
        }
    }
}
