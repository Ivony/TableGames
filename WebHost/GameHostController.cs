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
    public object JoinGame( string name )
    {
      var game = Games.GetOrCreateGame( name );

      game.JoinGame( PlayerHost );
      return "OK";
    }



    [HttpGet]
    public object GameRooms()
    {
      var games = Games.GetAllGames();

      return games.Select( item => new { Name = item.RoomName, State = item.Game.GameState, Players = item.Game.Players.Select( player => player.PlayerName ) } );
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


        Compatibility = PlayerHost.Compatibility,
        GameInformation = GetGameInformation(),
        Messages = PlayerHost.GetMessages(),

        FeatureDeclared = PlayerHost.FeatureDeclared,
      };
    }


    [HttpGet]

    public object Say( string message )
    {
      lock ( PlayerHost.SyncRoot )
      {
        var player = PlayerHost.Player;
        if ( player == null )
          throw new InvalidOperationException();


        player.GameHost.SendChatMessage( new GameChatMessage( player, message ) );
        return "OK";
      }
    }


    /// <summary>
    /// 客户端访问此方法声明自己支持的特性
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    [HttpGet]
    public object DeclaringSupport( [FromUri] string[] feature )
    {

      feature = feature.SelectMany( item => item.Split( ',' ) ).ToArray();


      PlayerHost.SetSupportFeatures( feature );
      return feature;

    }



    /// <summary>
    /// 客户端访问此方法声明自己支持的特性
    /// </summary>
    /// <param name="feature"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<object> DeclaringSupport()
    {

      var text = await ControllerContext.Request.Content.ReadAsStringAsync();
      var features = text.Split( ',' ).ToArray();


      PlayerHost.SetSupportFeatures( features );
      return features;

    }


    [HttpPost]

    public async Task<object> Response( HttpRequestMessage request )
    {

      Guid id;


      if ( request.Content.Headers.ContentType.MediaType != "text/responding" )
        return BadRequest();


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