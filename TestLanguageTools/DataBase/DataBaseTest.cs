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

		private static SqlRepository repository;

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			repository = new SqlRepository();
		}

		private static Oggy.Transliterator.Transliterator transliterator;

        /// <summary>
        /// Get all pairs of source and destination language for transliteration
        /// </summary>
        private void LoadLanguagePairs()
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
		private void TestPronunciation(Dictionary<string, string> examples, string srcLang = "", string dstLang = "")
        {
            foreach (var example in examples)
            {
				string actual = transliterator[example.Key];

				Assert.AreEqual(example.Value, actual,
					string.Format("{0}->{1} expected {2}, but actual {3}", srcLang, dstLang, example.Value, actual));
            }
        }

		private void TestPronunciationWithThreshold(string srcLang, string dstLang, int failThreshold, int distanceTreshold)
		{
			// Load transliterator and examples
			repository.SetSourceLanguage(srcLang);
			repository.SetDstLanguage(dstLang);
			transliterator = new Oggy.Transliterator.Transliterator(repository, srcLang, dstLang);
			var examples = repository
				.ListExamples(true)
				.ToDictionary(example => example.Source, example => example.Destination);

			int total = 0, failed = 0, totalDistance = 0;
			foreach (var example in examples)
			{
				total++;
				string actual = transliterator[example.Key];

				if (example.Value != actual)
				{
					failed++;
					totalDistance += Oggy.TransliterationEditor.WordDistance.CalculateDistance(example.Value, actual);
				}
			}
			int failedPercentage = (100 * failed) / total;
			Assert.IsTrue(failedPercentage < failThreshold, "There are " + failedPercentage + "% failed");
			int distancePercentage = (100 * totalDistance) / total;
			Assert.IsTrue(distancePercentage < distanceTreshold, "The distance " + distancePercentage + "% exceeded threshold");
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
			transliterator = new Oggy.Transliterator.Transliterator(repository, LangId1, LangId2);

			var examples = repository
				.ListExamples(true)
				.ToDictionary(example => example.Source, example => example.Destination);

            // Test it as it is
            TestPronunciation(examples, LangId1, LangId2);
            
            #region ****** Removing rules **************************
            //Get all keys
			var keys = transliterator.Rules.Select(item => item.RawSource).ToList();

			foreach (var key in keys)
            {
				var rule = transliterator.Rules[key];
				transliterator.Rules.Remove(key);

                bool test_succeeded = false;
                try
                {
                    TestPronunciation(examples);
                }
                catch (AssertFailedException)
                {   // Test succeded!
                    test_succeeded = true;
                }

                //Re-add the rule
                transliterator.Rules.Add(rule);

                //Test fails if test_succeded has not been set to true
                Assert.IsTrue(test_succeeded, "(" + LangId1 + "," + LangId2 + ") The rule " + key + "->" + rule.Destination + " is needless. " +
					"Do one of the following:\n1) remove this rule\n2) set disable to true or\n3) add an example where this rule is applied");
            }
            #endregion

            //TODO: try with removing the break (i.e. |) from the rules that have it, and see if still all the examples pass
            //if they all pass, the rule should be wihtout the break
        }

		#region To Serbian
		[TestMethod, Ignore]
        public void TestAllLanguagePairs()
        {
            LoadLanguagePairs();

            //assumes LoadLanguagePairs() has been called
            foreach (var pair in LanguagePairs)
            {
				TestLanguagePair(pair.LangId1, pair.LangId2);
			}
        }

		[TestMethod, TestCategory("Long")]
		public void TestCRtoSR()
		{
			TestLanguagePair("HR", "SR");
		}

		[TestMethod, TestCategory("Long")]
		public void TestDEtoSR()
		{
			TestLanguagePair("DE", "SR");
		}

		[TestMethod, TestCategory("Long")]
		public void TestENtoSR()
		{
			TestPronunciationWithThreshold("EN", "SR", 10, 15);
		}

		[TestMethod, TestCategory("Long")]
		public void TestEStoSR()
		{
			TestLanguagePair("ES", "SR");
		}

		[TestMethod, TestCategory("Long")]
		public void TestFRtoSR()
		{
			TestLanguagePair("FR", "SR");
		}

		[TestMethod, TestCategory("Long")]
		public void TestITtoSR()
		{
			TestLanguagePair("IT", "SR");
		}

		[TestMethod, TestCategory("Long")]
		public void TestRUtoSR()
		{
			TestPronunciationWithThreshold("RU", "SR", 6, 9);
		}
		#endregion

		#region From Serbian
		[TestMethod, TestCategory("Long")]
		public void TestSRtoDE()
		{
			TestLanguagePair("SR", "DE");
		}

		[TestMethod, TestCategory("Long")]
		public void TestSRtoEN()
		{
			TestLanguagePair("SR", "EN");
		}

		[TestMethod, TestCategory("Long")]
		public void TestSRtoES()
		{
			TestLanguagePair("SR", "ES");
		}

		[TestMethod, TestCategory("Long")]
		public void TestSRtoRU()
		{
			TestLanguagePair("SR", "RU");
		}
		#endregion
	}
}
