using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Infrastructure.Web
{
    /// <summary>
    /// Make sure to open a connection to the database for each request.
    /// </summary>
    public class TransactionPerRequestModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            CurrentContainer.Container.Build<IPersistenceManager>().Open();
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var pm = CurrentContainer.Container.Build<IPersistenceManager>();

            try
            {
                if (HttpContext.Current.Error == null)
                {
                    pm.Commit();
                }
            }
            finally
            {
                pm.Close();
            }
        }

    }
}
