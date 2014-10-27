using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{


  /// <summary>
  /// 定义游戏回合事件
  /// </summary>
  public class GameAroundEvent : GameEventBase, IGamePlayerEvent
  {

    /// <summary>
    /// 创建 GameAroundEvent 对象
    /// </summary>
    /// <param name="player">轮到出牌的玩家</param>
    public GameAroundEvent( CardGamePlayer player )
    {
      Player = player;
    }

    /// <summary>
    /// 获取当前轮到出牌的玩家
    /// </summary>
    public GamePlayerBase Player
    {
      get;
      private set;
    }


  }
}
