using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TextFileParser.Interface;

namespace TextFileParser
{
    /// <summary>
    /// WordCounter is responsible to take word from list, add the word into dictionary and increment the word count
    /// </summary>
    public class WordCounter : IWordCounter
    {
        /// <summary>
        /// Add and increment the word in the dictionary
        /// </summary>
        /// <param name="wordsToCount">reference to collection of words</param>
        /// <param name="wordCountDictionary">reference to resulting dictionary</param>
        public void IncrementCount(ref BlockingCollection<string> wordsToCount, ref ConcurrentDictionary<string, ulong> wordCountDictionary)
        {
            string word;
            bool keepCounting = wordsToCount.TryTake(out word);
            while (keepCounting)
            {
                string sanitisedWord = StringHelper.RemoveSpecialCharacters(word);

                if (!wordCountDictionary.ContainsKey(sanitisedWord))
                {
                    wordCountDictionary.TryAdd(sanitisedWord, 0);
                }

                ulong count = wordCountDictionary[sanitisedWord] + 1;
                wordCountDictionary[sanitisedWord] = count;

                keepCounting = wordsToCount.TryTake(out word);

                if (!keepCounting)
                {
                    Thread.Sleep(3000);
                    keepCounting = wordsToCount.TryTake(out word);
                }
            }
        }
    }
}
