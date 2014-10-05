using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Oggy.Transliterator.Rules
{
    class RuleCollection : KeyedCollection<string, Rule>
    {
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

        public void AddRule(string source, string destination)
        {
            //this.Add(new Rule(source, destination, null, destination));
        }
    }
}