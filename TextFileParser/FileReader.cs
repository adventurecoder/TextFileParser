using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using TextFileParser.Interface;

namespace TextFileParser
{
    /// <summary>
    /// FileReader is responsible to read file from the path, extract words and insert into list
    /// </summary>
    public class FileReader : IFileReader
    {
        /// <summary>
        /// Read the text file and extract words into list
        /// </summary>
        /// <param name="fullFilePath">full file path including name</param>
        /// <param name="wordsToCount">reference to thread safe list</param>
        public void Read(string fullFilePath, ref BlockingCollection<string> wordsToCount)
        {
            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException(@"The full file path supplied is incorrect, try C:\FolderName\FileName.txt");

            using (var fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    string currentLine = sr.ReadLine();

                    while (currentLine != null)
                    {
                        string[] words = currentLine.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        int wordCount = words.Length;

                        // Add words to the BlockingCollection to be counted
                        for (int i = 0; i < wordCount; i++)
                        {
                            wordsToCount.Add(words[i]);
                        }

                        while (wordsToCount.Count > 45357000)
                        {
                            Thread.Sleep(1000);
                        }

                        currentLine = sr.ReadLine();
                    }
                    sr.DiscardBufferedData();
                }
            }
        }

        public long GetFileSize(string fullFilePath)
        {
            FileInfo fileInfo = new FileInfo(fullFilePath);
            return fileInfo.Length;
        }
    }
}
