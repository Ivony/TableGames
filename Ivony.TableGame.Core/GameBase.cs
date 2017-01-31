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
  /// 游戏基类，提供桌面游戏的基本辅助方法
  /// </summary>
  public abstract class GameBase
  {



    static GameBase()
    {
      Random = new Random( DateTime.Now.Millisecond );
    }


    /// <summary>
    /// 获取用于该游戏的随机数产生器
    /// </summary>
    protected internal static Random Random { get; private set; }


    /// <summary>
    /// 创建游戏对象
    /// </summary>
    protected GameBase( IGameHost gameHost )
    {
      SyncRoot = new object();
      GameState = GameState.NotInitialized;
      PlayerCollection = new List<GamePlayerBase>();
      GameCancellationSource = new CancellationTokenSource();

      GameHost = gameHost;
    }


    /// <summary>
    /// 获取承载该游戏的游戏宿主
    /// </summary>
    protected IGameHost GameHost
    {
      get;
      private set;
    }

    /// <summary>
    /// 当前游戏房间名称
    /// </summary>
    public string RoomName
    {
      get { return GameHost.RoomName; }
    }

    /// <summary>
    /// 玩家列表
    /// </summary>
    public GamePlayerBase[] Players
    {
      get { return PlayerCollection.ToArray(); }
    }


    /// <summary>
    /// 玩家容器，派生类可以通过该容器来增删游戏中的玩家
    /// </summary>
    protected List<GamePlayerBase> PlayerCollection
    {
      get;
      private set;
    }




    /// <summary>
    /// 对所有玩家广播一条消息
    /// </summary>
    /// <param name="massage">要广播的消息对象</param>

    public void Announce( GameMessage massage )
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Players )
          item.PlayerHost.WriteMessage( massage );
      }
    }


    /// <summary>
    /// 对所有玩家广播一个消息
    /// </summary>
    public void AnnounceMessage( string message )
    {

      Announce( new GenericMessage( GameMessageType.Info, message ) );

    }

    /// <summary>
    /// 对所有玩家广播一个消息
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void AnnounceMessage( string format, params object[] args )
    {
      AnnounceMessage( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }



    /// <summary>
    /// 对所有玩家广播一条系统消息
    /// </summary>
    public void AnnounceSystemMessage( string message )
    {
      Announce( new SystemMessage( message ) );

    }

    /// <summary>
    /// 对所有玩家广播一条系统消息
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
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
    public virtual GamePlayerBase TryJoinGame( IGameHost gameHost, IPlayerHost playerHost )
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.Initialized )
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
    protected virtual GamePlayerBase TryJoinGameCore( IGameHost gameHost, IPlayerHost playerHost )
    {
      throw new NotImplementedException();
    }


    /// <summary>
    /// 当玩家退出游戏时，调用此方法通知游戏
    /// </summary>
    /// <param name="player">退出游戏的玩家</param>
    public virtual void OnPlayerQuitted( GamePlayerBase player ) { }




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
        if ( GameState != GameState.Initialized )
          throw new InvalidOperationException();

        GameState = GameState.Running;
      }

      try
      {
        await Task.Yield();
        await RunGame( GameCancellationSource.Token );
      }
      catch ( OperationCanceledException )
      {
        if ( GameCancellationSource.IsCancellationRequested )
          AnnounceSystemMessage( "游戏结束。" );
      }
      catch ( Exception e )
      {
        AnnounceSystemMessage( "游戏出现异常，详细信息为： {0}", e );
      }
      finally
      {
        ReleaseGame();
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
        GameState = GameState.Initialized;
      }
    }

    /// <summary>
    /// 派生类实现此方法完成初始化工作
    /// </summary>
    protected virtual void InitializeCore()
    { }




    /// <summary>
    /// 确保游戏正在运行
    /// </summary>
    protected void EnsureGameRunning()
    {
      if ( GameState != GameState.Running )
        throw new InvalidOperationException( "游戏状态错误，不在运行状态" );

    }



    /// <summary>
    /// 终止游戏，释放所有资源。
    /// </summary>
    public virtual void ReleaseGame()
    {

      GameCancellationSource.Cancel();//立即终止游戏进程

      lock ( SyncRoot )
      {
        foreach ( var player in Players )
        {

          if ( player.PlayerHost.TryQuitGame() == false )
            throw new InvalidOperationException( string.Format( "释放玩家 {0} 失败", player.PlayerHost.Name ) );
        }

        GameHost.ReleaseGame( this );
      }
    }


    /// <summary>
    /// 获取参与此游戏客户端必须支持的特性列表
    /// </summary>
    public virtual IEnumerable<string> GetRequiredFeatures()
    {
      return Enumerable.Empty<string>();
    }
  }
}
