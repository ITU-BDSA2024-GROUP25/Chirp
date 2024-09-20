using CsvHelper;
using SimpleDB;
IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// adapted from copilot code, full code at bottom of program
// code appeared when googeling "c# forloop in app.mapget"
app.MapGet("/cheeps", () =>
    {
     var BS = new List<Cheep>();
     foreach (var cheep in database.Read()){
               BS.Add(cheep);
            }
     return BS;       
    });
        
app.MapGet("/cheep", () => "ein requeste hereth postth thine cheep to yon csv DB");
app.Run();


public record Cheep(string Author, string Message, long Timestamp);



/*app.MapGet("/loop", () =>
{
    var result = new List<int>();
    for (int i = 0; i < 10; i++)
    {
        result.Add(i);
    }
    return Results.Ok(result);
}); <-- suggested solution from copilot
        used in adapting the mapget to json */