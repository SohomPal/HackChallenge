using System.Diagnostics;

//Load .env as early as possible
DotNetEnv.Env.Load();

Console.WriteLine("Started!");

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Use(async (context, next) =>
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    await next();
    stopwatch.Stop();

    Console.WriteLine($"Request: {context.Request.Method} {context.Response.StatusCode} {context.Request.Path} {stopwatch.ElapsedMilliseconds}ms");
});


app.MapGet("/", () => "Hello World!");

app.Run();