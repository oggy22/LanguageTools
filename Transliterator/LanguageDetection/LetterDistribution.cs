using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository;

namespace Oggy.LanguageDetection
{
    /// <summary>
    /// Letter distribution of a text or a language.
    /// Letter distribution contains frequency (or count) for each letter.
    /// </summary>
    class LetterDistribution
    {
        #region Variables

        private Dictionary<char, double> letters;

        #endregion

        const string LETTERS_TO_OMIT = " ,.;'[]()";

        #region Constructors

        /// <summary>
        /// Creates a letter distibution based on a lanugage from the repository
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="languageCode">The language code of the language</param>
        public LetterDistribution(ARepository repository, string languageCode)
        {
            repository.RetreiveLanguage(languageCode);
        }

        /// <summary>
        /// Creates a letter distribution based on the given text.
        /// </summary>
        /// <param name="text">Text containing letters to create LetterDistibution</param>
        public LetterDistribution(string text)
        {
            letters = new Dictionary<char, double>();
            int totalCount = 0;

            foreach (char c in text)
            {
                if (LETTERS_TO_OMIT.IndexOf(c) >= 0)
                    continue;

                char lowerCaseLetter = char.ToLower(c);

                if (!letters.ContainsKey(lowerCaseLetter))
                    letters[lowerCaseLetter] = 1;
                else
                    letters[lowerCaseLetter]++;

                totalCount++;
            }

            foreach (var key in letters.Keys.ToList())
            {
                letters[key] = letters[key] / totalCount;
            }
        }

        #endregion

        /// <summary>
        /// Calculates the distance between the two language distribution,
        /// given that <code>text</code> is the LetterDistribution based on text.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double Distance(LetterDistribution language, LetterDistribution text)
        {
            double distance2 = 0;

            foreach (char key in text.letters.Keys)
            {
                double textFrequency = (double)(text.letters[key]);
                double languageFrequency = (double)(language.letters[key]);
                double sub = textFrequency - languageFrequency;
                distance2 += sub*sub;
            }

            return distance2;
        }
    }
}