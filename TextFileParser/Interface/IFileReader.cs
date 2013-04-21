using System;
using System.Collections.Concurrent;
namespace TextFileParser.Interface
{
    public interface IFileReader
    {
        void Read(string fullFilePath, ref BlockingCollection<string> wordsToCount);
        long GetFileSize(string fullFilePath);
    }
}
