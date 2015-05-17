using Moq;
using Oggy.Repository;
using Oggy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLanguageTools.Transliterator
{
	static class MockRepository
	{
		public static ARepository Create()
		{
			var mock = new Mock<ARepository>();
			mock.Setup(x => x.RetreiveLanguage("EN")).Returns(new Language() { Code = "EN", Name = "English" });
			mock.Setup(x => x.RetreiveLanguage("EN")).Returns(new Language() { Code = "SR", Name = "Serbian" });
			mock.Setup(x => x.GetJokers()).Returns(new Dictionary<char, string> { { '#', "qwrtpsdfghjklzxcvbnm" } });
			List<TransliterationRule> rules = new List<TransliterationRule>();
			rules.Add(new TransliterationRule("ai", "ej"));
			rules.Add(new TransliterationRule("all", "ol"));
			rules.Add(new TransliterationRule("a", "e"));
			rules.Add(new TransliterationRule("i", "aj"));
			rules.Add(new TransliterationRule("c", "s"));
			rules.Add(new TransliterationRule("a#e", "ej#"));
			rules.Add(new TransliterationRule("i#e", "aj#"));

			//TODO: add this one when multiple occurance of the same joker is supported
			//rules.Add(new TransliterationRule("#i#e", "#aj#"));    // two jokers of the same type

			mock.Setup(x => x.ListRules(true)).Returns(rules);
			return mock.Object;
		}
	}
}
