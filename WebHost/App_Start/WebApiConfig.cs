using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public static class WebApiConfig
  {
    public static void Register( HttpConfiguration config )
    {
      // Web API 配置和服务
      config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
      config.Services.Replace( typeof( IContentNegotiator ), new JsonContentNegotiator() );

      config.ParameterBindingRules.Add( PlayerHost.GetBinding );


      // Web API 路由
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute( name: "DefaultApi", routeTemplate: "{controller}/{action}"
      );
    }
  }
}
