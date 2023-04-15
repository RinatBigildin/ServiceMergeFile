using System;
using System.Reflection.PortableExecutable;

namespace ServiceMergeFile.Core
{
    /// <summary>
    /// Сервис обработки файла
    /// </summary>
    internal interface IFileService
    {
        Task<string[]> ReadFileAsync(IFormFile file, CancellationToken cancellationToken);
        Task<IFormFile> WriteFileAsync(string text, string name, string fileName, CancellationToken cancellationToken);
    }
}
