

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapPost("/Cheep", (Cheep cheep) => {

    ChirpDB.Instance.Store(cheep);


} );
app.MapGet("/Cheeps", () => ChirpDB.Instance.Read(null));

app.Run();
