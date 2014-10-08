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

      var game = Games.GetOrCreateGame( name );

      string reason;

      if ( game.TryJoinGame( PlayerHost, out reason ) )
        return "OK";

      return "加入游戏失败，" + reason;

    }


    private static string messageIndexCookieKey = "messageIndex";

    [HttpGet]
    public object Messages( HttpRequestMessage request )
    {

      var time = DateTime.UtcNow;
      var messages = PlayerHost.GetMessages();
      return new
      {
        Messages = messages,
        TimeStamp = time.Ticks,
      };
    }



    public PlayerHost PlayerHost
    {
      get
      {
        return (PlayerHost) ControllerContext.Request.Properties[PlayerHostHttpHandler.playerKey];
      }
    }

  }
}