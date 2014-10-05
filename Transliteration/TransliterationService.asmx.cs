using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Oggy.Repository.Entities;

using System.Collections;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.Script.Services;
using System.Diagnostics;
using Oggy.Transliterator;

namespace Transliteration
{
    /// <summary>
    /// Summary description for TransliterationService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TransliterationService1 : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string Transliterate(string text, string srcLang, string dstLang)
        {
            if (srcLang.Length != 2)
                throw new ArgumentException("should be 2 chars long", "srcLang");
            if (dstLang.Length != 2)
                throw new ArgumentException("should be 2 chars long", "dstLang");

            TransliteratorBase transliterator = TransliteratorManager.Instance[srcLang, dstLang];
            return transliterator.Transliterate(text);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public List<Language> GetLanguages()
        {
            return Global.repository.ListLaguagesCached.ToList();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public List<TextSample> ListTextSamples(string code)
        {
            if (code.Length != 2)
                throw new ArgumentException("should be 2 chars long", "code");

            return Global.repository.ListTextSamples(code);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public Language Recognize(string text)
        {
            throw new NotImplementedException("Recognize is not yet implemented");
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Capital(string text)
        {
            return text.ToUpper();
        }
    }
}