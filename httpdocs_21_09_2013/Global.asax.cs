using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Oggy;

using System.Web.Configuration;
using System.Configuration;
using System.Text;
using Oggy.Transliterator;
using Oggy.Repository;

namespace Transliteration
{
    public class Global : System.Web.HttpApplication
    {
        static public Oggy.Transliterator.Transliterator transliterator;
        static public ARepository repository;
        static public int sessionCounter, translitCounter;

        void Application_Start(object sender, EventArgs e)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            string s = config.ConnectionStrings.ConnectionStrings["RepositoryConnection"].ToString();
            //s = config.ConnectionStrings.ConnectionStrings.ToString();
            repository = new Oggy.Repository.SqlRepository(s);
            TransliteratorManager.Repository = repository;
            transliterator = new Transliterator(repository, "EN", "SR");
            sessionCounter = translitCounter = 0;
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            sessionCounter++;
            // Code that runs when a new session is started
            //Session[
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the session_state mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}
