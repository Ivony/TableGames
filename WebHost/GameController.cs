using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public class GameController : ApiController
  {




    [HttpGet]
    public object CreateGame()
    {

      var game = new GameHost();
      game.Initialize();

      var result = game.TryJoinGame( PlayerHost );

      return "OK";
    }

    [HttpGet]
    public object GetMessages()
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