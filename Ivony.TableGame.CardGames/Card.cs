using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.TableGame.CardGames
{


  /// <summary>
  /// 定义游戏卡牌的抽象
  /// </summary>
  public abstract class Card
  {

    /// <summary>
    /// 卡牌名称
    /// </summary>
    public abstract string Name { get; }


    /// <summary>
    /// 卡牌描述
    /// </summary>
    public abstract string Description { get; }



  }
}