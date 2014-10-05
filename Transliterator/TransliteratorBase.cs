using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator
{
    public abstract class TransliteratorBase
    {
        public abstract string Transliterate(string text);

        /// <summary>
        /// Message to be displayed if the transliteration was not direct, e.g. EN->SR->DE
        /// </summary>
        /// <returns></returns>
        public virtual string Message()
        {
            return string.Empty;
        }
    }
}