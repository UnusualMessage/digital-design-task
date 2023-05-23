using System.Text;

namespace TextProcessor;

public class File
{
    private readonly string _path;

    public File(string path)
    {
        _path = path;
    }

    private async Task<string[]> Read()
    {
        using var reader = new StreamReader(_path);
        var result = await reader.ReadToEndAsync();

        return result.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static async Task Write(IEnumerable<KeyValuePair<string, int>> pairs)
    {
        var output = new StringBuilder();
        foreach (var pair in pairs) output.Append($"{pair.Key}\t{pair.Value}\n");

        await using var writer = new StreamWriter("./output.txt");
        await writer.WriteAsync(output);
    }
}
