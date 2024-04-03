using Akka.Hosting;
using Akka.Actor;
using RequestsApi.ActorSystem.Actors;
using RequestsApi.ActorSystem.ActorMessages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAkka("MyActorSystem", configurationBuilder =>
        {
            configurationBuilder
                .WithActors((system, registry, resolver) =>
                {
                    IActorRef mainActor = system.ActorOf(
                        resolver.Props<MainActor>(),
                        "mainActor"
                    );
                    registry.TryRegister<MainActor>(mainActor);

                    IActorRef askActor = system.ActorOf(
                        resolver.Props<AskActor>(),
                        "askActor"
                    );
                    registry.TryRegister<AskActor>(askActor);

                });
        });

var app = builder.Build();


var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var actorSystem = app.Services.GetRequiredService<ActorSystem>();

lifetime.ApplicationStarted.Register(() =>
{
    Task.Run(() =>
    {
        var mainActorRef = actorSystem.ActorSelection("/user/mainActor");
        mainActorRef.Tell(new Messages.RunApp());
    });
});

app.Run();

// var myHttpClient = new MyHttpClient();

// await myHttpClient.GetAsync("https://echo.free.beeceptor.com");
