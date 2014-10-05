using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository.Entities;
using Oggy.Repository;

namespace Oggy.Transliterator
{
    /// <summary>
    /// This class holds multiple concrete transliterations and use them as a pipeline.
    /// For example ES -> SR, SR -> EN
    /// </summary>
    class MultiTransliterator : TransliteratorBase
    {
        List<Transliterator> transliterators;

        private string message;

        public MultiTransliterator(List<Language> languages, ARepository repository)
        {
            transliterators = new List<Transliterator>();
            message = "Note: " + languages[0].ToString();

            for (int i = 1; i < languages.Count; i++)
            {
                message += " -> " + languages[i].ToString();
                transliterators.Add(TransliteratorManager.Instance[languages[i - 1].Code, languages[i].Code] as Transliterator);
            }
        }

        public override string Transliterate(string text)
        {
            string output = text;

            foreach (var transliterator in transliterators)
                output = transliterator.Transliterate(output);

            return output;
        }

        public override string Message()
        {
            return message;
        }
    }
}