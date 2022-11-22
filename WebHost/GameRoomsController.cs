using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Ivony.TableGame.WebHost
{
  [Route( "GameRooms" )]
  public class GameRoomsController : GameControllerBase
  {

    [HttpGet]
    public object Join( string name )
    {

      lock ( PlayerHost.SyncRoot )
      {
        CheckGameming();

        var game = GameRoomsManager.GetGame( name );
        if ( game == null )
          return new HttpResponseMessage( HttpStatusCode.NotFound );

        game.JoinGame( PlayerHost );
        return new HttpResponseMessage( HttpStatusCode.OK );
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

      return games.Select( item => new { Name = item.RoomName, State = item.GameHostState, Players = item.Game.Players.Select( player => player.PlayerName ) } );
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

        Task.Run( async () =>
         {
           try
           {
             await GameRoomsManager.CreateGame( PlayerHost, name, @private );
           }
           catch ( Exception e )
           {
             PlayerHost.WriteMessage( GameMessage.Error( e.ToString() ) );
           }
         } );
        return new HttpResponseMessage( HttpStatusCode.OK );
      }
    }

    private void CheckGameming()
    {
      if ( PlayerHost.Gaming )
        throw new ActionResultException( Conflict( "当前已经在一个游戏房间，不能创建游戏房间" ) );
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