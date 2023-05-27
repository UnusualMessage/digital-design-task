using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WordsController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] Request request)
    {
        var words = SplitIntoWords(request.Text);
        var response = Count(words);

        return Ok(response);
    }

    private static IEnumerable<string> SplitIntoWords(string text)
    {
        return text.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static Dictionary<string, int> Count(IEnumerable<string> words)
    {
        var dictionary = new ConcurrentDictionary<string, int>();

        Parallel.ForEach(words, word =>
        {
            dictionary.AddOrUpdate(
                word,
                _ => 1,
                (_, currentValue) => currentValue + 1);
        });

        return dictionary.ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
