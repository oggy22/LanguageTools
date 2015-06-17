using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository;
using Oggy.Repository.Entities;
using Oggy.Transliterator.Rules;

namespace Oggy.Transliterator
{
    /// <summary>
    /// Several transliterations
    /// 1) Transliterate only
    /// 2) Transliterate with jokers and delimeters (i.e. |)
    /// 3) Transliterate with jokers, delimeters and word capitals
    /// </summary>
    public class TransliteratorOld : TransliteratorBase
    {
        private const char DASH = '-';

        #region Variables
        internal Dictionary<string, string> rules;

        //internal Rules.RuleCollection ruleCollection;

        internal Dictionary<char, string> jokers;

		public List<TransliterationExample> ExamplesToUpdate { get; private set; }
		#endregion

        #region Constructors
		public TransliteratorOld(ARepository repository, string sourceLang, string destLang)
		{
			repository.SetSourceLanguage(sourceLang);
			repository.SetDstLanguage(destLang);
			rules = new Dictionary<string, string>();
			jokers = repository.GetJokers();
			repository.ListRules()
				.Where(rule => !rule.Disabled)
				.ToList()
				.ForEach(rule => rules.Add(rule.Source, rule.Destination));
			ExamplesToUpdate = new List<TransliterationExample>();
		}

        public TransliteratorOld(Dictionary<string, string> rules)
        {
            this.rules = rules;
            this.jokers = new Dictionary<char, string>();
            ExamplesToUpdate = new List<TransliterationExample>();
        }

        #endregion

        #region Methods

		public string Transliterate2(string text)
		{
			SourceTextWithIterator source = new SourceTextWithIterator(text);
			string destination = string.Empty;
			while (source.LeftCharacters > 0)
			{
				string add;
				Rule rule = null;// = ruleCollection.FindBestRule(source);
				if (rule.RawSource == string.Empty)
				{
					add = source.CurrentCharacter.ToString();
					source.IncrementPosition();
				}
				else
				{
					rule.Apply(source);
				}
			}
			return destination;
		}

		public override string Transliterate(string text, bool dashes = false)
        {
            return TransliterateFinal(text, false);
        }

        /// <summary>
        /// Performs transliteration (without delimeters, for a single word)
        /// </summary>
        /// <param name="s">Input string in the Source Language</param>
        /// <returns>Output string in the Destination Language</returns>
        public string this[string s]
        {
            // Set is for adding or changing rules
            set
            {
                if (value != null)
                    rules[s] = value;
                else
                    rules.Remove(s);
                ExamplesToUpdate.ForEach(example => example.TransliterationChanged());
            }
            get
            {
                string input = s;
                string output = "";
                while (input.Length > 0)
                {
                    string found = "";
                    foreach (string src in rules.Keys)
                    {
                        if (src.Length > found.Length && input.StartsWith(src))
                            found = src;
                    }
                    if (found != "")
                        output += rules[found];
                    else
                        output += input[0];

                    // Take from the input string
                    int take = found.Length;
                    if (take == 0)
                        take = 1;
                    input = input.Substring(take);
                }
                return output;
            }
        }

        /// <summary>
        /// Compares rules
        /// </summary>
        /// <param name="newkey"></param>
        /// <param name="oldkey"></param>
        /// <returns></returns>
        internal bool CompareRules(string newkey, string oldkey)
        {
            if (newkey.Length > oldkey.Length)
                return true;

            if (newkey.Length < oldkey.Length)
                return false;

            return CountJokers(newkey) < CountJokers(oldkey);
        }
        
