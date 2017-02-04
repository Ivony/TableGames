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
  public class GameHostController : GameControllerBase
  {





    [HttpGet]
    public object JoinGame( string name )
    {

      try
      {
        var game = Games.GetOrCreateGame( name );
        game.JoinGame( PlayerHost );
      }
      catch ( ArgumentException e )
      {
        PlayerHost.WriteWarningMessage( "房间名称不合法，必须由不超过10个英文字母或者不超过5个中文字符组成" );
        return new HttpResponseMessage( HttpStatusCode.BadRequest );
      }

      return new HttpResponseMessage( HttpStatusCode.OK );
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
        RespondingUrl = PlayerHost.Responding == null ? null : "Responding/" + PlayerHost.Responding.RespondingID,

        GameInformation = GetGameInformation(),
        Messages = PlayerHost.GetMessages(),
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
    /// 客户端访问此方法获取游戏请求的特性列表
    /// </summary>
    /// <returns>游戏所需的特性</returns>
    [HttpGet]
    public string[] RequiredFeatures()
    {
      return PlayerHost.Player?.Game?.GetRequiredFeatures()?.ToArray();
    }


    private IResponding GetResponding( HttpRequestMessage request )
    {

      var responding = PlayerHost.Responding;

      if ( responding == null )
        return null;


      IEnumerable<string> values;
      if ( request.Headers.TryGetValues( "Responding", out values ) == false )
        return responding;


      Guid id;
      if ( Guid.TryParse( values.FirstOrDefault(), out id ) )
      {
        if ( responding.RespondingID == id )
          return responding;

        else
          return null;
      }

      return responding;
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


  }
}