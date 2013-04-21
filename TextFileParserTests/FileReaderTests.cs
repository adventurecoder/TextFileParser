using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextFileParser;
using System.IO;
using System.Collections.Concurrent;
using System;

namespace TextFileParserTests
{
    [TestClass]
    public class FileReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Read_InvalidFullFilePath_ThrowException()
        {
            var wordsToCount = new BlockingCollection<string>();
            FileReader fileReader = new FileReader();
            fileReader.Read(@"C:\DummyLocation", ref wordsToCount);
        }

        [TestMethod]
        [Ignore]
        public void Read_ValidFullFilePath_Success()
        {
            string fullFilePathToRead = @"C:\Temp\NormalFile.txt";

            TestHelper.GenerateLargeTestTextFile(fullFilePathToRead, 300);
            var wordsToCount = new BlockingCollection<string>();

            FileReader fileReader = new FileReader();
            fileReader.Read(fullFilePathToRead, ref wordsToCount);
        }

        [TestMethod]
        public void GetFileSize_ReturnFileSizeInBytes()
        {
            FileReader fileReader = new FileReader();
            long fileSize = fileReader.GetFileSize(string.Format(@"{0}\GutenbergFreeEBooks\WebstersUnabridgedDictionary.txt", Environment.CurrentDirectory));
            Assert.AreEqual(28956348, fileSize, "Test File size must be 28956348 bytes");
        }
    }
}
