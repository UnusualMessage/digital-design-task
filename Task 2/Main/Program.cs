using System.Reflection;
using TextProcessor;
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

        var file = new File(path);

        var method = typeof(File).GetMethod("Read",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

        if (method?.Invoke(file, null) is not Task<string[]> task) return;

        var words = await task;
        var pairs = new Words(words).GetSortedByCount();
        await File.Write(pairs);
    }
}
