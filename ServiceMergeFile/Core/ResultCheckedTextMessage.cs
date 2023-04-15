namespace ServiceMergeFile.Core;

internal class ResultCheckedTextMessage : IResultCheckedText
{
    public string Conflict(string source, string modified1, string modified2)
    {
        return string.Join("\n", "conflict source -> " + source, "conflict file 1 -> " + modified1, "conflict file 2 -> " + modified2);

    }

    public string Modified(string source, string modified)
    {
        return "modified -> "  + modified;
    }

    public string Modified1(string source, string modified1, string modified2)
    {
        return "modified file 1 -> " + modified1; 
    }

    public string Modified2(string source, string modified1, string modified2)
    {
        return "modified file 2 -> " + modified2;
    }
}