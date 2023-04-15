using System.Text;

namespace ServiceMergeFile.Core;

internal class FileService : IFileService
{
    public async Task<string[]> ReadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
       

        if (file == null) throw new ArgumentNullException(nameof(file));

        var textResult = new List<string>();


        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream, cancellationToken);
            
        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);


        while (reader.ReadLine() is { } line)
        {
            textResult.Add(line);
        }

        return textResult.ToArray();
    }

    public async Task<IFormFile> WriteFileAsync(string text, string name, string fileName,
        CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();

        await memoryStream.WriteAsync(Encoding.UTF8.GetBytes(text), cancellationToken);

        var fromFile = new FormFile(memoryStream,
            0, memoryStream.Length,
            name,
            fileName);
        fromFile.Headers = new HeaderDictionary();
        fromFile.ContentDisposition = "";

        return fromFile;
    }
}