using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{

  /// <summary>
  /// 代表一局游戏。
  /// </summary>
  public class Game
  {


    public Game( CardDealer cardDealer, int cabins )
    {

      CardDealer = cardDealer;
      Cabins = new Cabin[cabins];
      MaximumPlayers = cabins / 2;

    }



    /// <summary>
    /// 发牌器
    /// </summary>
    public CardDealer CardDealer
    {
      get;
      private set;
    }

    /// <summary>
    /// 舱位列表
    /// </summary>
    public Cabin[] Cabins
    {
      get;
      private set;
    }

    /// <summary>
    /// 玩家列表
    /// </summary>
    public Player[] Players
    {
      get { return Cabins.Select( item => item.Player ).Where( item => item != null ).ToArray(); }
    }





    /// <summary>
    /// 发出游戏公告，所有玩家都能收到这一公告
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void Announce( string format, params object[] args )
    {
      foreach ( var p in Players )
        p.WriteMessage( format, args );
    }



    private bool _gameStarted = false;
    private object _sync = new object();



    private Random random = new Random( DateTime.Now.Millisecond );


    /// <summary>
    /// 尝试加入游戏
    /// </summary>
    /// <returns>若加入游戏成功，则返回一个 Player 对象</returns>
    public Player TryJoinGame( IPlayerHost host )
    {
      lock ( _sync )
      {
        if ( _gameStarted )
          return null;

        if ( Players.Length == MaximumPlayers )
          return null;


        var candidates = Cabins.Where( item => item.Player == null ).ToArray();
        var cabin = candidates[random.Next( candidates.Length )];
        return CreatePlayer( cabin, host );
      }
    }



    private Player CreatePlayer( Cabin cabin, IPlayerHost host )
    {
      var codeName = GetCodeName();
      var player = new Player( codeName, cabin, host );
      cabin.SetPlayer( player );
      return player;

    }

    private string GetCodeName()
    {
      return "扯犊子";
    }


    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <returns></returns>
    public GameProgress StartGame()
    {

      lock ( _sync )
      {
        _gameStarted = true;

        return new GameProgress( this );
      }
    }


    public int MaximumPlayers { get; private set; }
  }
}
