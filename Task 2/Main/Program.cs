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

        // Берем protected метод из экземпляра File
        var readMethod = typeof(File).GetMethod("Read",
            BindingFlags.Instance | BindingFlags.NonPublic);

        // Вызов protected метода
        if (readMethod?.Invoke(file, null) is not Task<string[]> task) return;

        var words = await task;

        var wordsInstance = new Words(words);

        // Берем private метод из экземпляра Words
        var getSortedByCountMethod = typeof(Words).GetMethod("GetSortedByCount",
            BindingFlags.Instance | BindingFlags.NonPublic);

        // Вызов private метода
        if (getSortedByCountMethod?.Invoke(wordsInstance, null) is not IEnumerable<KeyValuePair<string, int>> pairs)
            return;

        await File.Write(pairs);
    }
}
