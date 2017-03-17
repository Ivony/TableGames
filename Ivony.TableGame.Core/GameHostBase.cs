using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 提供 IGameHost 的基础实现
  /// </summary>
  public abstract class GameHostBase : IGameHost
  {


    /// <summary>
    /// 创建 GameHostBase 对象
    /// </summary>
    /// <param name="roomName">游戏房间名称</param>
    protected GameHostBase( string roomName )
    {
      RoomName = roomName;
      ID = Guid.NewGuid();
      Data = new Dictionary<string, object>();
    }



    /// <summary>
    /// 获取游戏宿主唯一ID
    /// </summary>
    public Guid ID { get; }


    /// <summary>
    /// 宿主名称
    /// </summary>
    public string RoomName { get; }


    /// <summary>
    /// 游戏房间数据
    /// </summary>
    public Dictionary<string, object> Data { get; }



    /// <summary>
    /// 派生类实现此属性获取游戏对象
    /// </summary>
    public GameBase Game { get; private set; }


    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="game">游戏对象</param>
    protected GameBase InitializeGame( GameBase game )
    {
      if ( game.GameState != GameState.NotInitialized )
        throw new InvalidOperationException();

      lock ( SyncRoot )
      {
        if ( Game != null )
          throw new InvalidOperationException();

        Game = game;
        Game.Initialize( this );

        return Game;
      }
    }



    /// <summary>
    /// 尝试加入游戏
    /// </summary>
    /// <param name="playerHost"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public virtual bool TryJoinGame( IPlayerHost playerHost, out string reason )
    {
      var player = playerHost.Player;
      reason = null;

      if ( player != null )
      {
        if ( player.GameHost == this )
          return true;

        else
        {
          reason = "玩家已经加入其他游戏";
          return false;
        }
      }
      else
      {
        if ( Game == null || Game.GameState == GameState.NotInitialized || Game.GameState == GameState.Initializing )
        {
          reason = "游戏尚未初始化";
          return false;
        }
        else if ( Game.GameState != GameState.Initialized )
        {
          reason = "游戏已经开始或结束";
          return false;
        }

        player = Game.TryJoinGame( playerHost );
        if ( player == null )
        {
          reason = "未知原因";
          return false;
        }
        else
        {
          playerHost.OnJoinedGame( player );
          return true;
        }

      }
    }


    /// <summary>
    /// 运行游戏
    /// </summary>
    /// <returns></returns>
    public virtual Task Run()
    {
      lock ( SyncRoot )
      {
        if ( Game.GameState == GameState.NotInitialized )
          throw new InvalidOperationException( "游戏尚未初始化" );
      }

      return Game.Run();
    }


    /// <summary>
    /// 释放游戏资源
    /// </summary>
    /// <param name="game">要释放资源的游戏</param>
    public virtual void ReleaseGame( GameBase game )
    {

      if ( game != Game )
        throw new InvalidOperationException();

    }



    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; } = new object();




    /// <summary>
    /// 推送一条聊天消息
    /// </summary>
    /// <param name="message">聊天消息对象</param>
    public void SendChatMessage( GameChatMessage message )
    {
      Game.Announce( message );
    }
  }
}
