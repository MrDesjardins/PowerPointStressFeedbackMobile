using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PowerPointStressFeedbackWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                           name: "Short",
                           url: "{sessionId}",
                           defaults: new { controller = "StressFeedback", action = "Index", sessionId = "12345" }
                       );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{sessionId}",
                defaults: new { controller = "StressFeedback", action = "Index", sessionId = "12345" }
            );
           
        }
    }
}
