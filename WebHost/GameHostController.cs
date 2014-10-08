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


      game.JoinGame( PlayerHost );
      return RedirectToRoute( "Default", new { controller = "GameHost", action = "Messages" } );

    }



    [HttpGet]
    public object Messages( HttpRequestMessage request, long timeStamp = 0 )
    {

      var time = DateTime.UtcNow;
      var messages = PlayerHost.GetMessages( timeStamp );
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