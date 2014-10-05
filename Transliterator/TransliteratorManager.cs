using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository;
using Oggy.Repository.Entities;

namespace Oggy.Transliterator
{
    /// <summary>
    /// Manages empty, single and multi transliterations.
    /// It gets the right transliteration for each pair of languages.
    /// </summary>
    public class TransliteratorManager
    {
        #region Variables
        private readonly Language SERBIAN;
        internal const string SERBIAN_CODE = "SR";

        private static TransliteratorManager instance;
        
        /// <summary>
        /// Singletone pattern
        /// </summary>
        public static TransliteratorManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TransliteratorManager();
                
                return instance;
            }
        }
        
        private Dictionary<string, TransliteratorBase> transliterators;

        private EmptyTransliterator emptyTransliterator;

        public static ARepository Repository
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public TransliteratorManager()
        {
            SERBIAN = Repository.RetreiveLanguage(SERBIAN_CODE);
            emptyTransliterator = new EmptyTransliterator();
            transliterators = new Dictionary<string, TransliteratorBase>();
        }
        
        public TransliteratorBase this[string src, string dst]
        {
            get
            {
                string key = src + dst;
                lock (this)
                {
                    if (!transliterators.ContainsKey(key))
                    {
                        if (src == dst)
                            return emptyTransliterator;

                        if (src == SERBIAN.Code || dst == SERBIAN.Code)
                        {
                            Repository.SetSourceLanguage(src);
                            Repository.SetDstLanguage(dst);
                            transliterators[key] = new Transliterator(Repository, src, dst);
                        }
                        else
                        {
                            transliterators[key] = new MultiTransliterator(
                                new List<Language>() {
                                    Repository.RetreiveLanguage(src),
                                    SERBIAN,
                                    Repository.RetreiveLanguage(dst) },
                                Repository);
                        }
                    }
                }
                return transliterators[key];
            }
        }
        #endregion
    }
}