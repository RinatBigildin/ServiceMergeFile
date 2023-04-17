using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ServiceMergeFile.Core;
using System.Text;
using System.IO;

namespace ServiceMergeFile.Controllers
{
    [ApiController]
    [Route("api/mergefiles")]
    public class MergeFileController: Controller
    {
        private readonly ILogger<MergeFileController> _logger;
        private readonly IMergeFile? _mergeFile;
        private readonly IFileService? _fileService;

        public MergeFileController(IServiceProvider serviceProvider, ILogger<MergeFileController> logger)
        {
            _logger = logger;
            _mergeFile = serviceProvider.GetService<IMergeFile>();
            _fileService = serviceProvider.GetService<IFileService>();

        }

        /// <summary>
        /// Выберите файлы для слияния
        /// </summary>
        /// <param name="source">Исходный файл</param>
        /// <param name="modified1">Измененный файл 1</param>
        /// <param name="modified2">Измененный файл 2</param>
        /// <returns>Файл после слияния</returns>
        [HttpPost("merge")]
        public async Task<IActionResult> Merge([Required]IFormFile source, IFormFile? modified1 = null, IFormFile? modified2 = null, CancellationToken cancellationToken = default)
        {

            if (_mergeFile == null)
            {
                _logger.LogCritical($"Не зарегистрирован сервис {nameof(IMergeFile)}");
                return new StatusCodeResult((int)(HttpStatusCode.FailedDependency));
            }

            if (_fileService == null)
            {
                _logger.LogCritical($"Не зарегистрирован сервис {nameof(IFileService)}");
                return new StatusCodeResult((int)(HttpStatusCode.FailedDependency));
            }

            if (source is null)
            {
                return new BadRequestObjectResult(new { message = "Необходимо обязательно указать исходный файл" });
            }


            if (modified1 is null && modified2 is null)
            {
                return new BadRequestObjectResult( new {message = "Необходимо указать хотя бы один файл для слияния" });
            }

            if (source.ContentType != "text/plain" 
                || 
                (modified1 != null &&  modified1.ContentType != "text/plain")
                || 
                (modified2 != null && modified2.ContentType != "text/plain"))
            {
                return new BadRequestObjectResult(new { message = "Некорректный формат файла" });
            }


            string nameFileResult = "result" + Path.GetExtension(source.FileName);

            var sourceFile = await _fileService.ReadFileAsync(source, cancellationToken);

            var modified1File = modified1 is null
                ? null
                : await _fileService.ReadFileAsync(modified1, cancellationToken);

            var modified2File = modified2 is null
                ? null
                : await _fileService.ReadFileAsync(modified2, cancellationToken);

            var mergeFile = await _mergeFile.MergeAsync(sourceFile, modified1File, modified2File);


            return new File(Encoding.UTF8.GetBytes(mergeFile), "plain/text", nameFileResult);
        }


    }
}
