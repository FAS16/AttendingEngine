﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AttendingEngine.App_Start;
using Newtonsoft.Json;

namespace AttendingEngine
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapper.Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpConfiguration config = GlobalConfiguration.Configuration;
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            settings.NullValueHandling = NullValueHandling.Ignore;


        }
    }
}
