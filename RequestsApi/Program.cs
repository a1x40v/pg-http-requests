using PGHttpRequests.Services;

// var builder = WebApplication.CreateBuilder(args);

// var app = builder.Build();

// app.Run();

var myHttpClient = new MyHttpClient();

await myHttpClient.GetAsync("https://echo.free.beeceptor.com");
