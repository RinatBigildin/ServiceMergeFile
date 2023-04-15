using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceMergeFile.Core;

namespace ServiceMergeFile.Tests
{
    [TestClass]   
    public class FileServiceTests
    {
        [TestMethod]
        public void Can_Read_File_To_IFromFile_Convert_To_ArrayString()
        {
            var pathFile = "..\\..\\..\\source.txt";

           var stream = new StreamReader(pathFile);

            var sourceFileByPath = Service.ReadFile(pathFile);

            var fromFile = new FormFile(stream.BaseStream, 
                0, stream.BaseStream.Length, 
                "text/plain", 
                "source.txt");
            fromFile.Headers = new HeaderDictionary();
            fromFile.ContentDisposition = "";
            var fileService = new FileService();

            var sourceFileByFromFile = fileService.ReadFileAsync(fromFile, CancellationToken.None).GetAwaiter().GetResult();

            Assert.AreEqual(sourceFileByFromFile.Length, sourceFileByPath.Length);
           
            for (int i = 0; i < sourceFileByFromFile.Length; i++)
            {
                Assert.AreEqual(sourceFileByFromFile[i], sourceFileByPath[i]);

            }

        }

        [TestMethod]
        public void Can_Write_File_To_IFrom_Convert_To_ArrayString()
        {
            //arrnge
            var pathFile = "..\\..\\..\\source.txt";

            var stream = new StreamReader(pathFile);

            var sourceFileByPath = Service.ReadFile(pathFile);

            var stringBuilder = new StringBuilder(sourceFileByPath.Length);

            foreach (var s in sourceFileByPath)
            {
                stringBuilder.AppendLine(s);
            }

            var fileService = new FileService();

            //action
            var fromFile = fileService.WriteFileAsync(stringBuilder.ToString(), "text/plain", "mergeFile.txt", CancellationToken.None).GetAwaiter().GetResult();

            var sourceFileByFromFile = fileService.ReadFileAsync(fromFile, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.AreEqual(sourceFileByFromFile.Length, sourceFileByPath.Length);

            for (int i = 0; i < sourceFileByFromFile.Length; i++)
            {
                Assert.AreEqual(sourceFileByFromFile[i], sourceFileByPath[i]);

            }

        }
    }
}
