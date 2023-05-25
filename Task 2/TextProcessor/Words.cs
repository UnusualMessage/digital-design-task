using System.Collections.Concurrent;
using System.Diagnostics;

namespace TextProcessor;

public class Words
{
    private readonly ConcurrentDictionary<string, int> _instance = new();

    private IEnumerable<KeyValuePair<string, int>> GetSortedByCount()
    {
        var pairs = _instance.ToList();
        pairs.Sort((firstPair, secondPair) => secondPair.Value.CompareTo(firstPair.Value));
        return pairs;
    }

    public void CountFast(IEnumerable<string> words)
    {
        var stopWatch = new Stopwatch();

        stopWatch.Start();
        Parallel.ForEach(words, word =>
        {
            _instance.AddOrUpdate(
                word,
                _ => 1,
                (_, currentValue) => currentValue + 1);
        });
        stopWatch.Stop();

        Console.WriteLine($"Многопоточный способ - {stopWatch.ElapsedMilliseconds} мс");
    }

    private void CountSlow(IEnumerable<string> words)
    {
        var stopWatch = new Stopwatch();

        stopWatch.Start();

        foreach (var word in words)
            _instance.AddOrUpdate(
                word,
                _ => 1,
                (_, currentValue) => currentValue + 1);

        stopWatch.Stop();

        Console.WriteLine($"Обычный способ - {stopWatch.ElapsedMilliseconds} мс");
    }
}
