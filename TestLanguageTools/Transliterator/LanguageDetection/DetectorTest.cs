using Oggy.LanguageDetection;
using Oggy.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLanguageTools
{
    [TestClass()]
    public class DetectorTest
    {
        /// <summary>
        ///A test for detect
        ///</summary>
        [TestMethod()]
        public void detectTest()
        {
            SqlRepository repository = new SqlRepository();

            var languages = repository.ListLanguages(true);

            foreach (var lang in languages)
            {
                // Check Sample Texts
                var sampleTexts = repository.ListTextSamples(lang.Code);
                foreach (var text in sampleTexts)
                {
                    Assert.AreEqual(lang.Code, Detector.detect(text.Text));
                }
            }
        }
    }
}
