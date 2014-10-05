using System.Collections.Generic;
using Oggy.Repository.Entities;

namespace Oggy.Repository
{
    public abstract class ARepository
    {
        #region Variables
        public Language srcLanguage;
        public Language dstLanguage;
        #endregion

        public ARepository() { }

        #region General
        /// <summary>
        /// This function should be invoked within constructor
        /// </summary>
        public void SetUp()
        {
            SetSourceLanguage("EN");
            SetDstLanguage("SR");
        }

        public void SwapSourceAndDestination()
        {
            Language temp = srcLanguage;
            SetSourceLanguage(dstLanguage.Code);
            SetDstLanguage(temp.Code);
        }
        
        public virtual void SetSourceLanguage(string code)
        {
            srcLanguage = RetreiveLanguage(code);
        }

        public void SetDstLanguage(string code)
        {
            dstLanguage = RetreiveLanguage(code);
        }
        #endregion

        #region CRUD for Languages + LanguageList Cache
        public abstract IEnumerable<Language> ListLanguages(bool enabledOnly=false);
        public abstract void CreateLanguage(Language language);
        public abstract Language RetreiveLanguage(string code);
        public abstract void UpdateLanguage(Language code);
        public abstract bool DeleteLanguage(string code);

        private List<Language> listLaguagesCached;
        public IEnumerable<Language> ListLaguagesCached
        {
            get
            {
                if (listLaguagesCached == null)
                    listLaguagesCached = new List<Language>(ListLanguages(true));
                return listLaguagesCached;
            }
        }
        #endregion

        #region Word
        public abstract IEnumerable<Word> ListWords();
        
        public abstract void CreateWord(Word word);
        public abstract void UpdateWord(Word word);
        public abstract void DeleteWord(string word);
        #endregion

        #region TransliterationRules
        public abstract List<TransliterationRule> ListRules(bool enabledOnly = true);
        public virtual int CountRules(bool enabledOnly = true)
        {
            return ListRules().Count;
        }
        public abstract void CreateRule(TransliterationRule rule);
        public abstract void UpdateRule(TransliterationRule rule, string source);
        public abstract void DeleteRule(string source);
        #endregion

        #region TransliterationExamples
        public abstract List<TransliterationExample> ListExamples(bool enabledOnly = true);
        public abstract void CreateExample(TransliterationExample example);
        public abstract void UpdateExample(TransliterationExample example, string word);
        public abstract void DeleteExample(string word);
        #endregion

        #region Jokers
        public abstract Dictionary<char, string> GetJokers();
        public abstract void AddJoker(char joker, string substitution);
        public abstract void UpdateJoker(char joker, string substitution);
        public abstract void DeleteJoker(char joker);
        #endregion

        #region TextSamples
        public abstract List<TextSample> ListTextSamples(string langCode);
        #endregion

        #region Letters Distribution
        public abstract Dictionary<char, double> LoadLetterDistribution(string langCode);
        #endregion
    }
}