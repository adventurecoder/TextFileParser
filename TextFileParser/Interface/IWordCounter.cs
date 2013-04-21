using System;
using System.Collections.Concurrent;
namespace TextFileParser.Interface
{
    public interface IWordCounter
    {
        void IncrementCount(ref BlockingCollection<string> wordsToCount, ref ConcurrentDictionary<string, ulong> wordCountDictionary);
    }
}
