using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextFileParser.Interface;

[assembly: InternalsVisibleTo("TextFileParserTests")]
namespace TextFileParser
{
    public class DocumentWordCounter
    {
        private ConcurrentDictionary<string, ulong> _WordCountDictionary = new ConcurrentDictionary<string, ulong>();
        private BlockingCollection<string> _WordsToCount = new BlockingCollection<string>();

        private IFileReader _FileReader;
        private IFileWriter _FileWriter;
        private IWordCounterFactory _wordCounterFactory;

        public DocumentWordCounter(IFileReader fileReader, IFileWriter fileWriter, IWordCounterFactory wordCounterFactory)
        {
            _FileReader = fileReader;
            _FileWriter = fileWriter;
            _wordCounterFactory = wordCounterFactory;
        }

        public DocumentWordCounter()
            : this(new FileReader(), new FileWriter(), new WordCounterFactory())
        { }

        public void ProcessDocument(string fullFilePath, string resultFullFilePath)
        {
            var watch = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>();

            // Populate wordsToCount sequentially
            Console.WriteLine(string.Format("Start to read from {0} at {1}", fullFilePath, DateTime.Now));
            var populateWords = Task.Factory.StartNew(() => ReadFileAndPopulateWords(fullFilePath));
            tasks.Add(populateWords);

            // Parallel instantiation of WordCounter based on the core
            int parallelTaskCount = Environment.ProcessorCount;
            Console.WriteLine(string.Format("Parallel word counters = {0}\r\n", parallelTaskCount));
            Thread.Sleep(2000); // Give sometime for reader to start inserting wordsToCount

            for (int i = 0; i < parallelTaskCount; i++)
            {
                if (_WordsToCount.Count > 0)
                {
                    var countWords = Task.Factory.StartNew(() => CreateAndStartWordCounter());
                    tasks.Add(countWords);
                }
            }

            // After all tasks completed, write result file and stop timer
            Task[] taskArray = tasks.ToArray();
            Task.Factory.ContinueWhenAll(taskArray, result => WriteFile(resultFullFilePath), TaskContinuationOptions.ExecuteSynchronously);

            Task.WaitAll(taskArray);
            Console.WriteLine(string.Format("Process is completed at {0} with duration of {1}", DateTime.Now, watch.Elapsed.ToString()));
        }

        internal void ReadFileAndPopulateWords(string fullFilePath)
        {
            _FileReader.Read(fullFilePath, ref _WordsToCount);
        }

        private void CreateAndStartWordCounter()
        {
            var wordCounter = _wordCounterFactory.CreateWordCounter();
            wordCounter.IncrementCount(ref _WordsToCount, ref _WordCountDictionary);
        }

        private void WriteFile(string resultFullFilePath)
        {
            _FileWriter.WriteToFile(resultFullFilePath, ref _WordCountDictionary);
        }
    }
}
