using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Oggy.DocumentConverter
{
    class PDFConverter : DocumentConverterBase
    {
        public override void Convert(StreamReader inputStream, FileStream outputStream)
        {
            throw new NotImplementedException();
        }

        public override string FileExtension
        {
            get { return ".pdf"; }
        }
    }
}
