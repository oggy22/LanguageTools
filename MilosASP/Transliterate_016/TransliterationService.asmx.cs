using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Oggy.Repository.Entities;

using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.Script.Services;
using System.Diagnostics;

namespace Transliteration
{
    /// <summary>
    /// Summary description for TransliterationService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TransliterationService1 : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string Transliterate(string text)
        {
            string ret = Global.transliterator.Transliterate(text);
            return ret;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public List<Language> GetLanguages()
        {
            return Global.repository.ListLaguagesCached;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public Language Recognize(string text)
        {
            throw new NotImplementedException("Recognize is not implemented yet");
        }

        //public StructredText GetStructuredTransliteration(string text)
        //{
        //    //TODO:
        //    return new StructredText();
        //}
    }

    //public class StructredText
    //{
    //    private string text;

    //    /// <summary>
    //    /// Positions of the correspondant letter in the original.
    //    /// The lenght should be the same as text.length
    //    /// </summary>
    //    private int[] headers;
    //}
}