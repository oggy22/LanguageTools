using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy
{
    /// <summary>
    /// This is a supporting class for new rules and new algorithm.
    /// The rules are of the form
    /// pretext + text + posttext
    /// pretext and posttext are optional (so they compatible with old rules)
    /// and they can optinally start and/or end the word
    /// 
    /// To compare two rules
    /// - compare text len
    /// - compare posttext len
    /// - compare pretext len
    /// </summary>
    class Parser
    {
        #region Variables
        /// <summary>
        /// All the letters. To be used in IsLetter
        /// </summary>
        static private string letters;
        
        private string text, lowerCaseText;

        private int position, lastWordStartPosition, lastTransliteratedWordStartPosition;

        private bool inTheWord;

        private StringBuilder output;

        private Dictionary<char, string> jokers;
        #endregion

        #region Constructors

        public Parser(string text)
        {
            this.text = text;
            this.lowerCaseText = text.ToLower();
            position = 0; lastWordStartPosition = -1; lastTransliteratedWordStartPosition = -1;
            inTheWord = false;
            output = new StringBuilder(256);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the char is a letter
        /// </summary>
        /// <param name="c">Char to be determined if it is a letter</param>
        /// <returns>True if it is</returns>
        private static bool IsLetter(char c)
        {
            //TODO: improve it with binary search, and letters should be sorted on init
            return letters.IndexOf(c) >= 0;
        }

        private bool IsCurrentCharLetter()
        {
            return IsLetter(lowerCaseText[position]);
        }

        public static char BREAK = '|';

        public bool CanApplyRule(string ruleKey)
        {
            string key = ruleKey;
            bool endWord = false;
            if (key.First() == BREAK)
            {
                key = ruleKey.Substring(1);

                // Assert inTheWord=false && IsCurrentCharLetter()=true
                if (inTheWord || !IsCurrentCharLetter())
                    return false;
            }
            if (key.Last() == BREAK)
            {
                key = ruleKey.Substring(0, ruleKey.Length - 1);
                endWord = true;
            }

            // Check if the text is long enough for the key
            if (position + key.Length > text.Length)
                return false;

            if (endWord)
            {
                if (position + key.Length < text.Length && IsLetter(lowerCaseText[position + key.Length]))
                    return false;
            }

            // Check the rule against the text
            for (int i = position, j = 0; i < text.Length && j < key.Length; i++, j++)
            {
                if (jokers.ContainsKey(key[j]))
                {
                    if (!jokers[key[j]].Contains(lowerCaseText[i]))
                        return false;
                }
                else
                {
                    if (lowerCaseText[i] != key[j])
                        return false;
                }
            }

            return true;
        }

        public void ApplyRule(string ruleKey, string ruleValue)
        {
            bool startWord, endWord;
            if (ruleKey.First() != BREAK)
            {
                ruleKey = ruleKey.Substring(1);
                startWord = true;
                lastWordStartPosition = position;
                lastTransliteratedWordStartPosition = output.Length;
            }
            if (ruleKey.Last() != BREAK)
            {
                ruleKey = ruleKey.Substring(0, ruleKey.Length - 1);
                endWord = true;
            }
        }

        #endregion
    }
}
