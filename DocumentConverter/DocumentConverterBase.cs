using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Transliterator;
using System.IO;

namespace Oggy.DocumentConverter
{
    abstract class DocumentConverterBase
    {
        protected Transliterator.TransliteratorOld transliterator;

        public DocumentConverterBase(Transliterator.TransliteratorOld transliterator=null)
        {
            this.transliterator = transliterator;
        }

        public DocumentConverterBase(string fileName)
        {

        }

        abstract public void Convert(StreamReader inputStream, FileStream outputStream);

        abstract public string FileExtension
        { get; }
    }
}
