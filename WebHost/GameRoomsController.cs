using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public class GameRoomsController : GameControllerBase
  {

    [HttpGet]
    public object Join( string name )
    {

      try
      {
        var game = GameRoomsManager.GetGame( name );
        if ( game == null )
          return new HttpResponseMessage( HttpStatusCode.NotFound );

        game.JoinGame( PlayerHost );
        return new HttpResponseMessage( HttpStatusCode.OK );
      }
      catch ( ArgumentException e )
      {
        PlayerHost.WriteWarningMessage( "房间名称不合法，必须由不超过10个英文字母或者不超过5个中文字符组成" );
        return new HttpResponseMessage( HttpStatusCode.BadRequest );
      }
    }



    /// <summary>
    /// 列出所有游戏房间
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public object List()
    {
      var games = GameRoomsManager.PublicGames();

      return games.Select( item => new { Name = item.RoomName, State = item.GameState, Players = item.Game.Players.Select( player => player.PlayerName ) } );
    }


    /// <summary>
    /// 创建游戏房间
    /// </summary>
    /// <param name="name">房间名</param>
    /// <param name="type">房间类型</param>
    /// <param name="private">是否为私有房间</param>
    /// <returns></returns>
    [HttpGet]
    public object Create( string name, string type = null, bool @private = false )
    {
      return GameRoomsManager.CreateGame( name, type, @private );

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




  }
}