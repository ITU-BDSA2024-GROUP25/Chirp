var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cheeps", () => "here we doth reatethht thine cheeps");
app.MapGet("/cheep", () => "ein requeste hereth postth thine cheep to yon csv DB");
app.Run();
