using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Oggy.DocumentConverter
{
    class TextFileConverter : DocumentConverterBase
    {
        /// <summary>
        /// The signature should be probably like Convert(StreamReader, FileStream)
        /// </summary>
        /// <returns></returns>
        public override void Convert(StreamReader inputStream, FileStream outputStream)
        {
            while (!inputStream.EndOfStream)
            {
                string line = inputStream.ReadLine();
                string transliteratedLine = transliterator.Transliterate(line);
                outputStream.Write(System.Text.Encoding.UTF8.GetBytes(transliteratedLine), 0, line.Length);
            }
        }

        public override string FileExtension
        {
            get { return ".txt"; }
        }
    }
}