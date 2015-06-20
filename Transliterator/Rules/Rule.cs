using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator.Rules
{
    public class Rule
    {

#region Fields
        private int[] jokersLookUp;

        public string Source
		{
			get;
			private set;
		}

		public string Destination
		{
			get;
			private set;
		}

        Dictionary<char, string> jokers;

        RuleCollection ruleCollection;
		HashSet<string> counterExamples;

#endregion

		  #region Constructors
		  /// <summary>
        /// Creates a Rule
        /// </summary>
        /// <param name="source">Source of of the rule.</param>
        /// <param name="destination">Destination of the rule.</param>
        /// <param name="jokers"></param>
        /// <param name="ruleCollection">Containing RuleCollection.</param>
        public Rule(string source, string destination, Dictionary<char, string> jokers, RuleCollection ruleCollection, IEnumerable<string> counterExamples=null)
        {
            this.ruleCollection = ruleCollection;
            this.RawSource = source;
            this.Destination = destination;
            this.jokers = jokers;
				this.counterExamples = counterExamples != null ? new HashSet<string>(counterExamples) : null;

			if (source != string.Empty)
			{
				if (source.First() == '|')
				{
					WordBegins = true;
					source = source.Substring(1);
				}

				if (source.Last() == '|')
				{
					WordEnds = true;
					source = source.Substring(0, source.Length - 1);
				}
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

            this.Source = source;
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

			public int Length
			{
				get
				{
					return Source.Length;
				}
			}
#endregion

#region Methods
		static public bool IsLetter(char c)
		{
			return char.IsLetter(c);
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

            for (int i = 0; i < Source.Length; i++)
            {
                char c = Source[i];
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
            if (text.LeftCharacters < this.Source.Length)
                return false;
            
            if (WordBegins)
            {
                if (text.InWord || !char.IsLetter(text.CurrentCharacter))
                    return false;
            }

            for (int i = 0, texti = text.Position; i < this.Source.Length; i++, texti++)
            {
                if (jokersLookUp[i] == -1)
                {
                    if (this.Source[i] != char.ToLower(text[texti]))
                        return false;
                }
                else // So it is a joker
                {
                    if (jokers[Source[i]].IndexOf(char.ToLower(text[texti])) == -1)
                        return false;
                }
            }

			if (WordEnds && Length!=text.LeftCharacters)
			{
				if (IsLetter(text[text.Position + Length]))
					return false;
			}

			  if (this.counterExamples != null)
			  {
				  if (this.counterExamples.Contains(text.CurrentWord))
					  return false;
			  }

            return true;
        }

        public string Apply(SourceTextWithIterator text)
        {
            StringBuilder ret = new StringBuilder(this.Destination);

            for (int i = 0, texti = text.Position; i < this.Source.Length; i++, texti++)
            {
                if (jokersLookUp[i] != -1)
                {
					string C = text[texti].ToString();
					string c = C.ToLower();

                    if (ruleCollection.Contains(c))
                        ret[jokersLookUp[i]] = ruleCollection[c].Destination[0];
                    else
                        ret[jokersLookUp[i]] = c[0];

					if (c[0] != C[0])
						ret[jokersLookUp[i]] = char.ToUpper(ret[jokersLookUp[i]]);
                }
            }

			switch (text.IncrementPosition(this.Source.Length))
			{
				case WordCapitals.UpperCase:
					return ret.ToString().ToUpper();
				case WordCapitals.CapitalLetter:
					{
						string rets = ret.ToString();
						if (rets != string.Empty)
							rets = char.ToUpper(rets[0]) + rets.Substring(1);
						return rets;
					}
			}

			return ret.ToString();
		}

        /// <summary>
        /// Compares rules.
        /// </summary>
        /// <param name="rule1">Left rule</param>
        /// <param name="rule2">Right rule</param>
        /// <returns>True if the left rule is greater (i.e more appropriate) than the right rule</returns>
        static public bool operator > (Rule rule1, Rule rule2)
        {
            if (rule1.Source.Length > rule2.Source.Length)
                return true;

            if (rule1.Source.Length < rule2.Source.Length)
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