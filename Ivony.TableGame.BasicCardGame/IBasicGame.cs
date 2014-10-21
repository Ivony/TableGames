using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  internal interface IBasicGame
  {


    /// <summary>
    /// 获取发牌器
    /// </summary>
    CardDealer CardDealer { get; }


    /// <summary>
    /// 当玩家退出游戏时，调用此方法通知游戏
    /// </summary>
    /// <param name="player">退出游戏的玩家</param>
    void OnPlayerQuitted( GamePlayerBase player );

  }
}
