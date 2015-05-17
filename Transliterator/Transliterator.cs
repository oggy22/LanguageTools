using Oggy.Repository;
using Oggy.Repository.Entities;
using Oggy.Transliterator.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator
{
	public class Transliterator : TransliteratorBase
	{
		private RuleCollection rules;

		internal Dictionary<char, string> jokers;

		public List<TransliterationExample> ExamplesToUpdate { get; private set; }

		public override string Transliterate(string text)
		{
			SourceTextWithIterator source = new SourceTextWithIterator(text);
			string destination = string.Empty;
			while (source.LeftCharacters > 0)
			{
				string add;
				Rule rule = rules.FindBestRule(source);
				if (rule.RawSource == string.Empty)
				{
					add = source.CurrentCharacter.ToString();
					source.IncrementPosition();
				}
				else
				{
					add = rule.Apply(source);
				}
				destination += add;
			}
			return destination;
		}

		public string this[string s]
		{
			get
			{
				return Transliterate(s);
			}
		}

		public Transliterator(ARepository repository, string sourceLang, string destLang)
		{
			rules = new RuleCollection(repository, sourceLang, destLang);
			jokers = repository.GetJokers();
		}

		public Transliterator()
		{
		}
	}
}