namespace ServiceMergeFile.Core;

internal class ResultCheckedText : IResultCheckedText
{
    public string Conflict(string source, string modified1, string modified2)
    {
        return "Конфликт -> " + source;
    }

    public string Modified(string source, string modified)
    {
        return modified;
    }

    public string Modified1(string source, string modified1, string modified2)
    {
        return modified1;
    }

    public string Modified2(string source, string modified1, string modified2)
    {
        return modified2;
    }
}