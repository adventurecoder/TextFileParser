using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextFileParser;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TextFileParserTests
{
    [TestClass]
    public class WordCounterTests
    {
        [TestMethod]
        public void IncrementCount_NewWord_AddToDictionary()
        {
            var testWord = "abc";
            var wordsToCount = new BlockingCollection<string>() {testWord};
            var wordCountDictionary = new ConcurrentDictionary<string, ulong>();

            var wordCounter = new WordCounter();
            wordCounter.IncrementCount(ref wordsToCount, ref wordCountDictionary);

            Assert.IsTrue(wordCountDictionary.ContainsKey(testWord), "abc must be added to wordCountDictionary");
            Assert.AreEqual((ulong)1, wordCountDictionary[testWord], "The word count for abc must be 1");
        }

        [TestMethod]
        public void IncrementCount_ExistingWord_IncrementCount()
        {
            var testWord = "abc";
            var wordsToCount = new BlockingCollection<string>() { testWord };
            var wordCountDictionary = new ConcurrentDictionary<string, ulong>();
            wordCountDictionary.TryAdd(testWord, 1);

            var wordCounter = new WordCounter();
            wordCounter.IncrementCount(ref wordsToCount, ref wordCountDictionary);

            Assert.IsTrue(wordCountDictionary.ContainsKey(testWord), "abc must be added to wordCountDictionary");
            Assert.AreEqual((ulong)2, wordCountDictionary[testWord], "The word count for abc must be 2");
        }

        [TestMethod]
        public void IncrementCount_MultipleWordCounters_CorrectCount()
        {
            var testWord0 = "abc";
            var testWord1 = "def";
            var testWord2 = "0123456789";
            var testWord3 = "xyz";
            var wordsToCount = new BlockingCollection<string>() { testWord0, testWord1, testWord2, testWord3, 
                                                                  testWord1, testWord2, testWord3, 
                                                                  testWord2, testWord3,
                                                                  testWord3
                                                                };
            var wordCountDictionary = new ConcurrentDictionary<string, ulong>();

            List<Task> tasks = new List<Task>();

            var countWords = Task.Factory.StartNew(() =>
            {
                var wordCounter = new WordCounter();
                wordCounter.IncrementCount(ref wordsToCount, ref wordCountDictionary);
            });
            tasks.Add(countWords);

            Task.WaitAll(tasks.ToArray());

            Assert.IsTrue(wordCountDictionary.Count == 4, "There must be 4 words in wordCountDictionary");
            Assert.AreEqual((ulong)1, wordCountDictionary[testWord0], string.Format("The word count for {0} must be 1", testWord0));
            Assert.AreEqual((ulong)2, wordCountDictionary[testWord1], string.Format("The word count for {0} must be 1", testWord1));
            Assert.AreEqual((ulong)3, wordCountDictionary[testWord2], string.Format("The word count for {0} must be 1", testWord2));
            Assert.AreEqual((ulong)4, wordCountDictionary[testWord3], string.Format("The word count for {0} must be 1", testWord3));
        }
    }
}
