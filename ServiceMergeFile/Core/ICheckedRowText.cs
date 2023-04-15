namespace ServiceMergeFile.Core;

/// <summary>
/// Сервис сравнения строк
/// </summary>
internal interface ICheckedRowText
{
    /// <summary>
    /// Сравнение двух строк
    /// </summary>
    /// <param name="sourceText">исходная строка</param>
    /// <param name="modifiedText">модифицированная строка</param>
    /// <returns></returns>
    bool Equals(string sourceText, string modifiedText);
}