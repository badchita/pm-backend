var builder = WebApplication.CreateBuilder(args);

// Minimal: no DB/Identity/Swagger yet
var app = builder.Build();

app.MapGet("/", () => "Hello from pm-backend!");

app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Sweltering","Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new {
            Date = DateTime.Now.AddDays(index).ToString("yyyy-MM-dd"),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();

    return Results.Ok(forecast);
});

app.Run();
