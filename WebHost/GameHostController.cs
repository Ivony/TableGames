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

      return JoinGame( name );

    }


    [HttpPost]
    public object JoinGame( string name )
    {
      var game = Games.GetOrCreateGame( name );

      game.JoinGame( PlayerHost );
      return "OK";
    }



    [HttpGet]
    public object AllGames()
    {
      var games = Games.GetAllGames();

      return games.Select( item => new { Name = item.RoomName, State = item.Game.GameState, Players = item.Game.Players.Length } );
    }


    [HttpGet]
    public object QuitGame()
    {
      lock ( PlayerHost.SyncRoot )
      {
        if ( !PlayerHost.Gaming )
          return "玩家未加入任何游戏";

        if ( PlayerHost.TryQuitGame() )
          return "OK";

        else
          return "Failed";
      }
    }



    [HttpGet]
    public object Rename( string name )
    {
      return PlayerHost.TrySetName( name );
    }




    [HttpGet]
    public object Status( HttpRequestMessage request, string messageMode = null )
    {

      int amount;
      if ( messageMode == "all" )
        PlayerHost.SetMessageIndex( 0 );

      else if ( int.TryParse( messageMode, out amount ) )
      {
        PlayerHost.GetMessages();
        PlayerHost.SetMessageIndex( Math.Max( PlayerHost.LastMesageIndex - amount, 0 ) );
      }





      return new
      {

        Gaming = PlayerHost.Gaming,
        WaitForResponse = PlayerHost.WaitForResponse,
        PromptText = PlayerHost.PromptText,
        Options = PlayerHost.GetOptions(),

        GameInformation = GetGameInformation(),
        Messages = PlayerHost.GetMessages(),
      };
    }


    [HttpPost]

    public async Task<object> Response( HttpRequestMessage request )
    {

      var message = await request.Content.ReadAsStringAsync();

      PlayerHost.OnResponse( message );
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


    protected PlayerHost PlayerHost
    {
      get
      {
        return (PlayerHost) ControllerContext.Request.Properties[PlayerHostHttpHandler.playerKey];
      }
    }

  }
}