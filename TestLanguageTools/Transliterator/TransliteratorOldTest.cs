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
    public class TransliteratorOldTest
    {
        #region Tests
        /// <summary>
        ///A test for match
        ///</summary>
        [TestMethod()]
        public void MatchTest()
        {
            Oggy.Transliterator.TransliteratorOld target = new Oggy.Transliterator.TransliteratorOld(null);

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
            var target = new Oggy.Transliterator.TransliteratorOld(MockRepository.Create(), "EN", "SR");
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