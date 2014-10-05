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
        protected Transliterator.Transliterator transliterator;

        public DocumentConverterBase(Transliterator.Transliterator transliterator=null)
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
