using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalyzer
{
    public class TextAnalyzerWorker
    {
        static void add<T>(Dictionary<T, int> sum, T item)
        {
            if (sum.ContainsKey(item))
                sum[item]++;
            else
                sum[item] = 1;
        }

        public Dictionary<char, int> letters;
        public Dictionary<string, int> capitalWords;
        public Dictionary<string, int>[] all_words;

        public TextAnalyzerWorker(string text)
        {
            letters = new Dictionary<char, int>();
            foreach (char c in text)
            {
                char.GetUnicodeCategory(c).ToString();
                if (char.IsLetter(c))
                    add(letters, char.ToUpper(c));
            }
            letters.Add('*', letters.Sum(pair => pair.Value));

            const int NOT_INITIALIZED = -1;
            int MAX_WORDS = 4;
            
            int[] pointers = Enumerable.Repeat(-1, MAX_WORDS).ToArray();

            all_words = new Dictionary<string, int>[MAX_WORDS];
            for (int i = 0; i < MAX_WORDS; i++)
                all_words[i] = new Dictionary<string, int>();

            capitalWords = new Dictionary<string, int>();

            bool inWord = false, specialWordCharaceter = false;
            for (int i=0; i<text.Length; i++)
            {
                specialWordCharaceter = false;
                char c = char.ToLower(text[i]);
                if (char.IsLetter(c))
                {
                    specialWordCharaceter = false;

                    if (inWord)
                        continue;

                    inWord = true;
                    for (int j = MAX_WORDS - 1; j > 0; j--)
                        pointers[j] = pointers[j - 1];

                    pointers[0] = i;
                }
                else if ((c == '\'' || c=='-') && inWord && !specialWordCharaceter)
                {
                    specialWordCharaceter = true;
                }
                else if (inWord)
                {
                    for (int j = 0; j < MAX_WORDS && pointers[j] != NOT_INITIALIZED; j++)
                    {
                        string key = text.Substring(pointers[j], i - pointers[j]);

                        if (j == 0 && char.IsUpper(key[0]))
                            add(capitalWords, key);
                        
                        key = key.ToLower();
                        add(all_words[j], key);
                    }
                    
                    if (c!=' ')
                        for (int j = 0; j < pointers.Length; j++)
                            pointers[j] = NOT_INITIALIZED;

                    inWord = false;
                }
                else
                {
                    for (int j = 0; j < pointers.Length; j++)
                        pointers[j] = NOT_INITIALIZED;
                }
            }

            for (int i = 0; i < MAX_WORDS; i++)
                all_words[i].Add("[all]", all_words[i].Sum(pair => pair.Value));

            capitalWords.Add("[ALL]", capitalWords.Sum(pair => pair.Value));
        }
    }
}