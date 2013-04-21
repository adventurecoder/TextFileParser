using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextFileParser;

namespace TextFileParserTests
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void RemoveSpecialCharacters_ReturnSanitisedString()
        {
            string testString = @"!@£$^$@^£$*A+.<>?B[]\±\";
            Assert.AreEqual("AB", StringHelper.RemoveSpecialCharacters(testString), "All the special characters must be removed");
        }
    }
}
