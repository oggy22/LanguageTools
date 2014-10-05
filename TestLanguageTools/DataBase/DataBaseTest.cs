using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oggy.Repository;
using Oggy.Transliterator;

namespace TestLanguageTools.DataBase
{
    [TestClass]
    public class DataBaseTest
    {
        class LanguagePair
        { public string LangId1, LangId2;   }

        private static List<LanguagePair> LanguagePairs = new List<LanguagePair>();

        private static SqlRepository repository = new SqlRepository();

        private static Oggy.Transliterator.Transliterator transliterator = new Oggy.Transliterator.Transliterator(null);

        /// <summary>
        /// Get all pairs of source and destination language for transliteration
        /// </summary>
        private static void LoadLanguagePairs()
        {
            var languages = repository.ListLanguages();
            foreach (var lang1 in languages)
                foreach (var lang2 in languages)
                {
                    repository.SetSourceLanguage(lang1.Code);
                    repository.SetDstLanguage(lang2.Code);
                    if (repository.CountRules() > 0)
                        LanguagePairs.Add(new LanguagePair() { LangId1 = lang1.Code, LangId2 = lang2.Code });
                }
        }

        /// <summary>
        /// Goes through pronunciation examples and test them.
        /// </summary>
        /// <param name="examples">Dictionary of examples. Every example is a pair (word, pronunciation).</param>
        /// <remarks>
        /// Assumes pronunciation dictionary in target.phons is loaded.
        /// </remarks>
        /// <exception cref="Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// When pronunciation acquired from <code>targer.transliterate</code> is not equal to one provided in the example.
        /// </exception>
        private void TestPronunciation(Dictionary<string, string> examples, string srcLang="", string dstLang="")
        {
            foreach (var example in examples)
            {
                string actual = transliterator.TransliterateFinal(example.Key);

                Assert.AreEqual(example.Value, actual,
                    string.Format("{0}->{1} expected {2}, but actual {3}", srcLang, dstLang, example.Value, actual));
            }
        }

        /// <summary>
        /// Tests all examples for given language pair.
        /// </summary>
        /// <param name="LangId1">Source language</param>
        /// <param name="LangId2">Destination language</param>
        /// <exception cref="Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// When the test fails.
        /// </exception>
        private void TestLanguagePair(string LangId1, string LangId2)
        {
            // Set the src and dst languages
            repository.SetSourceLanguage(LangId1);
            repository.SetDstLanguage(LangId2);
            
            // ****** Read jokers, transliteration rules and transliteration examples
            transliterator.jokers = repository.GetJokers();

            transliterator.rules = new Dictionary<string, string>();
            var rules = repository.ListRules();
            foreach (var rule in rules)
                transliterator.rules.Add(rule.Source, rule.Destination);

            var examples = repository
                .ListExamples(true)
                .ToDictionary(example => example.Source, example => example.Destination);

            // Test it as it is
            TestPronunciation(examples, LangId1, LangId2);
            
            #region ****** Removing rules **************************
            //Copy the keys to a buffer array
            var buffer = new string[transliterator.rules.Count];
            transliterator.rules.Keys.CopyTo(buffer, 0);

            foreach (var key in buffer)
            {
                // Store (key, value) pair and remove it
                string value = transliterator.rules[key];
                transliterator.rules.Remove(key);

                bool test_succeeded = false;
                try
                {
                    TestPronunciation(examples);
                }
                catch (AssertFailedException)
                {   // Test succeded!
                    test_succeeded = true;
                }

                //Re-add the (key, value) pair
                transliterator.rules.Add(key, value);

                //Test fails if test_succeded has not been set to true
                Assert.IsTrue(test_succeeded, "(" + LangId1 + "," + LangId2 + ") The rule " + key + "->" + value + " is needless." +
                    "Either: 1) remove this rule 2) set disable to true or 3) add an example where this rule is applied");

                //if (key.First() == '|')
                if (true)
                {
                    string newKey = key.Substring(1);
                    if (!transliterator.rules.ContainsKey(newKey))
                    {
                        transliterator[newKey] = value;
                        transliterator[key] = null;

                        test_succeeded = false;
                        try
                        {
                            TestPronunciation(examples);
                        }
                        catch (AssertFailedException)
                        {   // Test succeded!
                            test_succeeded = true;
                        }

                        transliterator[key] = value;

                        //Test fails if test_succeded has not been set to true
                        Assert.IsTrue(test_succeeded, "(" + LangId1 + "," + LangId2 + ") The rule " + key + "->" + value + " can be shortened as " + newKey);
                    }
                }

                //if (key.Last() == '|')
                if (true)
                {
                    string newKey = key.Substring(0, key.Length - 1);
                    if (!transliterator.rules.ContainsKey(newKey))
                    {
                        transliterator[newKey] = value;
                        transliterator[key] = null;

                        test_succeeded = false;
                        try
                        {
                            TestPronunciation(examples);
                        }
                        catch (AssertFailedException)
                        {   // Test succeded!
                            test_succeeded = true;
                        }

                        transliterator[key] = value;

                        //Test fails if test_succeded has not been set to true
                        Assert.IsTrue(test_succeeded, "(" + LangId1 + "," + LangId2 + ") The rule " + key + "->" + value + " can be shortened as " + newKey);
                    }
                }
            }
            #endregion

            //TODO: try with removing the break (i.e. |) from the rules that have it, and see if still all the examples pass
            //if they all pass, the rule should be wihtout the break
        }

        [TestMethod(), Ignore()]
        public void TestAllLanguagePairs()
        {
            LoadLanguagePairs();

            //assumes LoadLanguagePairs() has been called
            foreach (var pair in LanguagePairs)
            {
                TestLanguagePair(pair.LangId1, pair.LangId2);
            }
        }
    }
}
