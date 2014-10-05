using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository;
using Oggy.Repository.Entities;

namespace Oggy
{
    public class TransliteratorGraph
    {
        private Repository.SqlRepository repository;
        private Dictionary<string, double> edges;
        private List<Language> languages;

        public TransliteratorGraph(Repository.SqlRepository repository)
        {
            this.repository = repository;
            languages = repository.ListLanguages().ToList();
            edges = new Dictionary<string, double>();
            foreach (var sourceLanguage in languages)
                foreach (var destinationLanguage in languages)
                {
                    int count = repository.CountRules();

                    // the formulea for direct distance between two nodes
                    if (count > 0)
                        edges[sourceLanguage.Code + destinationLanguage] = 1.0 / count;
                }
        }

        private Dictionary<string, double> distance;
        private Dictionary<string, string> path;

        public void Path(string source, string destination)
        {
            distance = new Dictionary<string,double>();
            path = new Dictionary<string, string>();
            distance[source] = 0;
            path[source] = source;
            RecursivelyCalculate(source);
        }

        public void RecursivelyCalculate(string source)
        {
            foreach (var language in languages)
            {
                if (language.Code == source)
                    continue;

                string key = source + language.Code;

                if (edges.ContainsKey(key) && distance[source] + edges[key] < distance[language.Code])
                {
                    distance[language.Code] = distance[source] + edges[key];
                    path[language.Code] = path[source] + language.Code;
                    RecursivelyCalculate(language.Code);
                }
            }
        }
    }
}