        internal string TransliterateWithJokersAndWordCapitals(string text, bool addDash)
        {
            bool inTheWord = false;
            string lastWord = string.Empty;
            StringBuilder ret = new StringBuilder();
            int lastDestWordStartPos = -1;
            string lowercaseText = text.ToLower();

            while (text.Length > 0)
            {
                // Empty string indicates no rule found so far
                string key = string.Empty;

                if (!char.IsLetter(text, 0))
                {
                    if (inTheWord)
                    {
                        while (lastDestWordStartPos < ret.Length && ret[lastDestWordStartPos] == '|')
                            lastDestWordStartPos++;

                        switch (TransliteratorHelper.Determine(lastWord.Trim('|')))
                        {
                            case WordCapitals.CapitalLetter:
                                if (lastDestWordStartPos < ret.Length)
                                    ret[lastDestWordStartPos] = ret[lastDestWordStartPos].ToString().ToUpper()[0];
                                break;
                            case WordCapitals.UpperCase:
                                for (int pos = lastDestWordStartPos; pos < ret.Length; pos++)
                                    ret[pos] = ret[pos].ToString().ToUpper()[0];
                                break;
                        }
                    }
                    else
                    {
                        lastWord = string.Empty;
                        lastDestWordStartPos = ret.Length;
                    }

                    inTheWord = !inTheWord;
                }

                //Try to find the longest rule with minimum number of jokers
                foreach (var rule in rules)
                {
                    if (Match(rule.Key, lowercaseText) && CompareRules(rule.Key, key))
                        key = rule.Key;
                }

                // If no rule was found, then paste one char from input into output
                if (key == string.Empty)
                {
                    ret.Append(text[0]);
                    if (addDash)
                        ret.Append(DASH);
                    lastWord += text[0];
                    text = text.Substring(1);
                    lowercaseText = lowercaseText.Substring(1);
                }
                else
                {
                    string value = rules[key];

                    string add = string.Empty;
                    
                    //Iterate for each character within the value
                    foreach (char c in value)
                        if (jokers.ContainsKey(c))
                        {
                            // Find position of the joker in the key
                            int pos = key.IndexOf(c);

                            #if DEBUG
                            // If the joker is not found in the key
                            if (pos == -1) continue;
                                //throw new ArgumentException("No joker '" + c + "' in key found!");
                            #endif

                            // Transliterate the character masked by joker and add it into the ret
                            add += this[lowercaseText[pos].ToString()];
                        }
                        else add += c;
                    ret.Append(add);
                    if (addDash && ret[ret.Length-1]!=DASH)
                        ret.Append(DASH);

                    lastWord += text.Substring(0, key.Length);
                    text = text.Substring(key.Length);
                    lowercaseText = lowercaseText.Substring(key.Length);
                }
            }

            if (addDash && ret[ret.Length-1]==DASH)
                ret.Remove(ret.Length - 1, 1);

            return ret.ToString();
        }
        
        public string TransliterateFinal(string word, bool addDash=false)
        {
            if (word == string.Empty)
                return string.Empty;
            
            word = TransliteratorHelper.AddBreaks(word);
            word += " ";
            string trans = TransliterateWithJokersAndWordCapitals(word, addDash);
            trans = trans.Substring(0, trans.Length - 1);
            trans = TransliteratorHelper.RemoveBreaks(trans);
            if (addDash && trans.Last() == DASH)
                trans = trans.Substring(0, trans.Length - 1);
            return trans;
        }

        /// <summary>
        /// Counts the number of jokers inside the word
        /// </summary>
        /// <param name="word">Word to be evaluated</param>
        /// <returns>Number of jokers</returns>
        private int CountJokers(string word)
        {
            int count = 0;
            foreach (char c in word)
                if (jokers.ContainsKey(c))
                    count++;
            return count;
        }

        internal bool IsJoker(char c)
        {
            return jokers.ContainsKey(c);
        }
        
        /// <summary>
        /// Checks if phon matches beggining of the word.
        /// To be used in transliterate_with_brakes
        /// </summary>
        /// <param name="phon">Pattern</param>
        /// <param name="word">Word to be matched</param>
        /// <returns>True if matches</returns>
        internal bool Match(string phon, string word)
        {
            // Pattern lenght must not exceed word lenght
            if (phon.Length > word.Length)
                return false;

            for (int i = 0; i < phon.Length; i++)
            {
                if (IsJoker(phon[i]))
                {
                    if (!jokers[phon[i]].Contains<char>(word[i]))
                        return false;
                    continue;
                }

                if (phon[i] != word[i])
                    return false;
            }
            return true;
        }
        #endregion
    }
}