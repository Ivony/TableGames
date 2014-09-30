using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 代表一局游戏。
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
    protected Game()
    {
      SyncRoot = new object();
      GameState = GameState.NotInstallized;
    }

    public abstract string Name
    {
      get;
    }

    /// <summary>
    /// 舱位列表
    /// </summary>
    public abstract Cabin[] Cabins
    {
      get;
    }

    /// <summary>
    /// 玩家列表
    /// </summary>
    public Player[] Players
    {
      get { return Cabins.Select( item => item.Player ).Where( item => item != null ).ToArray(); }
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
    protected object SyncRoot
    {
      get;
      private set;
    }





    /// <summary>
    /// 尝试加入游戏
    /// </summary>
    /// <returns>若加入游戏成功，则返回一个 Player 对象</returns>
    public virtual Player TryJoinGame( IPlayerHost host )
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotStarted )
          return null;


        var player = TryJoinGameCore( host );
        if ( player != null )
          player.WriteMessage( new SystemMessage( string.Format( "恭喜您已经加入 {0} 游戏", Name ) ) );


        return player;
      }
    }

    /// <summary>
    /// 派生类重写此方法尝试将玩家加入游戏
    /// </summary>
    /// <param name="host">玩家宿主，游戏环境通过玩家宿主与玩家进行通信</param>
    /// <returns>若成功加入游戏，则返回游戏中的玩家</returns>
    protected virtual Player TryJoinGameCore( IPlayerHost host )
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
    public GameProgress StartGame()
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotStarted )
          throw new InvalidOperationException();

        GameState = GameState.InProgress;
        return new GameProgress( this );
      }
    }

    /// <summary>
    /// 对游戏进行初始化
    /// </summary>
    public void Initialize()
    {

      lock ( SyncRoot )
      {
        if ( GameState != GameState.NotInstallized )
          throw new InvalidOperationException();

        InitializeCore();

        GameState = GameState.NotStarted;
      }
    }

    protected virtual void InitializeCore()
    { }
  }
}
