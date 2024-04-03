using Akka.Actor;
using RequestsApi.ActorSystem.ActorMessages;

namespace RequestsApi.ActorSystem.Actors;

public class DummyHttpActor : ReceiveActor
{
    private readonly string _ip;

    public DummyHttpActor(string ip)
    {
        _ip = ip;

        ReceiveAsync<Messages.DummyRequest>(async msg =>
        {
            Console.WriteLine($"DummyHttpActor[{_ip}] received MessageDummyReqeust [{msg.Url}]");
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine($"DummyHttpActor[{_ip}] delay is over [{msg.Url}]");
            Sender.Tell(new Messages.DummyResponse { Id = msg.Id, Url = msg.Url, Response = "response" });
        });
    }
}
