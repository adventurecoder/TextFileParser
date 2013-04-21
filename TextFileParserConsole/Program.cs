using System;
using TextFileParser;

namespace TextFileParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(@"Please type the full file path to the document. Eg. C:\Temp\LargeFile.txt");
                string fullFilePath = Console.ReadLine();
                if (string.IsNullOrEmpty(fullFilePath))
                    fullFilePath = @"C:\Temp\LargeFile.txt";

                Console.WriteLine();
                Console.WriteLine(@"Please type the full file path to the result document. Eg. C:\Temp\Result.txt");
                string resultFullFilePath = Console.ReadLine();
                if (string.IsNullOrEmpty(resultFullFilePath))
                    resultFullFilePath = @"C:\Temp\Result.txt";

                Console.WriteLine();

                var documentWordCounter = new DocumentWordCounter();
                documentWordCounter.ProcessDocument(fullFilePath, resultFullFilePath);

                Console.WriteLine(string.Format("@The file {0} has been processed at {1}. The Word Count results can be found in {2}",
                                        fullFilePath, DateTime.Now, resultFullFilePath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
