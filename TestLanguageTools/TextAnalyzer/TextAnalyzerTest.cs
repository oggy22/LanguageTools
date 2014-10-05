using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextAnalyzer;


namespace TestLanguageTools.TextAnalyzerTest
{
    [TestClass]
    public class TextAnalyzerTest
    {
        [TestMethod]
        public void TestAnalyzer()
        {
            string text = "John's is john IS. John is good-looking.";

            TextAnalyzerWorker textAnalyzer = new TextAnalyzerWorker(text);
            // Word 1
            var word1 = textAnalyzer.all_words[0];
            Assert.AreEqual(2, word1["john"]);
            Assert.AreEqual(1, word1["john's"]);
            Assert.AreEqual(3, word1["is"]);
            Assert.AreEqual(1, word1["good-looking"]);

            // Word 2
            var word2 = textAnalyzer.all_words[1];
            Assert.AreEqual(2, word2["john is"]);
            Assert.AreEqual(1, word2["is good-looking"]);

            // Capital words
            var capitals = textAnalyzer.capitalWords;
            Assert.AreEqual(1, capitals["John's"]);
            Assert.AreEqual(1, capitals["IS"]);
        }
    }
}