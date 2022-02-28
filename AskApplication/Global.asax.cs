using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AskApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();
            MyAppContext.Initialize(System.Web.Hosting.HostingEnvironment.MapPath);
        }
    }
    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext,
          ModelBindingContext bindingContext,
          System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var stringValue = (string)value;
                try
                {
                    if (
                        !string.IsNullOrEmpty(
                            controllerContext.RequestContext.HttpContext.Request[propertyDescriptor.Name]))
                    {
                        stringValue = controllerContext.RequestContext.HttpContext.Request[propertyDescriptor.Name] + "";
                    }

                }
                catch
                {
                    // ignored
                    //如是html，这边会报错
                }
                //if (!string.IsNullOrEmpty(stringValue))
                //    stringValue = stringValue.Trim();

                value = stringValue;
            }

            base.SetProperty(controllerContext, bindingContext,
                                propertyDescriptor, value);
        }
    }
}
