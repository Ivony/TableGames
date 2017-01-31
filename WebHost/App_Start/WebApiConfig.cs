using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

      config.MessageHandlers.Add( new PlayerHostHttpHandler() );

      JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
      jsonSetting.Converters.Add( new StringEnumConverter() );
      config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;



      // Web API 路由
      config.Routes.MapHttpRoute( name: "JoinGame", routeTemplate: "JoinGame/{name}", defaults: new { controller = "GameHost", action = "JoinGame" } );
      config.Routes.MapHttpRoute( name: "Rename", routeTemplate: "Rename/{name}", defaults: new { controller = "GameHost", action = "Rename" } );
      config.Routes.MapHttpRoute( name: "JoinGameLegacy", routeTemplate: "Game", defaults: new { controller = "GameHost", action = "JoinGame" } );
      config.Routes.MapHttpRoute( name: "Default", routeTemplate: "{action}", defaults: new { controller = "GameHost", action = "Status" } );


      config.Routes.MapHttpRoute( name: "Responding", routeTemplate: "Responding/{id}", defaults: new { controller = "Responding" } );
    }
  }
}
