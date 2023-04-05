namespace Main;

public class WordsDictionary
{
    private readonly Dictionary<string, int> _instance = new();

    public WordsDictionary(IEnumerable<string> words)
    {
        Count(words);
    }

    public IEnumerable<KeyValuePair<string, int>> GetSortedByCount()
    {
        var pairs = _instance.ToList();
        pairs.Sort((firstPair, secondPair) => secondPair.Value.CompareTo(firstPair.Value));
        return pairs;
    }

    private void Count(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (!_instance.ContainsKey(word))
            {
                _instance.Add(word, 1);
            }
            else
            {
                _instance[word] += 1;
            }
        }
    }
}