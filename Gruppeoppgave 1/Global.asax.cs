//using Gruppeoppgave_1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gruppeoppgave_1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();      
            //fikser feil "Model backing a DB Context has changed; Consider Code First Migrations"
            //Database.SetInitializer<BestillingContext>(new DropCreateDatabaseIfModelChanges<BestillingContext>());
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }
    }
}
