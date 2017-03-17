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
  public class GameRoomsController : GameControllerBase
  {

    [HttpGet]
    public object Join( string name )
    {

      lock ( PlayerHost.SyncRoot )
      {
        CheckGameming();

        try
        {
          var game = GameRoomsManager.GetGame( name );
          if ( game == null )
            return new HttpResponseMessage( HttpStatusCode.NotFound );

          game.JoinGame( PlayerHost );
          return new HttpResponseMessage( HttpStatusCode.OK );
        }
        catch ( ArgumentException )
        {
          PlayerHost.WriteWarningMessage( "房间名称不合法，必须由不超过10个英文字母或者不超过5个中文字符组成" );
          return new HttpResponseMessage( HttpStatusCode.BadRequest );
        }
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
    public object Create( string name, bool @private = false )
    {
      lock ( PlayerHost.SyncRoot )
      {
        CheckGameming();

        var task = GameRoomsManager.CreateGame( PlayerHost, name, @private );
        return new HttpResponseMessage( HttpStatusCode.OK );
      }
    }

    private void CheckGameming()
    {
      if ( PlayerHost.Gaming )
        throw new HttpResponseException( new HttpResponseMessage { StatusCode = HttpStatusCode.Forbidden, Content = new StringContent( "当前已经在一个游戏房间，不能创建游戏房间" ) } );
    }


    /// <summary>
    /// 退出当前游戏房间
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public object QuitGame()
    {
      PlayerHost.QuitGame();
      return new HttpResponseMessage( HttpStatusCode.OK );
    }
  }
}