using System;
using System.Collections.Concurrent;
namespace TextFileParser.Interface
{
    public interface IFileWriter
    {
        void WriteToFile(string fullFilePath, ref ConcurrentDictionary<string, ulong> wordCountDictionary);
    }
}
