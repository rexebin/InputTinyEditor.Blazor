namespace InputTinyMCEEditor.Blazor;

internal static class Helper
{
    // Borrowed from original source code at TinyMCE.Blazor.Editor
    internal static Dictionary<string, object> Merge(params Dictionary<string, object>[] dictionaries)
    {
        Dictionary<string, object> merged = new();
        foreach (Dictionary<string, object> d in dictionaries)
        {
            d.ToList().ForEach(pair => merged[pair.Key] = pair.Value);
        }

        return merged;
    }
}