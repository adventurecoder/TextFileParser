using System;
using System.Collections.Concurrent;

namespace TextFileParser.Interface
{
    public interface IWordCounterFactory
    {
        IWordCounter CreateWordCounter();
    }
}
