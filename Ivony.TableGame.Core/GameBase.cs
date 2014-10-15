using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 游戏桌面
  /// </summary>
  public abstract class GameBase
  {



    static GameBase()
    {
      Random = new Random( DateTime.Now.Millisecond );
    }

    /// <summary>
    /// 获取随机数产生器
    /// </summary>
    protected internal static Random Random { get; private set; }


    /// <summary>
    /// 创建游戏对象
    /// </summary>
    protected GameBase( IGameHost gameHost )
    {
      SyncRoot = new object();
      GameState = GameState.NotInitialized;
      PlayerCollection = new List<GamePlayer>();
      GameCancellationSource = new CancellationTokenSource();

      GameHost = gameHost;
    }



    protected IGameHost GameHost
    {
      get;
      private set;
    }

    public string RoomName
    {
      get { return GameHost.RoomName; }
    }

    /// <summary>
    /// 玩家列表
    /// </summary>
    public GamePlayer[] Players
    {
      get { return PlayerCollection.ToArray(); }
    }


    protected List<GamePlayer> PlayerCollection
    {
      get;
      private set;
    }


    /// <summary>
    /// 对所有玩家发出消息
    /// </summary>
    public void AnnounceMessage( string message )
    {

      var _message = new GenericMessage( GameMessageType.Info, message );

      lock ( SyncRoot )
      {
        foreach ( var item in Players )
          item.PlayerHost.WriteMessage( _message );
      }
    }

    public void AnnounceMessage( string format, params object[] args )
    {
      AnnounceMessage( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }



    /// <summary>
    /// 对所有玩家发出系统消息
    /// </summary>
    public void AnnounceSystemMessage( string message )
    {
      var _message = new SystemMessage( message );

      lock ( SyncRoot )
      {
        foreach ( var item in Players )
          item.PlayerHost.WriteMessage( _message );
      }
    }

    public void AnnounceSystemMessage( string format, params object[] args )
    {
      AnnounceSystemMessage( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }


    /// <summary>
    /// 用于同步的对象
    /// </summary>
    public object SyncRoot
    {
      get;
      private set;
    }





    /// <summary>
    /// 尝试加入游戏
    /// </summary>
    /// <param name="gameHost">游戏宿主</param>
    /// <param name="playerHost">玩家宿主</param>
    /// <returns>若加入游戏成功，则返回一个 Player 对象</returns>
    public virtual GamePlayer TryJoinGame( IGameHost gameHost, IPlayerHost playerHost )
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotStarted )
          return null;


        var player = TryJoinGameCore( gameHost, playerHost );
        if ( player != null )
        {
          player.PlayerHost.WriteSystemMessage( string.Format( "恭喜您已经加入 {0} 游戏。", RoomName, player.PlayerName ) );
          AnnounceSystemMessage( "玩家 {0} 已经加入游戏", player.PlayerName );
        }

        return player;
      }
    }

    /// <summary>
    /// 派生类重写此方法尝试将玩家加入游戏
    /// </summary>
    /// <param name="gameHost">游戏宿主</param>
    /// <param name="playerHost">玩家宿主，游戏环境通过玩家宿主与玩家进行通信</param>
    /// <returns>若成功加入游戏，则返回游戏中的玩家</returns>
    protected virtual GamePlayer TryJoinGameCore( IGameHost gameHost, IPlayerHost playerHost )
    {
      throw new NotImplementedException();
    }



    /// <summary>
    /// 游戏状态
    /// </summary>
    public GameState GameState { get; private set; }


    /// <summary>
    /// 用于通知取消游戏进程的通知器
    /// </summary>
    protected CancellationTokenSource GameCancellationSource { get; private set; }

    /// <summary>
    /// 运行游戏
    /// </summary>
    /// <returns></returns>
    public virtual async Task Run()
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotStarted )
          throw new InvalidOperationException();

        GameState = GameState.Running;
      }

      try
      {
        await Task.Yield();
        await RunGame( GameCancellationSource.Token );
      }
      catch ( TaskCanceledException )
      {
        AnnounceSystemMessage( "游戏结束。" );
      }
      catch ( Exception e )
      {
        AnnounceSystemMessage( "游戏出现异常，详细信息为： {0}", e );
      }
      finally
      {
        Release();
      }
    }

    /// <summary>
    /// 派生类实现此方法运行游戏
    /// </summary>
    /// <returns></returns>
    protected virtual Task RunGame( CancellationToken token )
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// 对游戏进行初始化
    /// </summary>
    public void Initialize()
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotInitialized )
          return;


        InitializeCore();
        GameState = GameState.NotStarted;
      }
    }

    protected virtual void InitializeCore()
    { }




    protected void EnsureGameRunning()
    {
      if ( GameState != GameState.Running )
        throw new InvalidOperationException( "游戏状态错误，不在运行状态" );

    }



    /// <summary>
    /// 游戏结束，释放所有资源。
    /// </summary>
    public void Release()
    {
      foreach ( var player in Players )
      {
        player.PlayerHost.QuitGame();
      }
    }

  }
}
