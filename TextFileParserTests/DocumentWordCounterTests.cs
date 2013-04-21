using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextFileParser;
using TextFileParser.Interface;
using Rhino.Mocks;
using System.Collections.Concurrent;

namespace TextFileParserTests
{
    [TestClass]
    public class DocumentWordCounterTests
    {
        [TestMethod]
        public void ProcessDocument_VerifyAllExpectations()
        {
            // Arrange
            var fileReader = MockRepository.GenerateStrictMock<IFileReader>();
            var fileWriter = MockRepository.GenerateStrictMock<IFileWriter>();
            var wordCounterFactory = MockRepository.GenerateStrictMock<IWordCounterFactory>();
            var wordCounter = MockRepository.GenerateStrictMock<IWordCounter>();
            string testFullFilePath = @"D:\Test\Read.txt";
            string testResultFullFilePath = @"D:\Test\Result.txt";
            var wordsToCount = new BlockingCollection<string>() {"abc"};
            var wordCountDictionary = new ConcurrentDictionary<string, ulong>();

            fileReader.Expect(fr => fr.Read(testFullFilePath, ref wordsToCount)).IgnoreArguments();
            fileWriter.Expect(fw => fw.WriteToFile(testResultFullFilePath, ref wordCountDictionary));
            wordCounterFactory.Expect(wcf => wcf.CreateWordCounter()).Repeat.Any().Return(wordCounter);
            wordCounter.Expect(wc => wc.IncrementCount(ref wordsToCount, ref wordCountDictionary)).Repeat.Any();

            // Act
            var documentWordCounter = new DocumentWordCounter(fileReader, fileWriter, wordCounterFactory);
            documentWordCounter.ProcessDocument(testFullFilePath, testResultFullFilePath);

            // Assert
            fileReader.VerifyAllExpectations();
            fileWriter.VerifyAllExpectations();
            wordCounterFactory.VerifyAllExpectations();
            wordCounter.VerifyAllExpectations();
        }

        [TestMethod]
        public void ReadFileAndPopulateWords_CallFileReader()
        {
            // Arrange
            var fileReader = MockRepository.GenerateStrictMock<IFileReader>();
            var fileWriter = MockRepository.GenerateStrictMock<IFileWriter>();
            var wordCounterFactory = MockRepository.GenerateStrictMock<IWordCounterFactory>();
            string testFullFilePath = "D:\\Test\\Read.txt";
            var wordsToCount = new BlockingCollection<string>() { "abc" };

            fileReader.Expect(fr => fr.Read(testFullFilePath, ref wordsToCount)).IgnoreArguments();

            // Act
            var documentWordCounter = new DocumentWordCounter(fileReader, fileWriter, wordCounterFactory);
            documentWordCounter.ReadFileAndPopulateWords(testFullFilePath);

            fileReader.VerifyAllExpectations();
        }
    }
}
