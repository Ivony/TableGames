using Ivony.TableGame.SimpleGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
      if ( !TryJoinGame( player, out var reason ) )
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
      get { return Game?.GameState ?? GameState.NotInitialized; }
    }

    /// <summary>
    /// 获取游戏宿主状态
    /// </summary>
    public GameHostState GameHostState
    {
      get
      {
        if ( Game == null || Game.GameState < GameState.Initialized )
          return GameHostState.Initializing;

        else if ( Game.GameState == GameState.Initialized )
          return GameHostState.WaitForPlayer;

        else
          return GameHostState.Running;
      }
    }


    /// <summary>
    /// 创建游戏宿主
    /// </summary>
    /// <param name="name">房间名称</param>
    /// <param name="type">游戏类型</param>
    /// <param name="privateRoom">是否为私有房间</param>
    /// <returns>游戏宿主</returns>
    public static GameRoom Create( string name, bool privateRoom )
    {
      return new GameRoom( name, privateRoom );
    }


    /// <summary>
    /// 初始化游戏房间
    /// </summary>
    /// <param name="initializer">游戏创建者</param>
    internal async Task Initialize( PlayerHost initializer )
    {
      try
      {
        var options = GameManager.RegisteredGames.Select( item => Option.Create( item, item.GameName, item.GameDescription ) ).ToArray();
        if ( options.Any() == false )
        {
          initializer.WriteSystemMessage( "当前没有已经注册的游戏，无法创建房间" );
          throw new InvalidOperationException();
        }

        var factory = await initializer.Console.Choose( "请选择游戏类型：", options, CancellationToken.None );
        await InitializeGame( factory.CreateGame(), initializer );
      }
      catch
      {
        GameRoomsManager.ReleaseRoom( this );
      }

    }
  }
}