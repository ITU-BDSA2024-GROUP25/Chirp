using CsvHelper;
using SimpleDB;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cheeps", () => new[]
    {
            new {name = "pølse"}, 
            /*         foreach (var cheep in database.Read()){
                {new name = $"{cheep.Author}",timestamp =  "{cheep.Timestamp}", content = $"{cheep.Message}"}
            }*/
    });
        
app.MapGet("/cheep", () => "ein requeste hereth postth thine cheep to yon csv DB");
app.Run();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
public record Cheep(string Author, string Message, long Timestamp);
