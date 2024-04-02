namespace PGHttpRequests.Services;

public class MyHttpClient
{
    private readonly HttpClient _httpClient;

    public MyHttpClient()
    {
        _httpClient = new HttpClient();
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
