using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Oggy.Repository.Entities;

namespace Oggy.Repository
{
	public class XMLRepository : ARepository
	{
		XElement root, currentLanguageElement;
		XDocument document;
		string filename;

		public XMLRepository()
			: this("Dictionary.xml")
		{
		}

		public XMLRepository(string filename)
		{
			this.filename = filename;

			if (!File.Exists(filename))
			{
				XDocument xDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("Languages"));
				xDocument.Save(filename);
			}

			document = XDocument.Load(filename);
			root = document.Element(XName.Get("Languages"));
		}

		#region Languages
		public override IEnumerable<Language> ListLanguages(bool enabledOnly = false)
		{
			foreach (var node in root.Elements())
			{
				yield return new Language()
				{
					Code = node.Attribute(XName.Get("id")).Value,
					Name = node.Attribute(XName.Get("name")).Value//,
					//Alphabet = node.Attribute(XName.Get("alphabet")).Value
				};
			}
		}

		public override bool CreateLanguage(Language language)
		{
			if (existsLanguage(language.Code))
				return false;

			XElement element = new XElement(XName.Get("Language"));

			// Create word types string separeted by commas
			string wordTypes = string.Empty;
			foreach (var wt in language.WordTypes)
				wordTypes += wt + ",";
			if (wordTypes != string.Empty)
				wordTypes = wordTypes.Substring(0, wordTypes.Length - 1);

			element.Add(
				 new XAttribute(XName.Get("id"), language.Code),
				 new XAttribute(XName.Get("name"), language.Name),
				 new XAttribute(XName.Get("alphabet"), language.Alphabet ?? ""),
			new XAttribute(XName.Get("wordtypes"), wordTypes)
				 );

			root.Add(element);
			root.Save(filename);
			return true;
		}

		private XElement getLanguage(string code)
		{
			return root.Elements().First(node => node.Attribute(XName.Get("id")).Value == code);
		}

		private bool existsLanguage(string code)
		{
			return root.Elements().Any(node => node.Attribute(XName.Get("id")).Value == code);
		}

		public override Language RetreiveLanguage(string code)
		{
			XElement element = getLanguage(code);
			XAttribute xalphabet = element.Attribute("alphabet");

			Language language = new Language()
			{
				Code = element.Attribute("id").Value,
				Name = element.Attribute("name").Value,
				Alphabet = xalphabet != null ? xalphabet.Value : null
			};

			XAttribute xWordTypes = element.Attribute("wordtypes");
			if (xWordTypes != null)
				language.WordTypes = new HashSet<string>(xWordTypes.Value.Split(','));

			return language;
		}

		public override void UpdateLanguage(Language language)
		{
			XElement element = getLanguage(language.Code);
			element.Attribute(XName.Get("name")).Value = language.Name;
			element.SetAttributeValue(XName.Get("alphabet"), language.Alphabet);
			element.SetAttributeValue("wordtypes",
				language.WordTypes.Count > 0 ?
				language.WordTypes.Aggregate((s1, s2) => s1 + "," + s2) :
				string.Empty);
			this.root.Save(filename);
		}

		public override bool DeleteLanguage(string code)
		{
			XElement element = getLanguage(code);
			if (element == null)
				return false;

			element.Remove();
			return true;
		}

		public override void SetSourceLanguage(string code)
		{
			base.SetSourceLanguage(code);
			currentLanguageElement = getLanguage(code);
		}
		#endregion Languages

		#region Word
		public override IEnumerable<Word> ListWords()
		{
			foreach (var word in currentLanguageElement.Elements())
			{
				if (word.Name == "word")
					yield return extractWord(word);
				else if (word.Name == "category")
				{
					foreach (var word2 in word.Elements())
					{
						yield return extractWord(word2);
					}
				}

			}
		}

		private Word extractWord(XElement element)
		{
			XAttribute xattr;

			int status;
			if ((xattr = element.Attribute("status")) == null)
			{
				if (element.Parent.Name == "category")
					xattr = element.Parent.Attribute("id");
			}

			status = (xattr != null ? int.Parse(xattr.Value) : 0);

			string type = string.Empty;
			if ((xattr = element.Attribute("type")) != null)
				type = xattr.Value;

			return new Word()
			{
				Name = element.Attribute("name").Value,
				status = status,
				Translation = element.Attribute("translation").Value,
				Type = type ?? null
			};
		}

		public override void CreateWord(Word word)
		{
			XElement element = new XElement("word",
				 new XAttribute("name", word.Name),
				 new XAttribute("translation", word.Translation),
				 new XAttribute("status", word.status));

			if (word.Type != null)
				element.Add(new XAttribute("type", word.Type));

			currentLanguageElement.Add(element);
			root.Save(filename);
		}

		public override void UpdateWord(Word word)
		{
			XElement element = currentLanguageElement.Elements().First(w => w.Attribute("name").Value == word.Name);
			element.Attribute("translation").Value = word.Translation;
			element.Attribute("status").Value = word.status.ToString();
			if (word.Type != string.Empty)
			{
				XAttribute xattr;
				if ((xattr = element.Attribute("type")) == null)
					element.Add(new XAttribute("type", word.Type));
				else
					xattr.Value = word.Type;
			}
			if (element.Attribute("modified") == null)
				element.Add(new XAttribute("modified", ""));
			element.Attribute("modified").Value = word.Modified.ToString();
			root.Save(filename);
		}

		public override void DeleteWord(string word)
		{
			XElement element = currentLanguageElement.Elements().First(w => w.Attribute("name").Value == word);
			element.Remove();
			root.Save(filename);
		}

		#endregion

		#region Rules
		public override List<TransliterationRule> ListRules(bool enabledOnly = true)
		{
			throw new NotImplementedException();
		}

		public override void CreateRule(TransliterationRule rule)
		{
			throw new NotImplementedException();
		}

		public override void UpdateRule(TransliterationRule rule, string source)
		{
			throw new NotImplementedException();
		}

		public override void DeleteRule(string source)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Examples
		public override List<TransliterationExample> ListExamples(bool enabledOnly = true)
		{
			throw new NotImplementedException();
		}

		public override void CreateExample(TransliterationExample example)
		{
			throw new NotImplementedException();
		}

		public override void UpdateExample(TransliterationExample example, string word)
		{
			throw new NotImplementedException();
		}

		public override void DeleteExample(string word)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Jokers
		public override Dictionary<char, string> GetJokers()
		{
			throw new NotImplementedException();
		}

		public override void AddJoker(char joker, string substitution)
		{
			throw new NotImplementedException();
		}

		public override void UpdateJoker(char joker, string substitution)
		{
			throw new NotImplementedException();
		}

		public override void DeleteJoker(char joker)
		{
			throw new NotImplementedException();
		}
		#endregion

		public override List<TextSample> ListTextSamples(string langCode)
		{
			throw new NotImplementedException();
		}

		public override Dictionary<char, double> LoadLetterDistribution(string langCode)
		{
			throw new NotImplementedException();
		}
	}
}
