using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Routing;
using RequestsApi.ActorSystem.ActorMessages;

namespace RequestsApi.ActorSystem.Actors;

public class AskActor : ReceiveActor
{
    private IActorRef _httpWorkersGroup;
    private int _lastId = 0;
    private readonly Dictionary<int, IActorRef> _messageRoute = [];

    public AskActor()
    {
        Receive<Messages.CodeReqeust>(msg =>
        {
            Console.WriteLine($"AskActor receieved CodeRequest {msg.Url}");
            var message = new Messages.DummyRequest { Id = _lastId++, Url = msg.Url };
            _messageRoute.Add(message.Id, Sender);
            _httpWorkersGroup.Tell(message);
        });

        Receive<Messages.DummyResponse>(msg =>
        {
            Console.WriteLine($"AskActor receieved DummyResponse {msg.Url} {msg.Response}");
            var sender = _messageRoute[msg.Id];
            _messageRoute.Remove(msg.Id);
            sender.Tell(msg);
        });
    }

    protected override void PreStart()
    {
        string[] ips = ["11.11.11.11", "22.22.22.22"];
        List<string> actorNames = [];

        _httpWorkersGroup = Context.System.ActorOf(Props.Empty.WithRouter(new RoundRobinGroup()), "http-workers");

        for (int i = 0; i < ips.Length; i++)
        {
            string ip = ips[i];
            var props = DependencyResolver.For(Context.System).Props<DummyHttpActor>(ip);
            string name = $"http-worker-w{i + 1}";
            actorNames.Add($"/user/{name}");
            var actorRef = Context.ActorOf(props, name);
            var routee = Routee.FromActorRef(actorRef);
            _httpWorkersGroup.Tell(new AddRoutee(routee));
        }
    }
}
