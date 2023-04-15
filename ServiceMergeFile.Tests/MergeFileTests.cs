using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceMergeFile.Core;

namespace ServiceMergeFile.Tests
{
    [TestClass]
    public class MergeFileTests
    {
        [TestMethod]
        [
                DataRow(
                    new[] { "    Hello World    " },
                    new[] { "    Hello World!   " },
                    new[] { "    Hello Worlds    " },
                    "Конфликт"),

            DataRow(
                new[] { "    Hello World    " },
                new[] { "    Hello World!   " },
                new[] { "    Hello World    " },
                "Hello World!"),
            DataRow(
                new[] { "    Hello World    " },
                new[] { "    Hello World   " },
                new[] { "    Hello Worlds    " },
                "Hello Worlds"),
        DataRow(
        new[] { "    Hello World    " },
        new[] { "    Hello World   ", "  The Good " },
        new[] { "    Hello Worlds    " },
        "The Good "),
                DataRow(
                    new[] { "    Hello World    " },
                    new string[] {},
                    new string[] { },
                    "") // здесь должно быть пусто!!!!
        ]
        public void Can_Merge_Text( string[] source, string[] modified1, string[] modified2, string assertResult)
        {
            //arrange
            //action
            IMergeFile mergeFile = new MergeFile(new CheckedRowText(), new ResultCheckedText());
            var result = mergeFile.MergeAsync(source, modified1, modified2)
                .GetAwaiter().GetResult();
            //assert
            Assert.IsTrue(result.Contains(assertResult));
        }

        [TestMethod]
        public void Can_Merge_Two_File()
        {
            //arrange
            IMergeFile mergeFile = new MergeFile(new CheckedRowText(), new ResultCheckedText());
            var source = Service.ReadFile("..\\..\\..\\source.txt");
            var modified1 = Service.ReadFile("..\\..\\..\\modified1.txt");
            var modified2 = Service.ReadFile("..\\..\\..\\modified2.txt");
            //action
            var result = mergeFile.MergeAsync(source, modified1, modified2)
                .GetAwaiter().GetResult(); 

            Service.WriteFile("..\\..\\..\\result.txt", result);

        }
        [TestMethod]
        public void Can_Merge_One_File()
        {
            //arrange
            IMergeFile mergeFile = new MergeFile(new CheckedRowText(), new ResultCheckedTextMessage());
            var source = Service.ReadFile("..\\..\\..\\source.txt");
            var modified1 = Service.ReadFile("..\\..\\..\\modified1.txt");
            //action
            var result = mergeFile.MergeAsync(source, modified1, null)
                .GetAwaiter().GetResult();

            Service.WriteFile("..\\..\\..\\result.txt", result);

        }

    }
}
