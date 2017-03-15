using Oggy.Transliterator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestLanguageTools
{
    /// <summary>
    ///This is a test class for TransliteratorHelperTest and is intended
    ///to contain all TransliteratorHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TransliteratorHelperTest
    {
        [TestMethod]
        public void AddBreaksTest()
        {
            AddBreaks("to the hell", "|to| |the| |hell|");
            AddBreaks("house", "|house|");
            AddBreaks(" hum", " |hum|");
            AddBreaks("how to do", "|how| |to| |do|");
            AddBreaks("me, you and him!", "|me|, |you| |and| |him|!");
        }

        private void AddBreaks(string input, string output)
        {
            string result = TransliteratorHelper.AddBreaks(input);
            Assert.AreEqual(output, result);
        }
        
        [TestMethod]
        public void RemoveBreaksTest()
        {
            RemoveBreaks("|to| |the| |hell|", "to the hell");
            RemoveBreaks("|house|", "house");
            RemoveBreaks(" |hum|", " hum");
            RemoveBreaks("|hum| ", "hum ");
            RemoveBreaks("|how| |to| |do|", "how to do");
            RemoveBreaks(" ||ham| ", " ham ");
        }

        private void RemoveBreaks(string input, string output)
        {
            string result = TransliteratorHelper.RemoveBreaks(input);
            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public void DetermineTest()
        {
            Determine(WordCapitals.LowerCase, "abcde");
            Determine(WordCapitals.LowerCase, "oggy");
            Determine(WordCapitals.CapitalLetter, "Ognjen");
            Determine(WordCapitals.UpperCase, "ABBA");
            Determine(WordCapitals.UpperCase, "YMCA1");
            Determine(WordCapitals.None, "oGgY");
        }

        private void Determine(WordCapitals capitals, string word)
        {
            Assert.AreEqual(capitals, TransliteratorHelper.Determine(word));
        }
    }
}
