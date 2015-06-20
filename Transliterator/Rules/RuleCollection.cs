using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Oggy.Repository;

namespace Oggy.Transliterator.Rules
{
	public class RuleCollection : KeyedCollection<string, Rule>
	{
		public RuleCollection()
		{
			// just for testing purposes
		}
		public RuleCollection(ARepository repository, string sourceLang, string destLang)
		{
			repository.SetSourceLanguage(sourceLang);
			repository.SetDstLanguage(destLang);

			var jokers = repository.GetJokers();

			repository.ListRules()
				.Where(rule => !rule.Disabled)
				.ToList()
				.ForEach(rule =>
					this.AddRule(
					rule.Source,
					rule.Destination,
					jokers,
					(rule.CounterExamples ?? string.Empty).Split(new[]{ ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()));
		}

		protected override string GetKeyForItem(Rule rule)
		{
			return rule.RawSource;
		}

		/// <summary>
		/// Finds the best applicable rule for the given <c>SourceTextWithIterator</c> in the collection.
		/// </summary>
		/// <param name="source"></param>
		/// <returns>The best applicable rule if any, otherwise an empty rule (i.e. source == empty string)</returns>
		public Rule FindBestRule(SourceTextWithIterator source)
		{
			Rule best = new Rule("", "", null, null);
			foreach (Rule rule in this.Items)
			{
				if (rule.CanApply(source) && (rule > best))
					best = rule;
			}

			return best;
		}

		public void AddRule(string source, string destination, Dictionary<char, string> jokers, List<string> counterExamples)
		{
			this.Add(new Rule(source, destination, jokers, this, counterExamples));
		}
	}
}