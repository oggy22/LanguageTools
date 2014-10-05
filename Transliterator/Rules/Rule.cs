﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator.Rules
{
    class Rule
    {

#region Fields
        private int[] jokersLookUp;

        private string source, destination;

        Dictionary<char, string> jokers;

        RuleCollection ruleCollection;
#endregion
        
#region Constructors
        /// <summary>
        /// Creates a Rule
        /// </summary>
        /// <param name="source">Source of of the rule.</param>
        /// <param name="destination">Destination of the rule.</param>
        /// <param name="jokers"></param>
        /// <param name="ruleCollection">Containing RuleCollection.</param>
        public Rule(string source, string destination, Dictionary<char, string> jokers, RuleCollection ruleCollection)
        {
            this.ruleCollection = ruleCollection;
            this.RawSource = source;
            this.destination = destination;
            this.jokers = jokers;

            if (source.First() == '|')
            {
                WordBegins = true;
                source = source.Substring(1);
            }

            if (source.Last() == '|')
            {
                WordBegins = true;
                source = source.Substring(0, source.Length - 1);
            }

            this.jokersLookUp = new int[source.Length];
            this.Jokers = 0;
            for (int pos = 0; pos < source.Length; pos++)
            {
                this.jokersLookUp[pos] = -1;
                if (jokers.ContainsKey(source[pos]))
                {
                    int index = destination.IndexOf(source[pos]);
                    if (index == -1)
                        throw new ArgumentException("Unsupported joker in the source", "source");

                    this.jokersLookUp[pos] = index;
                    this.Jokers++;
                }
            }

            this.source = source;
        }
#endregion
        
#region Properties
        public bool WordBegins
        { get; private set; }

        public bool WordEnds
        { get; private set; }

        public string RawSource
        { get; private set; }

        public int Jokers
        { get; private set; }
#endregion

#region Methods
        static public bool IsLetter(char c)
        {
            return 'a' < c && c < 'z';
        }
        
        public bool CanApply(string text, int position)
        {
            if (WordBegins)
            {
                if (!IsLetter(text[position]))
                    return false;

                if (position > 0 && IsLetter(text[position - 1]))
                    return false;
            }

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                if (IsLetter(c))
                {
                    if (c != text[position + i])
                        return false;
                }
                else
                {
                    //todo
                }
            }

            return true;
        }

        public bool CanApply(SourceTextWithIterator text)
        {
            if (text.LeftCharacters < this.source.Length)
                return false;
            
            if (WordBegins)
            {
                if (text.InWord || !char.IsLetter(text.CurrentCharacter))
                    return false;
            }

            for (int i = 0, texti = text.Position; i < this.source.Length; i++, texti++)
            {
                if (jokersLookUp[i] == -1)
                {
                    if (this.source[i] != text[texti])
                        return false;
                }
                else // So it is a joker
                {
                    if (jokers[source[i]].IndexOf(text[texti]) == -1)
                        return false;
                }
            }

            return true;
        }

        public void Apply(string text)
        {

        }

        public string Apply(SourceTextWithIterator text)
        {
            StringBuilder ret = new StringBuilder(this.destination);

            for (int i = 0, texti = text.Position; i < this.source.Length; i++, texti++)
            {
                if (jokersLookUp[i] != -1)
                {
                    string c = text[texti].ToString();

                    if (ruleCollection.Contains(c))
                        ret[jokersLookUp[i]] = ruleCollection[c].destination[0];
                    else
                        ret[jokersLookUp[i]] = c[0];
                }
            }
            //todo: increment text/source
            return ret.ToString();
        }

        /// <summary>
        /// Compares rules.
        /// </summary>
        /// <param name="rule1">Left rule</param>
        /// <param name="rule2">Right rule</param>
        /// <returns>True if the left rule is greater (i.e more appropriate) than the right rule</returns>
        static public bool operator >(Rule rule1, Rule rule2)
        {
            if (rule1.source.Length > rule2.source.Length)
                return true;

            if (rule1.source.Length < rule2.source.Length)
                return false;

            // So rule1.source.Length == rule2.source.Length

            if (rule1.Jokers < rule2.Jokers)
                return true;

            if (rule1.Jokers > rule2.Jokers)
                return false;

            // So rule1.source.Length == rule2.source.Length and rule1.Jokers == rule2.Jokers

            if (rule1.WordBegins && !rule2.WordBegins)
                return true;

            if (!rule1.WordBegins && rule2.WordBegins)
                return false;

            if (rule1.WordEnds && !rule2.WordEnds)
                return true;

            if (!rule1.WordEnds && rule2.WordEnds)
                return false;

            return false;
        }

        /// <summary>
        /// This operator is added to prevent a compiler error
        /// when only one of less and greater operators are implemented.
        /// DO NOT USE THIS OPERATOR. USE &g operator instead.
        /// </summary>
        /// <param name="rule1">Left rule</param>
        /// <param name="rule2">Right rule</param>
        /// <returns>True if the left rule is smaller than the right rule</returns>
        static public bool operator <(Rule rule1, Rule rule2)
        {
            return rule2 < rule1;
        }
#endregion

    }
}