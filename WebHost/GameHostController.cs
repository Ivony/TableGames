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

    [HttpGet]
    public object Messages()
    {



      return PlayerHost.GetMessages();
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