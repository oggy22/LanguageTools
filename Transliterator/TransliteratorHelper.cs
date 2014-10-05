using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator
{
    public static class TransliteratorHelper
    {
        internal const char BREAK = '|';

        public const string DELIMETERS = " \t,.!?\r\n\"";
        
        /// <summary>
        /// Add Break characters in the front and after each word.
        /// Word is defined as sequence between two non-alphabetic characters
        /// </summary>
        /// <param name="word">The word to be changed</param>
        /// <returns></returns>
        public static string AddBreaks(string word)
        {
            string ret = "";
            bool InTheWord = false;
            foreach (char c in word)
            {
                if (char.IsLetter(c) != InTheWord)
                {
                    InTheWord = !InTheWord;
                    ret += BREAK;
                }
                ret += c;
            }
            if (ret != "" && char.IsLetter(ret, ret.Length - 1))
                ret += BREAK;
            return ret;
        }

        /// <summary>
        /// Remove breaks out of the word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string RemoveBreaks(string word)
        {
            string ret = "";
            foreach (char c in word)
                if (c != BREAK)
                    ret += c;
            return ret;
        }

        public static WordCapitals Determine(string word)
        {
            if (word == word.ToLower())
                return WordCapitals.LowerCase;
            if (char.IsUpper(word[0]) && Determine(word.Substring(1)) == WordCapitals.LowerCase)
                return WordCapitals.CapitalLetter;
            if (word == word.ToUpper())
                return WordCapitals.UpperCase;
            return WordCapitals.None;
        }
    }
}