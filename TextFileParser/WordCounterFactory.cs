using System.Collections.Concurrent;
using TextFileParser.Interface;

namespace TextFileParser
{
    public class WordCounterFactory : IWordCounterFactory
    {
        public IWordCounter CreateWordCounter()
        {
            return new WordCounter();
        }
    }
}
