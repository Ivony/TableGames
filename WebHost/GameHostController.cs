using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public class GameHostController : ApiController
  {




    [HttpGet]
    public object Game( string name )
    {

      var game = GameHost.GetOrCreateGame( name );
      lock ( game.SyncRoot )
      {
        if ( game.GameState != GameState.NotStarted )
          return "游戏已经开始无法加入";

        game.TryJoinGame( PlayerHost );
        return "OK";
      }
    }


    private static string messageIndexCookieKey = "messageIndex";

    [HttpGet]
    public object Messages( HttpRequestMessage request )
    {


      int index;
      if ( !int.TryParse( request.Headers.GetCookieValue( messageIndexCookieKey ), out index ) )
        index = 0;

      var messages = PlayerHost.GetMessages( index );
      index += messages.Length;

      var response = new HttpResponseMessage();

      response.Headers.SetCookieValue( messageIndexCookieKey, index.ToString() );
      

      return response;
    }



    public PlayerHost PlayerHost
    {
      get
      {
        return (PlayerHost) ControllerContext.RequestContext.RouteData.Values[PlayerHostHttpHandler.playerRouteKey];
      }
    }

  }
}