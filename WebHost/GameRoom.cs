using Ivony.TableGame.SimpleGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class GameRoom : GameHostBase
  {


    private GameRoom( string roomName, bool privateRoom ) : base( roomName )
    {
      PrivateRoom = privateRoom;
    }



    public void JoinGame( IPlayerHost player )
    {
      string reason;
      if ( !TryJoinGame( player, out reason ) )
        player.WriteWarningMessage( "加入游戏 \"{0}\" 失败，原因为： {1}", Game.RoomName, reason );

    }


    /// <summary>
    /// 运行游戏
    /// </summary>
    /// <returns>用于等待游戏运行的 Task 对象</returns>
    public override Task Run()
    {
      return Task.Run( () => base.Run() );
    }


    /// <summary>
    /// 释放游戏
    /// </summary>
    /// <param name="game"></param>
    public override void ReleaseGame( GameBase game )
    {
      base.ReleaseGame( game );
      GameRoomsManager.ReleaseRoom( this );
    }


    /// <summary>
    /// 是否为私有房间
    /// </summary>
    public bool PrivateRoom { get; }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    public GameState GameState
    {
      get
      {
        return Game?.GameState ?? GameState.NotInitialized;
      }
    }

    /// <summary>
    /// 创建游戏宿主
    /// </summary>
    /// <param name="name">房间名称</param>
    /// <param name="type">游戏类型</param>
    /// <param name="privateRoom">是否为私有房间</param>
    /// <returns>游戏宿主</returns>
    public static GameRoom Create( string name, string type, bool privateRoom )
    {

      var host = new GameRoom( name, privateRoom );
      host.InitializeGame( new SimpleGame() );

      return host;


    }
  }
}