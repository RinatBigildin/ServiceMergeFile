namespace ServiceMergeFile.Core;

/// <summary>
/// Сервис обработки результата сравнения строк
/// </summary>
internal interface IResultCheckedText
{
    /// <summary>
    /// Обработка конфликта
    /// </summary>
    string Conflict(string source, string modified1, string modified2);
    /// <summary>
    /// Обработка, если строка источник отличается от модифицированной
    /// </summary>
    string Modified(string source, string modified);
    /// <summary>
    ///  Обработка, если строка источник отличается от модифицированной строки 1
    /// </summary>
    string Modified1(string source, string modified1, string modified2);
    /// <summary>
    ///  Обработка, если строка источник отличается от модифицированной строки 2
    /// </summary>
    string Modified2(string source, string modified1, string modified2);
}