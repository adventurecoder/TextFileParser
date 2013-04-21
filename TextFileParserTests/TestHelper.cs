using System;
using System.IO;

namespace TextFileParserTests
{
    public static class TestHelper
    {
        private const string _FirstDictionaryName = "TheGutenbergWebstersUnabridgedDictionary.txt";
        private const string _SecondDictionaryName = "WebstersUnabridgedDictionary.txt";

        /// <summary>
        /// Generate a large Text file containing natural words by combining dictionaries multiple times. 
        /// If the sizeInMb is not large enough, it will at least generate a 71.2MB file. 
        /// </summary>
        /// <param name="resultFileFullPath">The full file path for resulting file</param>
        /// <param name="approxSizeInMb">Approximate Size in Megabytes</param>
        public static void GenerateLargeTestTextFile(string resultFileFullPath, int approxSizeInMb)
        {
            if (!File.Exists(resultFileFullPath))
            {
                var resultDirectory = resultFileFullPath.Substring(0, resultFileFullPath.LastIndexOf(@"\"));
                
                if (!Directory.Exists(resultDirectory))
                    Directory.CreateDirectory(resultDirectory);

                const int chunkSize = 2 * 1024; // 2KB
                var inputFiles = new[] { _FirstDictionaryName, _SecondDictionaryName };
                var currentDirectory = string.Format(@"{0}\GutenbergFreeEBooks\", Environment.CurrentDirectory);
                var repeatTimes = (int)(approxSizeInMb / 71.2m) + 1;
                var count = 0;

                using (var output = File.Create(resultFileFullPath))
                {
                    while (count < repeatTimes)
                    {
                        foreach (var file in inputFiles)
                        {
                            using (var input = File.OpenRead(Path.Combine(currentDirectory, file)))
                            {
                                var buffer = new byte[chunkSize];
                                int bytesRead;
                                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    output.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                        count++;
                    }
                }
            }
        }

        public static void DeleteTestFile(string fileFullPath)
        {
            if(File.Exists(fileFullPath))
                File.Delete(fileFullPath);
        }
    }
}
