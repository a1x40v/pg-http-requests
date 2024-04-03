using Akka.Actor;
using Akka.Hosting;
using RequestsApi.ActorSystem.ActorMessages;

namespace RequestsApi.ActorSystem.Actors;

public class CodeActor : ReceiveActor
{
    private readonly IActorRef _askActor;

    public CodeActor(IRequiredActor<AskActor> requiredAskActor)
    {
        _askActor = requiredAskActor.ActorRef;

        ReceiveAsync<Messages.InitActor>(async msg =>
        {
            string actorNum = Self.Path.Name.Split("-").Last();
            string text = $"msg{actorNum}";
            Console.WriteLine($"[{Self.Path.Name}] sending DummyReqeust text: {text}");

            var response = await _askActor.Ask<Messages.DummyResponse>(new Messages.CodeReqeust { Url = text });

            Console.WriteLine($"[{Self.Path.Name}] received MessageDummyResponse text: {response.Url}");
        });
    }

    protected override void PreStart()
    {
        Self.Tell(new Messages.InitActor());
    }
}
