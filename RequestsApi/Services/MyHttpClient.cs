using System.Net;
using System.Net.Sockets;

namespace PGHttpRequests.Services;

public class MyHttpClient
{
    private readonly HttpClient _httpClient;

    public MyHttpClient()
    {
        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
                var localIpAddress = new IPAddress(new byte[] { 176, 119, 147, 201 });
                var localEndPoint = new IPEndPoint(localIpAddress, 0);
                socket.Bind(localEndPoint);

                var host = context.DnsEndPoint.Host;
                var port = context.DnsEndPoint.Port;
                await socket.ConnectAsync(host, port);

                return new NetworkStream(socket, ownsSocket: true);
            },
        };

        _httpClient = new HttpClient(handler);
    }

    public async Task GetAsync(string url)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

        Console.WriteLine($"--- GET {url}");
        HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();

        string body = await httpResponseMessage.Content.ReadAsStringAsync();

        Console.WriteLine(body);
    }
}
