namespace ServiceMergeFile.Core
{
    /// <summary>
    /// Сервис слияние файлов
    /// </summary>
    internal interface IMergeFile
    {
        Task<string> MergeAsync(string[] sourceText, string[] modifiedText1, string[] modifiedText2);
    }
}
