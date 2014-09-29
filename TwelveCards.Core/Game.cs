using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards.Core
{

  /// <summary>
  /// 代表一局游戏。
  /// </summary>
  public class Game
  {

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
      get;
      private set;
    }


    /// <summary>
    /// 进行一个游戏操作
    /// </summary>
    /// <param name="player">发起操作的玩家</param>
    /// <param name="target">目标舱位</param>
    /// <param name="card">使用卡牌</param>
    public void Do( Player player, Cabin target, Card card )
    {

      card.Action( this, player, target );
    }



    /// <summary>
    /// 发出游戏公告，所有玩家都能收到这一公告
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void Announce( string format, params object[] args )
    {
      foreach ( var p in Players )
        p.Console.WriteLine( format, args );
    }
  }
}
