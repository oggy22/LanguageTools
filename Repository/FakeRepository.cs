using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository;
using Oggy.Repository.Entities;

namespace Oggy.Repository
{
    public class FakeRepository : ARepository
    {
        static private List<Language> languages;

        static FakeRepository()
        {
            languages = new List<Language>();
            languages.Add(new Language() { Code = "DE", Name = "Deutsch" });
            languages.Add(new Language() { Code = "EN", Name = "English" });
            languages.Add(new Language() { Code = "ES", Name = "Español" });
            languages.Add(new Language() { Code = "FA", Name = "Farsi" });
            languages.Add(new Language() { Code = "FR", Name = "Français" });
            languages.Add(new Language() { Code = "HR", Name = "Hrvatski/Srpski latinica" });
            languages.Add(new Language() { Code = "HU", Name = "Magyar" });
            languages.Add(new Language() { Code = "IT", Name = "Italiano" });
            languages.Add(new Language() { Code = "MK", Name = "Македонски" });
            languages.Add(new Language() { Code = "PL", Name = "Polski" });
            languages.Add(new Language() { Code = "RU", Name = "Русский" });
            languages.Add(new Language() { Code = "SR", Name = "Српски" });
        }
        
        public override IEnumerable<Oggy.Repository.Entities.Language> ListLanguages(bool enabledOnly = false)
        {
            return languages.AsEnumerable();
        }

        public override void CreateLanguage(Language language)
        {
            throw new NotImplementedException();
        }

        public override Oggy.Repository.Entities.Language RetreiveLanguage(string code)
        {
            return languages.Find(language => language.Code == code);
        }

        public override void UpdateLanguage(Language code)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteLanguage(string code)
        {
            throw new NotImplementedException();
        }

        public override List<TransliterationRule> ListRules(bool enabledOnly = true)
        {
            return new List<TransliterationRule>();
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

        public override Dictionary<char, string> GetJokers()
        {
            return new Dictionary<char, string>();
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

        #region TextSamples
        public override List<TextSample> ListTextSamples(string langCode)
        {
            switch (langCode)
            {
                case "EN":
                    return new List<TextSample>()
                    {
                        new TextSample()
                        {
                            Title = "To be or not to be",
                            Text = "To be, or not to be\r\n, To be, or not to be\r\n, To be, or not to be\r\n, To be, or not to be\r\n, To be, or not to be\r\n, To be, or not to be\r\n, that is the question:\r\nWhether 'tis \r\nNobler in the mind to suffer \r\nThe Slings and Arrows of outrageous Fortune\r\n,"
                        },
                        new TextSample()
                        {
                            Title = "The quick brown fox",
                            Text = "The quick brown fox jumps over the lazy dog."
                        }
                    };
                case "SR":
                    return new List<TextSample>()
                    {
                        new TextSample()
                        {
                            Title = "Иво Андрић",
                            Text = "Ток догађаја у животу не зависи од нас, никако или врло мало, али начин на које ћемо те догађаје поднети, у доброј мери зависи од нас."
                        },
                    };
                default:
                    return new List<TextSample>();
            }
        }
        #endregion

        public override Dictionary<char, double> LoadLetterDistribution(string langCode)
        {
            switch (langCode)
            {
                case "EN":
                    return new Dictionary<char, double>()
                    {
                        {'a',   8.167},
                        {'b',	1.492},
                        {'c',	2.782},
                        {'d',	4.253},
                        {'e',	12.702},
                        {'f',	2.228},
                        {'g',	2.015},
                        {'h',	6.094},
                        {'i',	6.966},
                        {'j',	0.153},
                        {'k',	0.772},
                        {'l',	4.025},
                        {'m',	2.406},
                        {'n',	6.749},
                        {'o',	7.507},
                        {'p',	1.929},
                        {'q',	0.095},
                        {'r',	5.987},
                        {'s',	6.327},
                        {'t',	9.056},
                        {'u',	2.758},
                        {'v',	0.978},
                        {'w',	2.36},
                        {'x',	0.15},
                        {'y',	1.974},
                        {'z',	0.074}
                    };
                case "SR":
                    return new Dictionary<char, double>()
                    {
                        {'a', 15.0},
                        {'b', 5.0},
                        {'c', 7.0},
                    };
                default:
                    return new Dictionary<char, double>();
            }
        }

        public override IEnumerable<Word> ListWords()
        {
            throw new NotImplementedException();
        }

        public override void CreateWord(Word word)
        {
            throw new NotImplementedException();
        }

        public override void DeleteWord(string word)
        {
            throw new NotImplementedException();
        }

        public override void UpdateWord(Word word)
        {
            throw new NotImplementedException();
        }
    }
}