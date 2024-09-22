using CsvHelper;
using SimpleDB;
IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// adapted from copilot code, full code at bottom of program
// code appeared when googeling "c# forloop in app.mapget"
app.MapGet("/cheeps/{limit}", async (HttpRequest request) =>
    {
     var limit = request.RouteValues["limit"].ToString(); // <- https://www.youtube.com/watch?v=AuKKFVSMxJc
     int? intlimit = null;                                //    very helpfull getting the basics down
     if (limit == "all")                                  //    used the request example for thisS
        {
        intlimit = null;       
        }else{
        intlimit = int.Parse(limit);  
        }
     var BS = new List<Cheep>();
     foreach (var cheep in database.Read(intlimit)){ // <- this needs to accept an argument from cheepmanager
               BS.Add(cheep);
     }
     return BS;       
    });
        
app.MapPost("/cheep", (Cheep cheep) => 
    {
    database.Store(cheep);
    return Results.Ok(cheep);
    });

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