using System;
using System.Collections.Concurrent;
using System.IO;
using TextFileParser.Interface;

namespace TextFileParser
{
    /// <summary>
    /// FileWriter is responsible to write the finalised wordCountDictionary list.
    /// </summary>
    public class FileWriter : IFileWriter
    {
        /// <summary>
        /// Write to file
        /// </summary>
        /// <param name="fullFilePath">full file path including filename</param>
        /// <param name="wordCountDictionary">reference to populated dictionary list</param>
        public void WriteToFile(string fullFilePath, ref ConcurrentDictionary<string, ulong> wordCountDictionary)
        {
            using (var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                using(StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.WriteLine(string.Format("The Word Counter result For file {0}:{1}", fullFilePath, Environment.NewLine));

                    foreach (var countedWord in wordCountDictionary)
                    {
                        sw.WriteLine(string.Format("{0} is found {1} times", countedWord.Key, countedWord.Value));
                    }
                }
            }
        }
    }
}
