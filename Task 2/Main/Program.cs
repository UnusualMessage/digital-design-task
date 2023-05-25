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

        // Создание "экземпляра" абстрактного класса 
        var file = File.GetInstance(path);

        if (file is null) return;

        // Берем protected метод Read из экземпляра File
        var readMethod = typeof(File).GetMethod("Read",
            BindingFlags.Instance | BindingFlags.NonPublic);
        // Вызов Read
        if (readMethod?.Invoke(file, null) is not Task<string[]> task) return;

        var words = await task;

        var slowInstance = new Words();
        // Берем private метод CountSlow из Words
        var countSlowMethod = typeof(Words).GetMethod("CountSlow", BindingFlags.Instance | BindingFlags.NonPublic);
        // Вызов CountSlow
        countSlowMethod?.Invoke(slowInstance, new object[] { words });

        var fastInstance = new Words();
        fastInstance.CountFast(words);

        // Берем private метод GetSortedByCount из экземпляра Words
        var getSortedByCountMethod = typeof(Words).GetMethod("GetSortedByCount",
            BindingFlags.Instance | BindingFlags.NonPublic);
        // Вызов GetSortedByCount
        if (getSortedByCountMethod?.Invoke(slowInstance, null) is not IEnumerable<KeyValuePair<string, int>> slowPairs)
            return;

        // Вызов GetSortedByCount
        if (getSortedByCountMethod?.Invoke(fastInstance, null) is not IEnumerable<KeyValuePair<string, int>> fastPairs)
            return;

        await File.Write(slowPairs, "slow.txt");
        await File.Write(fastPairs, "fast.txt");
    }
}
