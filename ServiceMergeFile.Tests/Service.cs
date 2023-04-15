namespace ServiceMergeFile.Tests
{
    public static class Service
    {
        public static string[] ReadFile(string path)
        {
            var text = new List<string>();
            using (var reader = new StreamReader(path))
            {
                while (reader.ReadLine() is { } line)
                {
                    text.Add(line);
                }
            }
            return text.ToArray();
        }

        public static void WriteFile(string path, string text)
        {
            using var writer = new StreamWriter(path, false);
            writer.WriteLine(text);
        }




    }
}
