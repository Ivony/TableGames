using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 游戏桌面
  /// </summary>
  public abstract class Game
  {



    static Game()
    {
      Random = new Random( DateTime.Now.Millisecond );
    }

    /// <summary>
    /// 获取随机数产生器
    /// </summary>
    protected static Random Random { get; private set; }


    /// <summary>
    /// 创建游戏对象
    /// </summary>
    protected Game( string name )
    {
      SyncRoot = new object();
      GameState = GameState.NotInitialized;
      PlayerCollection = new List<GamePlayer>();

      Name = name;
    }

    public virtual string Name
    {
      get;
      private set;
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
        foreach ( var p in Players )
          p.WriteMessage( _message );
      }
    }


    /// <summary>
    /// 对所有玩家发出系统消息
    /// </summary>
    public void AnnounceSystemMessage( string message )
    {
      var _message = new SystemMessage( message );

      lock ( SyncRoot )
      {
        foreach ( var p in Players )
          p.WriteMessage( _message );
      }
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
          player.WriteMessage( new SystemMessage( string.Format( "恭喜您已经加入 {0} 游戏", Name ) ) );

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
    /// 开始游戏
    /// </summary>
    /// <returns></returns>
    public virtual GameProgress StartGame()
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotStarted )
          throw new InvalidOperationException();

        GameState = GameState.Running;
        return StartGameCore();
      }
    }

    protected virtual GameProgress StartGameCore()
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




    /// <summary>
    /// 游戏结束，释放所有资源。
    /// </summary>
    public void Release()
    {
      foreach ( var player in Players )
      {
        player.PlayerHost.LeavedGame();
      }
    }

  }
}
