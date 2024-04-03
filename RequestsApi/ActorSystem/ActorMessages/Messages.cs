namespace RequestsApi.ActorSystem.ActorMessages;

public class Messages
{
    public class RunApp { }
    public class InitActor { }

    public class CodeReqeust
    {
        public string Url { get; set; }
    }

    public class DummyRequest
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }

    public class DummyResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Response { get; set; }
    }
}
