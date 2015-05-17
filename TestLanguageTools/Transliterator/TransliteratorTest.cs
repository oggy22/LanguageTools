using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Oggy.Transliterator;
using Oggy.Repository;
using Oggy.Repository.Entities;

using Moq;
using Oggy;
using Oggy.Transliterator.Rules;

namespace TestLanguageTools.Transliterator
{
	[TestClass]
	public class TransliteratorTest
	{
		#region Tests
		/// <summary>
		///A test for Transliterator Constructor
		///</summary>
		[TestMethod()]
		public void TransliteratorConstructorTest()
		{
			var target = new Oggy.Transliterator.Transliterator(MockRepository.Create(), "EN", "SR");

			Assert.AreEqual("mejn", target["main"]);
			Assert.AreEqual("ajd", target["id"]);
			Assert.AreEqual("Ej", target["Ai"]);
			Assert.AreEqual("EJ", target["AI"]);

			Assert.AreEqual("hrom", target["hrom"]);
			Assert.AreEqual("mejk", target["make"]);
			Assert.AreEqual("najs", target["nice"]);
			
			// Separation mark "|"
			Assert.AreEqual("", target[""]);
			Assert.AreEqual("|", target["|"]);
			Assert.AreEqual("||", target["||"]);

			// Quotation marks
			Assert.AreEqual("\"Mejn\"", target["\"Main\""]);

			// Capital letters            
			Assert.AreEqual("Najs", target["Nice"]);
			Assert.AreEqual("NAJS", target["NICE"]);

			// I
			Assert.AreEqual("Aj", target["I"]);

			Assert.AreEqual("Ol h", target["All h"]);
		}

		[TestMethod()]
		public void TransliteratorMultipleWordsTest()
		{
			var target = new Oggy.Transliterator.Transliterator(MockRepository.Create(), "EN", "SR");
			Assert.AreEqual("mejn Hrom", target["main Hrom"]);
		}

		[TestMethod()]
		public void CanApplyTest()
		{
			var target = new Oggy.Transliterator.Transliterator(MockRepository.Create(), "EN", "SR");
			SourceTextWithIterator text = new SourceTextWithIterator("still");
			Rule rule;

			// Can Apply
			rule = new Rule("|s", "", new Dictionary<char, string>(), null);
			Assert.IsTrue(rule.CanApply(text));
			rule = new Rule("s", "", new Dictionary<char, string>(), null);
			Assert.IsTrue(rule.CanApply(text));
			rule = new Rule("st", "", new Dictionary<char, string>(), null);
			Assert.IsTrue(rule.CanApply(text));

			// Can not Apply
			rule = new Rule("s|", "", new Dictionary<char, string>(), null);
			Assert.IsFalse(rule.CanApply(text));
			rule = new Rule("s@", "", new Dictionary<char, string>(), null);
			Assert.IsFalse(rule.CanApply(text));
			rule = new Rule("stil|", "", new Dictionary<char, string>(), null);
			Assert.IsFalse(rule.CanApply(text));


		}
		#endregion
	}
}