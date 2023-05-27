using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TextProcessor;
using WebAPI;
using File = TextProcessor.File;

namespace Main;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var path = "./Assets/book.txt";
        try
        {
            path = (string?)args.GetValue(0);
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Параметры не заданы, берем книгу из ассетов");
        }

        if (path is null) return;

        var file = File.GetInstance(path);

        if (file is null) return;

        var readMethod = typeof(File).GetMethod("Read",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (readMethod?.Invoke(file, null) is not Task<string[]> task) return;

        var words = await task;

        await WriteSlowly(words);
        await WriteFastly(words);
        await WriteFromWebApi(file);
    }

    private static async Task WriteFromWebApi(File file)
    {
        var text = await file.ReadText();
        var serializedMessage = JsonSerializer.Serialize(new Request(text));

        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5170/words");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new StringContent(serializedMessage, Encoding.UTF8);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var client = new HttpClient();
        var response = await client.SendAsync(request);

        var str = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Dictionary<string, int>>(str);

        if (result is null) return;

        var webPairs = Words.GetSortedByCount(result);
        await File.Write(webPairs, "web.txt");
    }

    private static async Task WriteFastly(IEnumerable<string> words)
    {
        var fastInstance = new Words();
        fastInstance.CountFast(words);

        var getSortedByCountMethod = typeof(Words).GetMethod("GetSortedByCount",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (getSortedByCountMethod?.Invoke(fastInstance, null) is not IEnumerable<KeyValuePair<string, int>> fastPairs)
            return;

        await File.Write(fastPairs, "fast.txt");
    }

    private static async Task WriteSlowly(IEnumerable<string> words)
    {
        var slowInstance = new Words();

        var countSlowMethod = typeof(Words).GetMethod("CountSlow", BindingFlags.Instance | BindingFlags.NonPublic);
        countSlowMethod?.Invoke(slowInstance, new object[] { words });

        var getSortedByCountMethod = typeof(Words).GetMethod("GetSortedByCount",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (getSortedByCountMethod?.Invoke(slowInstance, null) is not IEnumerable<KeyValuePair<string, int>> slowPairs)
            return;

        await File.Write(slowPairs, "slow.txt");
    }
}
