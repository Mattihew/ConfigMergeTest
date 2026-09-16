using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ConfigMergeTest.ConfigMerger;

public class ConfigMergerProvider(string arrayPath, IConfigurationProvider provider) : IConfigurationProvider
{
    private int _offset = 0;

    public IChangeToken GetReloadToken() => provider.GetReloadToken();
    public void Load() => provider.Load();


    public void Set(string key, string? value) => provider.Set(MapPath(key), value);

    public bool TryGet(string key, out string? value) => provider.TryGet(MapPath(key), out value);

    private string MapPath(string key)
    {
        if (!key.StartsWith(arrayPath + ':'))
        {
            return key;
        }

        var parts = key.Substring(arrayPath.Length + 1).Split(':', 2);
        parts[0] = int.TryParse(parts[0], out var indexValue) ? (indexValue - _offset).ToString() : parts[0];
        return arrayPath + ':' + string.Join(':', parts);
    }


    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        if (parentPath is null || !parentPath.StartsWith(arrayPath))
        {
            return provider.GetChildKeys(earlierKeys, parentPath);
        }

        var keys = earlierKeys.ToList();
        var keyInts = TryParse(keys).ToList();
        _offset = keyInts.Max() + 1 ?? 0;

        var result = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath);

        var results = keys.Concat(result.Select(key => int.TryParse(key, out var value) ? (value + _offset).ToString() : key));
        return results;
    }

    private IEnumerable<int?> TryParse(IEnumerable<string> input) =>
        input.Select<string, int?>(key => int.TryParse(key, out var value) ? value : null);
}