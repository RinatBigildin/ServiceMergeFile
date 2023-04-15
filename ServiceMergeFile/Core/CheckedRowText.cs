namespace ServiceMergeFile.Core;

internal class CheckedRowText : ICheckedRowText
{
    public bool Equals(string sourceText, string modifiedText)
    {
        return sourceText.TrimStart().TrimEnd()
            .Equals(modifiedText.TrimStart().TrimEnd(), StringComparison.OrdinalIgnoreCase);
    }
}