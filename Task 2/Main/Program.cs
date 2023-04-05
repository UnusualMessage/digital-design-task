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

        var file = new File(path);
        
        var words = await file.ReadFile();
        var pairs = new WordsDictionary(words).GetSortedByCount();
        await File.WriteFile(pairs);
    }
}