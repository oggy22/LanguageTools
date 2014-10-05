using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.LanguageDetection
{
    static class Detector
    {
        static Dictionary<string, LetterDistribution> languages;

        /// <summary>
        /// Detects the source language based on the letter frequency.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>The language code (e.g. "EN" for English), or null if no language was detected</returns>
        public static string detect(string text)
        {
            LetterDistribution textDistribution = new LetterDistribution(text);
            
            double minDistance = double.MaxValue;
            KeyValuePair<string, LetterDistribution> pair = new KeyValuePair<string, LetterDistribution>(null, null);
            
            foreach (var lang in languages)
            {
                double distance = LetterDistribution.Distance(lang.Value, textDistribution);

                if (distance < minDistance)
                {
                    pair = lang;
                    minDistance = distance;
                }
            }
            
            return pair.Key;
        }

        static void LoadLanguages()
        {
        }

        static Repository.ARepository repository;

        static Detector()
        {
            repository = new Repository.SqlRepository();
            //languages = repository.ListLanguages();
        }
    }
}
