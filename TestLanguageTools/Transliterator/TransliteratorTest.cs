using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oggy.Transliterator;
using Oggy.Repository;
using Oggy.Repository.Entities;
using Moq;

namespace TestLanguageTools.Transliterator
{
    [TestClass]
    public class TransliteratorTest
    {
        #region Helper methods
        private ARepository CreateMockRepository()
        {
            var mock = new Mock<ARepository>();
            mock.Setup(x => x.RetreiveLanguage("EN")).Returns(new Language() { Code = "EN", Name = "English" });
            mock.Setup(x => x.RetreiveLanguage("EN")).Returns(new Language() { Code = "SR", Name = "Serbian" });
            mock.Setup(x => x.GetJokers()).Returns(new Dictionary<char, string> {{ '#', "qwrtpsdfghjklzxcvbnm" }});
            List<TransliterationRule> rules = new List<TransliterationRule>();
            rules.Add(new TransliterationRule("ai", "ej"));
            rules.Add(new TransliterationRule("a", "e"));
            rules.Add(new TransliterationRule("i", "aj"));
            rules.Add(new TransliterationRule("c", "s"));
            rules.Add(new TransliterationRule("a#e", "ej#"));
            rules.Add(new TransliterationRule("i#e", "aj#"));
            
            //TODO: add this one when multiple occurance of the same joker is supported
            //rules.Add(new TransliterationRule("#i#e", "#aj#"));    // two jokers of the same type
            
            mock.Setup(x => x.ListRules(true)).Returns(rules);
            return mock.Object;
        }
        #endregion

        #region Tests
        /// <summary>
        ///A test for match
        ///</summary>
        [TestMethod()]
        public void MatchTest()
        {
            Oggy.Transliterator.Transliterator target = new Oggy.Transliterator.Transliterator(null);

            // Add jokers
            target.jokers.Add('@', "aeiuo");
            target.jokers.Add('#', "qwrtpsdfghjklzxcvbnm");

            //No jokers test
            Assert.IsTrue(target.Match("x", "x"));
            Assert.IsTrue(target.Match("abc", "abc"));
            Assert.IsFalse(target.Match("abc", "ab"));
            Assert.IsFalse(target.Match("abb", "abce"));

            //Jokers
            Assert.IsTrue(target.Match("a#e", "ake"));
            Assert.IsTrue(target.Match("a#e", "ake"));
            Assert.IsFalse(target.Match("a#e", "aoe"));
        }

        /// <summary>
        ///A test for Transliterator Constructor
        ///</summary>
        [TestMethod()]
        public void TransliteratorConstructorTest()
        {
            var target = new Oggy.Transliterator.Transliterator(CreateMockRepository(), "EN", "SR");
            Assert.AreEqual("mejn", target["main"]);
            Assert.AreEqual("ajd", target["id"]);
            Assert.AreEqual("Ej", target.TransliterateFinal("Ai"));
            Assert.AreEqual("EJ", target.TransliterateFinal("AI"));
            Assert.AreEqual("hrom", target.TransliterateFinal("hrom"));
            Assert.AreEqual("mejk", target.TransliterateFinal("make"));
            Assert.AreEqual("najs", target.TransliterateFinal("nice"));
            Assert.AreEqual("", target[""]);

            // With Capital letters            
            Assert.AreEqual("Najs", target.TransliterateFinal("Nice"));
        }
        #endregion
    }
}