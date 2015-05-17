using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator.Rules
{
    /// <summary>
    /// A new class for Transliteration Rule
    /// </summary>
    class Rule2
    {
        #region Variables

        private string source, destination;

		  private int preTextLenght, ruleLenght, postTextLenght = 0;

		  private int jokersCount = 0;

		  private int[] jokerPosition = null;

        private bool startWord, endWord;

        #endregion

        #region Properties

        public string PreSource
        { get; private set; }
        
        public string Source
        { get; private set; }

        public string PostSource
        { get; private set; }

        public int this[int position]
        {
            get
            {
                return jokerPosition[position];
            }
        }

        #endregion

        private const char BREAK = '|', RULE_COMA = '+';

        public Rule2(string rule, string destination)
        {
            Parse(rule);
            this.destination = destination;
        }

        private void Parse(string rule)
        {
            if (rule.First() == BREAK)
            {
                rule = rule.Substring(1);
                startWord = true;
            }
            if (rule.Last() == BREAK)
            {
                rule = rule.Substring(0, rule.Length - 1);
                endWord = true;
            }

            int pos1 = rule.IndexOf(RULE_COMA);

            if (pos1 == -1)
            {
                preTextLenght = 0;
                ruleLenght = rule.Length;
                source = rule;
                return;
            }

            int pos2 = rule.IndexOf(RULE_COMA, pos1 + 1);
            if (pos2 == -1)
                throw new ArgumentException("There must be two or none RULE_COMAs");
            if (pos2 == pos2 + 1)
                throw new ArgumentException("The RULE_COMAs must not be next to each other");

            // Calculate pre, main and post
            string pre, main, post;
            main = rule.Substring(pos1 + 1, pos2 - pos1 - 1);
            pre = rule.Substring(0, pos1);
            post = rule.Substring(pos2 + 1);

            // Calculate
            source = pre + main + post;
            preTextLenght = pre.Length;
            ruleLenght = main.Length;
        }

        public bool GreaterThan(Rule2 rule)
        {
            if (this.ruleLenght > rule.ruleLenght)
                return true;
            if (this.ruleLenght < rule.ruleLenght)
                return false;

            if (this.jokersCount > rule.jokersCount)
                return true;
            if (this.jokersCount < rule.jokersCount)
                return false;

            if (this.postTextLenght > rule.postTextLenght)
                return true;
            if (this.postTextLenght < rule.postTextLenght)
                return false;

            if (this.preTextLenght > rule.preTextLenght)
                return true;
            if (this.preTextLenght < rule.preTextLenght)
                return false;

            if (this.endWord && !rule.endWord)
                return true;
            if (!this.endWord && rule.endWord)
                return false;

            if (this.startWord && !rule.startWord)
                return true;
            if (!this.startWord && rule.startWord)
                return false;

            throw new Exception("Internal error: The two rules are equal. Undefined situation");
        }
    }
}