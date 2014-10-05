using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator
{
    /// <summary>
    /// Transliteration that does nothing i.e. returns the original text
    /// </summary>
    public class EmptyTransliterator : TransliteratorBase
    {
        public override string Transliterate(string text)
        {
            return text;
        }

        public override string Message()
        {
            return string.Empty;
        }
    }
}
