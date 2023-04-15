using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("ServiceMergeFile.Tests")]

namespace ServiceMergeFile.Core
{
    internal class MergeFile: IMergeFile
    {
        private readonly ICheckedRowText _checkedRowText;
        private readonly IResultCheckedText _resultModifiedText;

        public MergeFile(ICheckedRowText checkedRowText, IResultCheckedText resultModifiedText)
        {
            _checkedRowText = checkedRowText;
            _resultModifiedText = resultModifiedText;
        }

        public Task<string> MergeAsync(string[] sourceText, string[] modifiedText1, string[] modifiedText2)
        {
            if (sourceText is null)
            {
                throw new ArgumentNullException("Необходимо указать исходный текст");
            }
            if (modifiedText1 is null && modifiedText2 is null)
            {
                throw new ArgumentNullException("Необходимо указать хотя бы один параметр с текстом для слияния");
            }

            if (modifiedText1 is not null && modifiedText2 is not null)
            {
                return Task.Run(() => Merge(sourceText, modifiedText1, modifiedText2));
            }

            if (modifiedText1 is not null)
            {
                return Task.Run(() => Merge(sourceText, modifiedText1));
            }

            if (modifiedText2 is not null)
            {
                return Task.Run(() => Merge(sourceText, modifiedText2));
            }

            return Task.FromResult(string.Join("/n", sourceText));
        }

        /// <summary>
        /// Объединение двух текстов с исходным
        /// </summary>
        /// <param name="sourceText">исходный текст</param>
        /// <param name="modifiedText1">изменнные текст 1</param>
        /// <param name="modifiedText2">изменнные текст 2</param>
        /// <returns></returns>
        private string Merge(string[] sourceText, string[] modifiedText1, string[] modifiedText2)
        {


            bool Conflict(string source, string modified1, string modified2)
            {
                return !_checkedRowText.Equals(modified1, modified2) &&
                       !_checkedRowText.Equals(source, modified1)
                       && !_checkedRowText.Equals(source, modified2);
            }

            bool IdenticalModifiedText(string source, string modified1, string modified2)
            {
                return _checkedRowText.Equals(modified1, modified2) &&
                       !_checkedRowText.Equals(source, modified1)
                       && !_checkedRowText.Equals(source, modified2);
            }


            bool ModifiedText1(string source, string modified1, string modified2)
            {
                return !_checkedRowText.Equals(source, modified1)
                       && _checkedRowText.Equals(source, modified2);
            }

            bool ModifiedText2(string source, string modified1, string modified2)
            {
                return _checkedRowText.Equals(source, modified1)
                       && !_checkedRowText.Equals(source, modified2);
            }

            var maxRow = new[] { sourceText.Length, modifiedText1.Length, modifiedText2.Length }.Max();
            var resultBuilder = new StringBuilder(maxRow);

            for (int i = 0; i < maxRow; i++)
            {
                var sourceRow = GetRowForIndex(sourceText, i);
                var modifiedRow1 = GetRowForIndex(modifiedText1, i);
                var modifiedRow2 = GetRowForIndex(modifiedText2, i);

                if (Conflict(sourceRow, modifiedRow1, modifiedRow2))
                {
                    resultBuilder.AppendLine(_resultModifiedText.Conflict(sourceRow, modifiedRow1, modifiedRow2));
                    continue;

                }
                if (IdenticalModifiedText(sourceRow, modifiedRow1, modifiedRow2))
                {
                    resultBuilder.AppendLine(_resultModifiedText.Modified1(sourceRow, modifiedRow1, modifiedRow2));
                    continue;
                }
                if (ModifiedText1(sourceRow, modifiedRow1, modifiedRow2))
                {
                    resultBuilder.AppendLine(_resultModifiedText.Modified1(sourceRow, modifiedRow1, modifiedRow2));
                    continue;
                }

                if (ModifiedText2(sourceRow, modifiedRow1, modifiedRow2))
                {
                    resultBuilder.AppendLine(_resultModifiedText.Modified2(sourceRow, modifiedRow1, modifiedRow2));
                    continue;
                }

                resultBuilder.AppendLine(sourceRow);
            }


            return resultBuilder.ToString();

        }

        /// <summary>
        /// Объединение текста с исходным
        /// </summary>
        /// <param name="sourceText">исходный текст</param>
        /// <param name="modifiedText">изменнные текст 1</param>
        /// <returns></returns>
        private string Merge(string[] sourceText, string[] modifiedText)
        {

            bool IdenticalModifiedText(string source, string modified)
            {
                return _checkedRowText.Equals(source, modified);
            }


            var maxRow = new[] { sourceText.Length, modifiedText.Length}.Max();
            var resultBuilder = new StringBuilder(maxRow);

            for (int i = 0; i < maxRow; i++)
            {
                var sourceRow = GetRowForIndex(sourceText, i);
                var modifiedRow = GetRowForIndex(modifiedText, i);

                if (IdenticalModifiedText(sourceRow, modifiedRow))
                {
                    resultBuilder.AppendLine(sourceRow);
                }
                else
                {
                    resultBuilder.AppendLine(_resultModifiedText.Modified(sourceRow, modifiedRow));
              
                }
         

            }


            return resultBuilder.ToString();

        }



        private string GetRowForIndex(string[] source, int index) =>
            index >= source.Length ? "" : source[index];


    }
}
