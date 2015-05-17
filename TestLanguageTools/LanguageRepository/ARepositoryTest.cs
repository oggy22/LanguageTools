using Oggy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Oggy.Repository;
using Oggy.Repository.Entities;
using System.Collections.Generic;
using Moq;
using Oggy.Transliterator;

namespace TransliteratorTest.LanguageRepository
{
    /// <summary>
    ///This is a test class for ARepositoryTest and is intended
    ///to contain all ARepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ARepositoryTest
    {
        internal virtual ARepository CreateARepository()
        {
            var mock = new Mock<ARepository>();
            mock.Setup(x => x.RetreiveLanguage("EN")).Returns(new Language() { Code = "EN", Name = "English" });
            mock.Setup(x => x.RetreiveLanguage("SR")).Returns(new Language() { Code = "SR", Name = "Serbian" });
            List<TransliterationRule> rules = new List<TransliterationRule>();
            rules.Add(new TransliterationRule() {Source="ai", Destination="ej"});
            rules.Add(new TransliterationRule() {Source="i", Destination="aj"});
            mock.Setup(x => x.ListRules(true)).Returns(rules);
            return mock.Object;
        }

        [TestMethod, Ignore]
        public void SetSrcLanguageTest()
        {
            ARepository target = CreateARepository();
            target.SetSourceLanguage("EN");
            Language language = target.srcLanguage;
            Assert.AreEqual("EN", language.Code);
            Assert.AreEqual("English", language.Name);
        }

        [TestMethod]
        public void CreateLanguageTest()
        {
            ARepository target = CreateARepository();
            target.SetDstLanguage("SR");
            Language language = target.dstLanguage;
            Assert.AreEqual("SR", language.Code);
            Assert.AreEqual("Serbian", language.Name);
        }

        [TestMethod, Ignore]
        public void SetUpTest()
        {
            ARepository target = CreateARepository();
            target.SetUp();
            Language language = target.srcLanguage;
            Assert.AreEqual("EN", language.Code);
            Assert.AreEqual("English", language.Name);
            language = target.dstLanguage;
            Assert.AreEqual("SR", language.Code);
            Assert.AreEqual("Serbian", language.Name);
        }
    }
}
