using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace TextProcessor;

public abstract class File
{
    private readonly string _path;

    public File()
    {
    }

    protected File(string path)
    {
        _path = path;
    }

    public static File? GetInstance(string path)
    {
        var name = new AssemblyName("DynamicAssembly");
        var ab =
            AssemblyBuilder.DefineDynamicAssembly(
                name,
                AssemblyBuilderAccess.Run);

        var moduleBuilder =
            ab.DefineDynamicModule(name.Name!);

        var typeBuilder = moduleBuilder.DefineType(
            "FileInstance",
            TypeAttributes.Public);

        typeBuilder.SetParent(typeof(File));

        var type = typeBuilder.CreateType();

        var instance = Activator.CreateInstance(type);

        var fieldInfo = type.BaseType?.GetField("_path", BindingFlags.Instance | BindingFlags.NonPublic);
        fieldInfo?.SetValue(instance, path);

        return instance as File;
    }

    protected async Task<string[]> Read()
    {
        using var reader = new StreamReader(_path);
        var result = await reader.ReadToEndAsync();

        return result.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static async Task Write(IEnumerable<KeyValuePair<string, int>> pairs, string name = "./output.txt")
    {
        var output = new StringBuilder();
        foreach (var pair in pairs) output.Append($"{pair.Key}\t{pair.Value}\n");

        await using var writer = new StreamWriter(name);
        await writer.WriteAsync(output);
    }
}
