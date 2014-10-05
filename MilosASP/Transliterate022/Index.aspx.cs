using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oggy.Repository;
using Oggy.Transliterator;

namespace Transliteration
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SrcLangDropDownList.Items.Count > 0)
                return;

            SrcLangDropDownList.Items.Clear();
            DstLangDropDownList.Items.Clear();

            foreach (var lang in Global.repository.ListLanguages(true))
            {
                SrcLangDropDownList.Items.Add(lang.ToString());
                if (lang.Code == "EN")
                    SrcLangDropDownList.SelectedValue = lang.ToString();

                DstLangDropDownList.Items.Add(lang.ToString());
                if (lang.Code == "SR")
                    DstLangDropDownList.SelectedValue = lang.ToString();
            }

            string srcLang = Response.Cookies["Transliteration"]["src"];
            if (srcLang != null)
            {
                int i=0;
                foreach (var item in SrcLangDropDownList.Items)
                {
                    if (item.ToString().StartsWith(srcLang))
                        SrcLangDropDownList.SelectedIndex = i;
                    i++;
                }
            }
        }

        protected void Transliterate(object sender, EventArgs e)
        {
            Global.translitCounter++;
            string srcLang = SrcLangDropDownList.SelectedValue.ToString().Substring(0, 2);
            string dstLang = DstLangDropDownList.SelectedValue.ToString().Substring(0, 2);

            TransliteratorBase transliterator = TransliteratorManager.Instance[srcLang, dstLang];

            DestinationText.Text = transliterator.Transliterate(SourceText.Text);
            NoteLabel.Text = transliterator.Message();
            //UpdateLabel();
        }

        private void UpdateLabel()
        {
            //Label1.Text = string.Format("Sessions={0} Transliterations={1}",
            //    Global.sessionCounter,
            //    Global.translitCounter);
        }
    }
}