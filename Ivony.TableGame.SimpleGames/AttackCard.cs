using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.TableGame.SimpleGames
{

  /// <summary>
  /// 攻击牌
  /// </summary>
  public class AttackCard : Card
  {
    public override string Name
    {
      get { return "攻击卡牌"; }
    }

    public override string Description
    {
      get { return "攻击任何一个玩家"; }
    }
  }
}
