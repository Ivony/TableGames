using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

      game.JoinGame( PlayerHost );
      return "OK";

    }



    [HttpGet]
    public object Status( HttpRequestMessage request, string messageMode = null )
    {
      if ( messageMode == "all" )
        PlayerHost.SetMessageIndex( 0 );


      return new
      {

        Gaming = PlayerHost.Gaming,
        WaitForResponse = PlayerHost.WaitForResponse,
        PromptText = PlayerHost.PromptText,

        GameInformation = GetGameInformation(),
        Messages = PlayerHost.GetMessages(),
      };
    }


    [HttpPost]

    public async Task<object> Response( HttpRequestMessage request )
    {

      var message = await request.Content.ReadAsStringAsync();

      PlayerHost.Response( message );
      return "OK";

    }

    private object GetGameInformation()
    {

      var player = PlayerHost.Player;

      if ( player == null )
        return null;

      return new
      {
        GameRoomName = player.GameHost.RoomName,
        Data = player.GetGameInformation(),
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