using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwelveCards.Core
{
  public sealed class Player
  {

    /// <summary>
    /// 玩家所处的舱位
    /// </summary>
    public Cabin Cabin { get; private set; }


    /// <summary>
    /// 玩家代号
    /// </summary>
    public string CodeName { get; private set; }

    /// <summary>
    /// 玩家控制台，用于显示游戏信息
    /// </summary>
    public PlayerConsole Console { get; private set; }


    /// <summary>
    /// 玩家所持有的卡牌
    /// </summary>
    public Card[] Cards { get; private set; }



  }
}
