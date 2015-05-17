using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oggy.Repository;
using Oggy.Repository.Entities;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TestLanguageTools.LanguageRepository
{
    [TestClass]
    public class XMLRepositoryTest
    {
        const string filename = "test_XMLRepository.xml";

        [TestMethod, Ignore]
        public void TestLanguages()
        {
            // Delete the file if it exists
            if (File.Exists(filename))
                File.Delete(filename);

            // Create a file and check there are no languages
            XMLRepository xmlRepository = new XMLRepository(filename);
            Assert.AreEqual(0, xmlRepository.ListLanguages().Count());
            
            // Add English and Serbian and make sure there are 2 languages
            xmlRepository.CreateLanguage(new Language() { Code = "EN", Name = "English", Alphabet = "abcdefghijklmnopqrstuvwxyz" });
            xmlRepository.CreateLanguage(new Language() { Code = "SR", Name = "Srpski" });
            Assert.AreEqual(2, xmlRepository.ListLanguages().Count());

            // Check English is there
            Language English = xmlRepository.RetreiveLanguage("EN");
            Assert.AreEqual("EN", English.Code);
            Assert.AreEqual("English", English.Name);
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", English.Alphabet);

            // Change English
            English.Name = "easy english";
            English.Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            xmlRepository.UpdateLanguage(English);

            // Check that English is updated
            Assert.AreEqual("EN", English.Code);
            Assert.AreEqual("easy english", English.Name);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", English.Alphabet);

            // Delete English and then Serbian
            xmlRepository.DeleteLanguage("EN");
            Assert.AreEqual(1, xmlRepository.ListLanguages().Count());
            xmlRepository.DeleteLanguage("SR");
            Assert.AreEqual(0, xmlRepository.ListLanguages().Count());
        }

        [TestMethod, Ignore]
        public void TestWords()
        {
            // Delete the file if it exists
            if (File.Exists(filename))
                File.Delete(filename);

            // Create a language
            XMLRepository xmlRepository = new XMLRepository(filename);
            Language language = new Language() { Code = "EN", Name="English" };
            xmlRepository.CreateLanguage(language);
            xmlRepository.SetSourceLanguage("EN");
            Assert.AreEqual(0, xmlRepository.ListWords().Count());

            // Add some words
            xmlRepository.CreateWord(new Word() { Name = "repository", status = 1, Translation = "skladiste" });
            xmlRepository.CreateWord(new Word() { Name = "create", status = 1, Translation = "stvorite" });
            xmlRepository.CreateWord(new Word() { Name = "vomit", status = 2, Translation = "povracati" });
            xmlRepository.CreateWord(new Word() { Name = "pissed off", status = 2, Translation = "iznerviran" });
            xmlRepository.CreateWord(new Word() { Name = "urge", status = 3, Translation = "goniti, nagon" });
            Assert.AreEqual(5, xmlRepository.ListWords().Count());

            // Update some of the words
            xmlRepository.UpdateWord(new Word() { Name = "pissed off", status = 3, Translation = "iznervirati" });
            Assert.AreEqual(5, xmlRepository.ListWords().Count());

            // Delete few words
            xmlRepository.DeleteWord("create");
            xmlRepository.DeleteWord("urge");
            Assert.AreEqual(3, xmlRepository.ListWords().Count());
        }
    }
}