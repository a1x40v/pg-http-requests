
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Hosting;
using RequestsApi.ActorSystem.ActorMessages;

namespace RequestsApi.ActorSystem.Actors;

public class MainActor : ReceiveActor
{
    private readonly IActorRef _askActor;

    public MainActor(IRequiredActor<AskActor> requiredAskActor)
    {
        _askActor = requiredAskActor.ActorRef;

        Receive<Messages.RunApp>(msg =>
        {
            Console.WriteLine("MainActor start");

            var propsOne = DependencyResolver.For(Context.System).Props<CodeActor>();
            Context.ActorOf(propsOne, "code-actor-1");

            var propsTwo = DependencyResolver.For(Context.System).Props<CodeActor>();
            Context.ActorOf(propsTwo, "code-actor-2");

            var propsThree = DependencyResolver.For(Context.System).Props<CodeActor>();
            Context.ActorOf(propsThree, "code-actor-3");

            Console.WriteLine("MainActor end");
        });
    }
}
